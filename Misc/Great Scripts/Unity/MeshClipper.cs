
// Mesh Clipper Script
// by Kyle Furey

namespace Navigation
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;

    // Generate polygons from a mesh.
    public class MeshClipper : MonoBehaviour
    {
        [Header("Generate polygons from a mesh.")]

        // Instance (to prevent duplicate components)
        private static MeshClipper instance = null;

        [Header("Materials to assign to the generated meshes:")]
        [SerializeField] private List<Material> polygonColors = new List<Material>();
        public static List<Material> colors = new List<Material>();
        private static int currentColor = 0;

        [Header("Auto hide the clipped meshes:")]
        [SerializeField] private bool hideMeshes = false;
        public static bool autoHide = false;

        [Header("Auto enable the clipped mesh colliders:")]
        [SerializeField] private bool enableCollision = true;
        public static bool collisionEnabled = true;

        [Header("Polygon clipping debugging:")]
        [SerializeField] private List<GameObject> clippedObjects = new List<GameObject>();
        [SerializeField] private ClipType testClipType = ClipType.Convex;
        [SerializeField] private float testExplosionDistance = 0;
        [SerializeField] private float testAngle = 180;
        [SerializeField] private bool testClip = false;

        [Header("Polygon combining debugging:")]
        [SerializeField] private List<GameObject> combinedObjects = new List<GameObject>();
        [SerializeField] private bool testCombine = false;

        [Header("Polygon decimation debugging:")]
        [SerializeField] private bool decimateMeshes = true;
        [SerializeField] private float decimationDistance = 999;

        [Header("Clear generated meshes:")]
        [SerializeField] private bool clear = false;
        private static List<GameObject> meshes = new List<GameObject>();

        // Clip type enum
        public enum ClipType { Convex, Flatten, Triangulate };


        // STATIC VARIABLES

        // Game start
        private void Awake()
        {
            // Update static variables
            SetStaticVariables();
        }

        // Updates static variables
        private void SetStaticVariables()
        {
            // Store the colors in a static list
            colors = polygonColors;

            // Set automatic mesh hiding
            autoHide = hideMeshes;

            // Set collision
            collisionEnabled = enableCollision;
        }

        // Updating static variables and deleting duplicates
        private void OnValidate()
        {
            // Set instance
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.LogError("You cannot have more than one instance of a Mesh Clipper! Deleting the previous Mesh Clipper now . . .");

                instance.Invoke("Destroy", 0);

                instance = this;
            }

            // Update static variables
            SetStaticVariables();

            // Test polygon clipping
            if (testClip)
            {
                testClip = false;

                foreach (GameObject clippedObject in clippedObjects)
                {
                    GameObject tempClippedObject;

                    if (decimateMeshes)
                    {
                        tempClippedObject = Explode(Generate(Clip(testClipType, Discard(Decimate(GetMesh(clippedObject), decimationDistance), testAngle))), testExplosionDistance);
                    }
                    else
                    {
                        tempClippedObject = Explode(Generate(Clip(testClipType, Discard(GetMesh(clippedObject), testAngle))), testExplosionDistance);
                    }

                    tempClippedObject.transform.position = clippedObject.transform.position;

                    tempClippedObject.transform.eulerAngles = clippedObject.transform.eulerAngles;

                    tempClippedObject.transform.localScale = clippedObject.transform.localScale;
                }
            }

            // Test polygon combining
            if (testCombine)
            {
                testCombine = false;

                List<Vector3> worldPositions = new List<Vector3>();

                foreach (GameObject gameObject in combinedObjects)
                {
                    worldPositions.Add(gameObject.transform.position);
                }

                GameObject combinedMesh;

                if (decimateMeshes)
                {
                    combinedMesh = Generate(Decimate(Combine(GetMeshes(combinedObjects), worldPositions.ToArray()), decimationDistance));
                }
                else
                {
                    combinedMesh = Generate(Combine(GetMeshes(combinedObjects), worldPositions.ToArray()));
                }

                combinedMesh.transform.position = combinedObjects[0].transform.position;

                combinedMesh.transform.eulerAngles = combinedObjects[0].transform.eulerAngles;

                combinedMesh.transform.localScale = combinedObjects[0].transform.localScale;
            }

            // Clearing temporary meshes
            if (clear)
            {
                clear = false;

                Invoke("Clear", 0);
            }
        }

        // Destroy this component
        private void Destroy()
        {
            DestroyImmediate(this);
        }

        // Clear temporary generated meshes
        public void Clear()
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                DestroyImmediate(meshes[i]);
            }

            meshes.Clear();
        }


        // CLIPPING FUNCTIONS

        // Clips the given mesh based on the set clipping type
        public static List<Mesh> Clip(ClipType clipType, Mesh mesh)
        {
            switch (clipType)
            {
                case ClipType.Convex:
                    return Convex(mesh);

                case ClipType.Flatten:
                    return Flatten(mesh);

                case ClipType.Triangulate:
                    return Triangulate(mesh);

                default:
                    Debug.LogError("Could not clip the mesh! No clipping type was selected!");
                    return new List<Mesh>();
            }
        }

        // Clips all concave polygons so the remaining meshes are convex
        public static List<Mesh> Convex(Mesh mesh)
        {
            // Check if the mesh is null
            if (mesh == null)
            {
                Debug.LogError("Could not clip the mesh! No mesh was given!");
                return new List<Mesh>();
            }

            // Store our new meshes in a list
            List<Mesh> polygons = new List<Mesh>(mesh.triangles.Length / 3);

            // Check if the mesh is convex
            if (IsConvex(mesh.vertices, mesh.triangles))
            {
                polygons.Add(mesh);
                return polygons;
            }

            // Store our remaining triangles
            List<int> trianglesLeft = new List<int>(mesh.triangles.Length);

            for (int i = 0; i < mesh.triangles.Length; i++)
            {
                trianglesLeft.Add(mesh.triangles[i]);
            }

            // Loop through the mesh's remaining triangles
            while (trianglesLeft.Count > 0)
            {
                // Create a new mesh
                Mesh newMesh = new Mesh();

                // Add the next triangle's vertices
                List<Vector3> newVertices = new List<Vector3>
            {
                mesh.vertices[trianglesLeft[0]],
                mesh.vertices[trianglesLeft[1]],
                mesh.vertices[trianglesLeft[2]]
            };

                // Add the next triangle's triangle
                List<int> newTriangles = new List<int>
            {
                0,
                1,
                2
            };

                // Add the next triangle's normal
                List<Vector3> newNormals = new List<Vector3>
            {
                mesh.normals[trianglesLeft[0]],
                mesh.normals[trianglesLeft[1]],
                mesh.normals[trianglesLeft[2]]
            };

                // Dequeue our next triangle
                trianglesLeft.RemoveAt(0);
                trianglesLeft.RemoveAt(0);
                trianglesLeft.RemoveAt(0);

                // Generate a convex polygon by combining meshes
                for (int i = 0; i < trianglesLeft.Count; i += 3)
                {
                    // Check if the next triangle is adjacent to our mesh
                    bool matches = false;

                    if (newVertices.Contains(mesh.vertices[trianglesLeft[i]]))
                    {
                        matches = true;
                    }
                    else if (newVertices.Contains(mesh.vertices[trianglesLeft[i + 1]]))
                    {
                        matches = true;
                    }
                    else if (newVertices.Contains(mesh.vertices[trianglesLeft[i + 2]]))
                    {
                        matches = true;
                    }

                    // Check if we found a matching vertex
                    if (matches)
                    {
                        // Make temporary mesh data of the current mesh with the next triangle
                        Vector3[] tempVertices = new Vector3[newVertices.Count + 3];
                        int[] tempTriangles = new int[newTriangles.Count + 3];

                        for (int j = 0; j < newVertices.Count; j++)
                        {
                            tempVertices[j] = newVertices[j];
                        }

                        for (int j = 0; j < newTriangles.Count; j++)
                        {
                            tempTriangles[j] = newTriangles[j];
                        }

                        tempVertices[tempVertices.Length - 3] = mesh.vertices[trianglesLeft[i]];
                        tempVertices[tempVertices.Length - 2] = mesh.vertices[trianglesLeft[i + 1]];
                        tempVertices[tempVertices.Length - 1] = mesh.vertices[trianglesLeft[i + 2]];

                        tempTriangles[tempTriangles.Length - 3] = newTriangles.Count;
                        tempTriangles[tempTriangles.Length - 2] = newTriangles.Count + 1;
                        tempTriangles[tempTriangles.Length - 1] = newTriangles.Count + 2;

                        // Check if the new mesh would be convex
                        if (IsConvex(tempVertices, tempTriangles))
                        {
                            // Add our next triangle's data to the new mesh
                            newVertices.Add(mesh.vertices[trianglesLeft[i]]);
                            newVertices.Add(mesh.vertices[trianglesLeft[i + 1]]);
                            newVertices.Add(mesh.vertices[trianglesLeft[i + 2]]);

                            newTriangles.Add(newTriangles.Count);
                            newTriangles.Add(newTriangles.Count);
                            newTriangles.Add(newTriangles.Count);

                            newNormals.Add(mesh.normals[trianglesLeft[i]]);
                            newNormals.Add(mesh.normals[trianglesLeft[i + 1]]);
                            newNormals.Add(mesh.normals[trianglesLeft[i + 2]]);

                            // Dequeue our next triangle
                            trianglesLeft.RemoveAt(i);
                            trianglesLeft.RemoveAt(i);
                            trianglesLeft.RemoveAt(i);

                            // Reset i to recalculate the rest of the mesh. Accounts for the event this new point makes another point convex.
                            i = -3;
                        }
                    }
                }

                // Add our mesh data to the mesh
                newMesh.vertices = newVertices.ToArray();
                newMesh.triangles = newTriangles.ToArray();
                newMesh.normals = newNormals.ToArray();

                // Name our new mesh
                newMesh.name = "Convex Polygon";

                // Add our new mesh
                polygons.Add(newMesh);
            }

            // Return our new meshes
            return polygons;
        }

        // Clips all polygons so the remaining meshes are all flat
        public static List<Mesh> Flatten(Mesh mesh)
        {
            // Check if the mesh is null
            if (mesh == null)
            {
                Debug.LogError("Could not clip the mesh! No mesh was given!");
                return new List<Mesh>();
            }

            // Store our new meshes in a list
            List<Mesh> polygons = new List<Mesh>(mesh.triangles.Length / 3);

            // Check if the mesh is convex
            if (IsFlat(mesh.vertices, mesh.triangles))
            {
                polygons.Add(mesh);
                return polygons;
            }

            // Store our remaining triangles
            List<int> trianglesLeft = new List<int>(mesh.triangles.Length);

            for (int i = 0; i < mesh.triangles.Length; i++)
            {
                trianglesLeft.Add(mesh.triangles[i]);
            }

            // Loop through the mesh's remaining triangles
            while (trianglesLeft.Count > 0)
            {
                // Create a new mesh
                Mesh newMesh = new Mesh();

                // Add the next triangle's vertices
                List<Vector3> newVertices = new List<Vector3>
            {
                mesh.vertices[trianglesLeft[0]],
                mesh.vertices[trianglesLeft[1]],
                mesh.vertices[trianglesLeft[2]]
            };

                // Add the next triangle's triangle
                List<int> newTriangles = new List<int>
            {
                0,
                1,
                2
            };

                // Add the next triangle's normal
                List<Vector3> newNormals = new List<Vector3>
            {
                mesh.normals[trianglesLeft[0]],
                mesh.normals[trianglesLeft[1]],
                mesh.normals[trianglesLeft[2]]
            };

                // Dequeue our next triangle
                trianglesLeft.RemoveAt(0);
                trianglesLeft.RemoveAt(0);
                trianglesLeft.RemoveAt(0);

                // Generate a convex polygon by combining meshes
                for (int i = 0; i < trianglesLeft.Count; i += 3)
                {
                    // Check if the next triangle is adjacent to our mesh
                    bool matches = false;

                    if (newVertices.Contains(mesh.vertices[trianglesLeft[i]]))
                    {
                        matches = true;
                    }
                    else if (newVertices.Contains(mesh.vertices[trianglesLeft[i + 1]]))
                    {
                        matches = true;
                    }
                    else if (newVertices.Contains(mesh.vertices[trianglesLeft[i + 2]]))
                    {
                        matches = true;
                    }

                    // Check if we found a matching vertex
                    if (matches)
                    {
                        // Make temporary mesh data of the current mesh with the next triangle
                        Vector3[] tempVertices = new Vector3[newVertices.Count + 3];
                        int[] tempTriangles = new int[newTriangles.Count + 3];

                        for (int j = 0; j < newVertices.Count; j++)
                        {
                            tempVertices[j] = newVertices[j];
                        }

                        for (int j = 0; j < newTriangles.Count; j++)
                        {
                            tempTriangles[j] = newTriangles[j];
                        }

                        tempVertices[tempVertices.Length - 3] = mesh.vertices[trianglesLeft[i]];
                        tempVertices[tempVertices.Length - 2] = mesh.vertices[trianglesLeft[i + 1]];
                        tempVertices[tempVertices.Length - 1] = mesh.vertices[trianglesLeft[i + 2]];

                        tempTriangles[tempTriangles.Length - 3] = newTriangles.Count;
                        tempTriangles[tempTriangles.Length - 2] = newTriangles.Count + 1;
                        tempTriangles[tempTriangles.Length - 1] = newTriangles.Count + 2;

                        // Check if the new mesh would be convex
                        if (IsFlat(tempVertices, tempTriangles))
                        {
                            // Add our next triangle's data to the new mesh
                            newVertices.Add(mesh.vertices[trianglesLeft[i]]);
                            newVertices.Add(mesh.vertices[trianglesLeft[i + 1]]);
                            newVertices.Add(mesh.vertices[trianglesLeft[i + 2]]);

                            newTriangles.Add(newTriangles.Count);
                            newTriangles.Add(newTriangles.Count);
                            newTriangles.Add(newTriangles.Count);

                            newNormals.Add(mesh.normals[trianglesLeft[i]]);
                            newNormals.Add(mesh.normals[trianglesLeft[i + 1]]);
                            newNormals.Add(mesh.normals[trianglesLeft[i + 2]]);

                            // Dequeue our next triangle
                            trianglesLeft.RemoveAt(i);
                            trianglesLeft.RemoveAt(i);
                            trianglesLeft.RemoveAt(i);

                            // Reset i to recalculate the rest of the mesh. Accounts for the event this new point makes another point convex.
                            i = -3;
                        }
                    }
                }

                // Add our mesh data to the mesh
                newMesh.vertices = newVertices.ToArray();
                newMesh.triangles = newTriangles.ToArray();
                newMesh.normals = newNormals.ToArray();

                // Name our new mesh
                newMesh.name = "Convex Polygon";

                // Add our new mesh
                polygons.Add(newMesh);
            }

            // Return our new meshes
            return polygons;
        }

        // Clips all polygons so the remaining meshes are triangles
        public static List<Mesh> Triangulate(Mesh mesh)
        {
            // Check if the mesh is null
            if (mesh == null)
            {
                Debug.LogError("Could not clip the mesh! No mesh was given!");
                return new List<Mesh>();
            }

            // Store our new meshes in a list
            List<Mesh> polygons = new List<Mesh>(mesh.triangles.Length / 3);

            // Loop through the mesh's triangles
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                // Create a new mesh
                Mesh newMesh = new Mesh();

                // Assemble the new mesh vertices
                Vector3[] newVertices = new Vector3[3];
                newVertices[0] = mesh.vertices[mesh.triangles[i]];
                newVertices[1] = mesh.vertices[mesh.triangles[i + 1]];
                newVertices[2] = mesh.vertices[mesh.triangles[i + 2]];
                newMesh.vertices = newVertices;

                // Assemble the new mesh triangles
                int[] newTriangles = new int[3];
                newTriangles[0] = 0;
                newTriangles[1] = 1;
                newTriangles[2] = 2;
                newMesh.triangles = newTriangles;

                // Assemble the new mesh normals
                Vector3[] newNormals = new Vector3[3];
                newNormals[0] = mesh.normals[mesh.triangles[i]];
                newNormals[1] = mesh.normals[mesh.triangles[i + 1]];
                newNormals[2] = mesh.normals[mesh.triangles[i + 2]];
                newMesh.normals = newNormals;

                // Name our new mesh
                newMesh.name = "Triangle";

                // Add our new mesh
                polygons.Add(newMesh);
            }

            // Return our new meshes
            return polygons;
        }


        // COMBINING FUNCTIONS

        // Combine two meshes using union combining
        public static Mesh Combine(Mesh meshA, Vector3 worldPositionA, Mesh meshB, Vector3 worldPositionB)
        {
            // Check if the meshes are missing
            if (meshA == null || meshB == null)
            {
                Debug.LogError("Could not combine the meshes! A mesh is missing!");
                return null;
            }

            // Store our new mesh
            Mesh newMesh = new Mesh();

            // Store our new mesh data
            List<Vector3> newVertices = new List<Vector3>(meshA.vertices.Length + meshB.vertices.Length);
            List<int> newTriangles = new List<int>(meshA.triangles.Length + meshB.triangles.Length);
            List<Vector3> newNormals = new List<Vector3>(meshA.normals.Length + meshB.normals.Length);

            // Add the mesh data of Mesh A
            for (int i = 0; i < meshA.vertices.Length; i++)
            {
                newVertices.Add(meshA.vertices[i]);
            }

            for (int i = 0; i < meshA.triangles.Length; i++)
            {
                newTriangles.Add(meshA.triangles[i]);
            }

            for (int i = 0; i < meshA.normals.Length; i++)
            {
                newNormals.Add(meshA.normals[i]);
            }

            // Add the mesh data of Mesh B offset by the distance
            Vector3 distance = (worldPositionB - worldPositionA) / 2;

            for (int i = 0; i < meshB.vertices.Length; i++)
            {
                newVertices.Add(meshB.vertices[i] + distance);
            }

            for (int i = 0; i < meshB.triangles.Length; i++)
            {
                newTriangles.Add(meshB.triangles[i] + meshA.vertices.Length);
            }

            for (int i = 0; i < meshB.normals.Length; i++)
            {
                newNormals.Add(meshB.normals[i]);
            }

            // Set our mesh data
            newMesh.vertices = newVertices.ToArray();
            newMesh.triangles = newTriangles.ToArray();
            newMesh.normals = newNormals.ToArray();

            // Name our new mesh
            newMesh.name = meshA.name + " and " + meshB.name;

            // Return our new mesh
            return newMesh;
        }

        // Combine two or more meshes using union combining
        public static Mesh Combine(List<Mesh> meshes, params Vector3[] worldPositions)
        {
            // Check if the meshes are missing
            if (meshes.Count < 2)
            {
                Debug.LogError("Could not generate a mesh object! Not enough meshes were given!");
                return null;
            }

            // Check if we do not have enough world positions
            if (worldPositions.Length < meshes.Count)
            {
                Debug.LogError("Could not generate a mesh object! Not enough world positions were given!");
                return null;
            }

            // Store our new mesh
            Mesh newMesh = meshes[0];

            for (int i = 1; i < meshes.Count; i++)
            {
                newMesh = Combine(newMesh, worldPositions[0], meshes[i], worldPositions[i]);
            }

            // Return our new mesh
            return newMesh;
        }


        // MESH FUNCTIONS

        // Get a game object's mesh
        public static Mesh GetMesh(GameObject gameObject)
        {
            // Check if the game object is missing
            if (gameObject == null)
            {
                Debug.LogError("Could not retrieve a mesh! No game object was given!");
                return null;
            }

            // Get the mesh filter
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();

            // Check if the mesh filter is missing
            if (filter == null)
            {
                Debug.LogError("Could not retrieve a mesh! No mesh filter was found!");
                return null;
            }

            // Return the found mesh
            return filter.sharedMesh;
        }

        // Get a list of game objects' meshes
        public static List<Mesh> GetMeshes(List<GameObject> gameObjects)
        {
            // Make a list of meshes
            List<Mesh> meshes = new List<Mesh>();

            // Loop through the game objects
            for (int i = 0; i < gameObjects.Count; i++)
            {
                meshes.Add(GetMesh(gameObjects[i]));
            }

            // Return the found mesh
            return meshes;
        }

        // Set a game object's mesh
        public static GameObject SetMesh(GameObject gameObject, Mesh mesh)
        {
            // Store our components
            MeshFilter filter = gameObject.GetComponentInChildren<MeshFilter>();
            MeshCollider collider = gameObject.GetComponentInChildren<MeshCollider>();

            // Check our mesh filter
            if (filter != null)
            {
                // Update our mesh filter's mesh
                filter.sharedMesh = mesh;
            }

            // Check our mesh collider
            if (collider == null)
            {
                // Update our mesh collider's mesh
                collider.sharedMesh = filter.sharedMesh;
            }

            return gameObject;
        }

        // Discard mesh normals that are not within a specific range of rotation
        public static Mesh Discard(Mesh mesh, float maxAngle)
        {
            // Check if the mesh is null
            if (mesh == null)
            {
                Debug.LogError("Could not discard any triangles! No mesh was given!");
                return null;
            }

            // Create a new mesh
            Mesh newMesh = new Mesh();

            // Store our mesh data
            List<Vector3> newVertices = new List<Vector3>(mesh.vertices.Length);
            List<int> newTriangles = new List<int>(mesh.triangles.Length);
            List<Vector3> newNormals = new List<Vector3>(mesh.normals.Length);

            // Remove triangles that do not match the criteria
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                // Calculate the triangle normal
                Vector3 meshNormal = GetNormal(mesh.vertices[mesh.triangles[i]], mesh.vertices[mesh.triangles[i + 1]], mesh.vertices[mesh.triangles[i + 2]]);

                // Check the normal of the triangle
                if (Vector3.Angle(Vector3.up, meshNormal) <= maxAngle)
                {
                    // Add that triangle
                    newVertices.Add(mesh.vertices[mesh.triangles[i]]);
                    newVertices.Add(mesh.vertices[mesh.triangles[i + 1]]);
                    newVertices.Add(mesh.vertices[mesh.triangles[i + 2]]);

                    newNormals.Add(mesh.normals[mesh.triangles[i]]);
                    newNormals.Add(mesh.normals[mesh.triangles[i + 1]]);
                    newNormals.Add(mesh.normals[mesh.triangles[i + 2]]);

                    newTriangles.Add(newTriangles.Count);
                    newTriangles.Add(newTriangles.Count);
                    newTriangles.Add(newTriangles.Count);
                }
            }

            // Set our mesh data
            newMesh.vertices = newVertices.ToArray();
            newMesh.triangles = newTriangles.ToArray();
            newMesh.normals = newNormals.ToArray();

            // Name our new mesh
            newMesh.name = "Clipped " + mesh.name;

            // Return our updated list of polygons
            return newMesh;
        }

        // Discard mesh normals that are not within a specific range of rotation
        public static Mesh Discard(Mesh mesh, float maxAngle, Vector3 direction)
        {
            // Check if the mesh is null
            if (mesh == null)
            {
                Debug.LogError("Could not discard any triangles! No mesh was given!");
                return null;
            }

            // Create a new mesh
            Mesh newMesh = new Mesh();

            // Store our mesh data
            List<Vector3> newVertices = new List<Vector3>(mesh.vertices.Length);
            List<int> newTriangles = new List<int>(mesh.triangles.Length);
            List<Vector3> newNormals = new List<Vector3>(mesh.normals.Length);

            // Remove triangles that do not match the criteria
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                // Calculate the triangle normal
                Vector3 meshNormal = GetNormal(mesh.vertices[mesh.triangles[i]], mesh.vertices[mesh.triangles[i + 1]], mesh.vertices[mesh.triangles[i + 2]]);

                // Check the normal of the triangle
                if (Vector3.Angle(direction, meshNormal) <= maxAngle)
                {
                    // Add that triangle
                    newVertices.Add(mesh.vertices[mesh.triangles[i]]);
                    newVertices.Add(mesh.vertices[mesh.triangles[i + 1]]);
                    newVertices.Add(mesh.vertices[mesh.triangles[i + 2]]);

                    newNormals.Add(mesh.normals[mesh.triangles[i]]);
                    newNormals.Add(mesh.normals[mesh.triangles[i + 1]]);
                    newNormals.Add(mesh.normals[mesh.triangles[i + 2]]);

                    newTriangles.Add(newTriangles.Count);
                    newTriangles.Add(newTriangles.Count);
                    newTriangles.Add(newTriangles.Count);
                }
            }

            // Set our mesh data
            newMesh.vertices = newVertices.ToArray();
            newMesh.triangles = newTriangles.ToArray();
            newMesh.normals = newNormals.ToArray();

            // Name our new mesh
            newMesh.name = "Clipped " + mesh.name;

            // Return our updated list of polygons
            return newMesh;
        }

        // Discard mesh normals that are not within a specific range of rotation
        public static List<Mesh> Discard(List<Mesh> polygons, float maxAngle)
        {
            // Check if the list of polygons are empty
            if (polygons.Count == 0)
            {
                Debug.LogError("Could not discard any meshes! No meshes were found!");
                return null;
            }

            // Remove triangles that do not match the criteria
            for (int i = 0; i < polygons.Count; i++)
            {
                // Calculate the triangle normal
                Vector3 meshNormal = GetNormal(polygons[i]);

                // Check the normal of the triangle
                if (Vector3.Angle(Vector3.up, meshNormal) > maxAngle)
                {
                    // Remove that triangle
                    polygons.RemoveAt(i);

                    i--;
                }
            }

            // Return our updated list of polygons
            return polygons;
        }

        // Discard mesh normals that are not within a specific range of rotation
        public static List<Mesh> Discard(List<Mesh> polygons, float maxAngle, Vector3 direction)
        {
            // Check if the list of polygons are empty
            if (polygons.Count == 0)
            {
                Debug.LogError("Could not discard any meshes! No meshes were found!");
                return null;
            }

            // Remove triangles that do not match the criteria
            for (int i = 0; i < polygons.Count; i++)
            {
                // Calculate the triangle normal
                Vector3 meshNormal = GetNormal(polygons[i]);

                // Check the normal of the triangle
                if (Vector3.Angle(direction, meshNormal) > maxAngle)
                {
                    // Remove that triangle
                    polygons.RemoveAt(i);

                    i--;
                }
            }

            // Return our updated list of polygons
            return polygons;
        }

        // Remove duplicate vertices and triangles from a mesh
        public static Mesh Clean(Mesh mesh)
        {
            // Check if the mesh is missing
            if (mesh == null)
            {
                Debug.LogError("Could not clean the mesh! No mesh was given!");
                return null;
            }

            // Store our new mesh
            Mesh newMesh = new Mesh();

            // Store our new mesh data
            List<Vector3> newVertices = new List<Vector3>(mesh.vertices.Length);
            List<int> newTriangles = new List<int>(mesh.triangles.Length);
            List<Vector3> newNormals = new List<Vector3>(mesh.normals.Length);

            // Add the mesh data
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                newVertices.Add(mesh.vertices[i]);
            }

            for (int i = 0; i < mesh.triangles.Length; i++)
            {
                newTriangles.Add(mesh.triangles[i]);
            }

            for (int i = 0; i < mesh.normals.Length; i++)
            {
                newNormals.Add(mesh.normals[i]);
            }

            // Loop through each vertice and similar ones
            for (int i = 0; i < newVertices.Count; i++)
            {
                // Store the next vertice
                Vector3 vertice = newVertices[i];

                for (int j = 0; j < newVertices.Count; j++)
                {
                    // Ignore this vertice
                    if (i == j)
                    {
                        continue;
                    }

                    // Check the similarity of the points by distance
                    if (vertice == newVertices[j])
                    {
                        // Remove the similar vertice and its normal
                        Vector3 removedPoint = newVertices[j];
                        newVertices.RemoveAt(j);
                        newNormals.RemoveAt(j);

                        // Repoint the triangles of that vertice to the stored vertice
                        for (int k = 0; k < newTriangles.Count; k++)
                        {
                            if (newTriangles[k] > j)
                            {
                                newTriangles[k]--;
                            }
                            else if (newTriangles[k] == j)
                            {
                                newTriangles[k] = i;
                            }
                        }

                        j--;
                    }
                }
            }

            // Loop through each triangle and remove duplicates
            for (int i = 0; i < newTriangles.Count; i += 3)
            {
                // Dequeue the vertice
                int x = newTriangles[i];
                int y = newTriangles[i + 1];
                int z = newTriangles[i + 2];

                for (int j = 0; j < newTriangles.Count; j += 3)
                {
                    // Ignore this triangle
                    if (i == j)
                    {
                        continue;
                    }

                    // Check if the triangles are identical
                    if ((x == newTriangles[j] && y == newTriangles[j + 1] && z == newTriangles[j + 2]) ||
                        (x == newTriangles[j] && z == newTriangles[j + 1] && y == newTriangles[j + 2]) ||
                        (y == newTriangles[j] && x == newTriangles[j + 1] && z == newTriangles[j + 2]) ||
                        (y == newTriangles[j] && z == newTriangles[j + 1] && x == newTriangles[j + 2]) ||
                        (z == newTriangles[j] && x == newTriangles[j + 1] && y == newTriangles[j + 2]) ||
                        (z == newTriangles[j] && y == newTriangles[j + 1] && x == newTriangles[j + 2]))
                    {
                        // Remove the duplicate triangle
                        newTriangles.RemoveAt(j);
                        newTriangles.RemoveAt(j);
                        newTriangles.RemoveAt(j);

                        j -= 3;
                    }
                }
            }

            // Set our mesh data
            newMesh.vertices = newVertices.ToArray();
            newMesh.triangles = newTriangles.ToArray();
            newMesh.normals = newNormals.ToArray();

            // Name our new mesh
            newMesh.name = mesh.name;

            // Return our new mesh
            return newMesh;
        }

        // Remove vertices without affecting the border shape of the mesh
        public static Mesh Decimate(Mesh mesh, float maxPointDistance)
        {
            // Check if the mesh is missing
            if (mesh == null)
            {
                Debug.LogError("Could not decimate the mesh! No mesh was given!");
                return null;
            }

            // Get the border vertices of the mesh
            List<Vector3> border = GetBorder(mesh);

            // Store our new mesh
            Mesh newMesh = new Mesh();

            // Store our new mesh data
            List<Vector3> newVertices = new List<Vector3>(mesh.vertices.Length);
            List<int> newTriangles = new List<int>(mesh.triangles.Length);
            List<Vector3> newNormals = new List<Vector3>(mesh.normals.Length);

            // Add the mesh data
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                newVertices.Add(mesh.vertices[i]);
            }

            for (int i = 0; i < mesh.triangles.Length; i++)
            {
                newTriangles.Add(mesh.triangles[i]);
            }

            for (int i = 0; i < mesh.normals.Length; i++)
            {
                newNormals.Add(mesh.normals[i]);
            }

            // Loop through each vertice and similar ones
            for (int i = 0; i < newVertices.Count; i++)
            {
                // Store the next vertice
                Vector3 vertice = newVertices[i];

                for (int j = 0; j < newVertices.Count; j++)
                {
                    // Ignore this vertice
                    if (i == j)
                    {
                        continue;
                    }

                    // Check the similarity of the points by distance
                    if (DistanceSquared(vertice, newVertices[j]) <= maxPointDistance * maxPointDistance && !border.Contains(newVertices[j]))
                    {
                        // Remove the similar vertice and its normal
                        Vector3 removedPoint = newVertices[j];
                        newVertices.RemoveAt(j);
                        newNormals.RemoveAt(j);

                        // Repoint the triangles of that vertice to the stored vertice
                        for (int k = 0; k < newTriangles.Count; k++)
                        {
                            if (newTriangles[k] > j)
                            {
                                newTriangles[k]--;
                            }
                            else if (newTriangles[k] == j)
                            {
                                newTriangles[k] = GetNearestVertice(removedPoint, newVertices);
                            }
                        }

                        i = 0;

                        break;
                    }
                }
            }

            // Loop through each triangle and remove duplicates
            for (int i = 0; i < newTriangles.Count; i += 3)
            {
                // Dequeue the vertice
                int x = newTriangles[i];
                int y = newTriangles[i + 1];
                int z = newTriangles[i + 2];

                for (int j = 0; j < newTriangles.Count; j += 3)
                {
                    // Ignore this triangle
                    if (i == j)
                    {
                        continue;
                    }

                    // Check if the triangles are identical
                    if ((x == newTriangles[j] && y == newTriangles[j + 1] && z == newTriangles[j + 2]) ||
                        (x == newTriangles[j] && z == newTriangles[j + 1] && y == newTriangles[j + 2]) ||
                        (y == newTriangles[j] && x == newTriangles[j + 1] && z == newTriangles[j + 2]) ||
                        (y == newTriangles[j] && z == newTriangles[j + 1] && x == newTriangles[j + 2]) ||
                        (z == newTriangles[j] && x == newTriangles[j + 1] && y == newTriangles[j + 2]) ||
                        (z == newTriangles[j] && y == newTriangles[j + 1] && x == newTriangles[j + 2]))
                    {
                        // Remove the duplicate triangle
                        newTriangles.RemoveAt(j);
                        newTriangles.RemoveAt(j);
                        newTriangles.RemoveAt(j);

                        j -= 3;
                    }
                }
            }

            Debug.Log(mesh.name + " was successfully decimated from " + mesh.vertices.Length + " vertices and " + mesh.triangles.Length + " triangles to " + newVertices.Count + " vertices and " + newTriangles.Count + " triangles.");

            // Set our mesh data
            newMesh.vertices = newVertices.ToArray();
            newMesh.triangles = newTriangles.ToArray();
            newMesh.normals = newNormals.ToArray();

            // Name our new mesh
            newMesh.name = "Simplified " + mesh.name;

            // Return our new mesh
            return newMesh;
        }


        // MESH OBJECT FUNCTIONS

        // Create a new game object from a mesh
        public static GameObject Generate(Mesh mesh)
        {
            // Create this polygon as a game object
            GameObject gameObject = new GameObject("Mesh");

            // Add a mesh filter
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();

            // Update our mesh filter's mesh
            filter.sharedMesh = mesh;

            // Add a mesh renderer
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();

            // Update our mesh renderer's color
            if (colors.Count != 0)
            {
                renderer.material = colors[currentColor];
            }

            // Automatically hide the mesh renderer
            renderer.enabled = !autoHide;

            // Increment the color
            currentColor = (currentColor == colors.Count - 1) ? 0 : (currentColor + 1);

            // Add a mesh collider
            MeshCollider collider = gameObject.AddComponent<MeshCollider>();

            // Update our mesh collider's mesh
            collider.sharedMesh = filter.sharedMesh;

            // Automatically enable the mesh collision
            collider.enabled = collisionEnabled;

            // Return our new game object
            return gameObject;
        }

        // Generates a mesh object from a list of polygons
        public static GameObject Generate(List<Mesh> polygons)
        {
            // Check if the list of polygons are empty
            if (polygons.Count == 0)
            {
                Debug.LogError("Could not generate a mesh object! No poly was given!");
                return null;
            }

            // Create our mesh object
            GameObject meshObject = new GameObject("Mesh");

            // Loop through each polygon
            foreach (Mesh mesh in polygons)
            {
                // Create this polygon as a game object
                GameObject polygon = new GameObject("Polygon");

                // Parent the polygon to the mesh object
                polygon.transform.parent = meshObject.transform;

                // Add a mesh filter
                MeshFilter filter = polygon.AddComponent<MeshFilter>();

                // Update our mesh filter's mesh
                filter.sharedMesh = mesh;

                // Add a mesh renderer
                MeshRenderer renderer = polygon.AddComponent<MeshRenderer>();

                // Update our mesh renderer's color
                if (colors.Count != 0)
                {
                    renderer.material = colors[currentColor];
                }

                // Automatically hide the mesh renderer
                renderer.enabled = !autoHide;

                // Increment the color
                currentColor = (currentColor == colors.Count - 1) ? 0 : (currentColor + 1);

                // Add a mesh collider
                MeshCollider collider = polygon.AddComponent<MeshCollider>();

                // Update our mesh collider's mesh
                collider.sharedMesh = filter.sharedMesh;

                // Automatically enable the mesh collision
                collider.enabled = collisionEnabled;
            }

            // Get unused mesh object components
            MeshRenderer meshRenderer = meshObject.GetComponent<MeshRenderer>();
            MeshCollider meshCollider = meshObject.GetComponent<MeshCollider>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            if (meshCollider != null)
            {
                meshCollider.enabled = false;
            }

            // Add this mesh to the list of generated meshes
            meshes.Add(meshObject);

            // Return our parent object
            return meshObject;
        }

        // Explodes a mesh object by moving each child object by a set distance by its normal direction
        public static GameObject Explode(GameObject meshObject, float distance)
        {
            // Check if the mesh object is missing
            if (meshObject == null)
            {
                Debug.LogError("Could not explode the mesh object! No mesh object was given!");
                return meshObject;
            }

            // Check if the list of polygons are empty
            if (meshObject.transform.childCount == 0)
            {
                Debug.LogWarning("Could not explode the mesh object! The mesh object has no children!");
                return meshObject;
            }

            // Translate each mesh object's mesh
            for (int i = 0; i < meshObject.transform.childCount; i++)
            {
                meshObject.transform.GetChild(i).Translate(GetNormal(meshObject.transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh) * distance);
            }

            // Return our parent object
            return meshObject;
        }

        // Gets a list of a mesh object's children
        public static List<GameObject> GetObjects(GameObject meshObject)
        {
            // Check if the mesh object is missing
            if (meshObject == null)
            {
                Debug.LogError("Could not explode the mesh object! No mesh object was given!");
                return null;
            }

            // Check if the list of polygons are empty
            if (meshObject.transform.childCount == 0)
            {
                Debug.LogWarning("Could not explode the mesh object! The mesh object has no children!");
                return null;
            }

            // Make a list of the children
            List<GameObject> children = new List<GameObject>();

            for (int i = 0; i < meshObject.transform.childCount; i++)
            {
                children.Add(meshObject.transform.GetChild(i).gameObject);
            }

            // Return the mesh object's meshes
            return children;
        }


        // OTHER FUNCTIONS

        // Returns the cross product of a vector 3
        public static float CrossProduct(Vector3 pointA, Vector3 pointB, Vector3 pointC)
        {
            return (pointB.x - pointA.x) * (pointC.y - pointA.y) - (pointC.x - pointA.x) * (pointB.y - pointA.y);
        }

        // Returns whether the new point is convex relative to a given mesh
        public static bool IsConvex(Vector3[] vertices, int[] triangles)
        {
            // Loop through triangles
            for (int i = 0; i < triangles.Length; i += 3)
            {
                // Make triangle
                Plane triangle = new Plane(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]);

                // Loop through points
                for (int j = 0; j < vertices.Length; j++)
                {
                    // Ignore the plane's points
                    if (j == triangles[i] || j == triangles[i + 1] || j == triangles[i + 2])
                    {
                        continue;
                    }

                    // Check that the point is not outside the triangle
                    if (triangle.GetSide(vertices[j]))
                    {
                        // A point is concave
                        return false;
                    }
                }
            }

            // All points are convex
            return true;
        }

        // Returns whether the new point is on the same side as a given mesh
        public static bool IsFlat(Vector3[] vertices, int[] triangles)
        {
            // Loop through triangles
            for (int i = 0; i < triangles.Length; i += 3)
            {
                // Make triangle
                Plane triangle = new Plane(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]);

                // Loop through points
                for (int j = 0; j < vertices.Length; j++)
                {
                    // Ignore the plane's points
                    if (j == triangles[i] || j == triangles[i + 1] || j == triangles[i + 2])
                    {
                        continue;
                    }

                    // Check that the point is not outside the triangle
                    if (triangle.GetDistanceToPoint(vertices[j]) < 0)
                    {
                        // A point is not the same side
                        return false;
                    }
                }
            }

            // All points are the same side
            return true;
        }

        // Get a normal from 3 points
        public static Vector3 GetNormal(Vector3 pointA, Vector3 pointB, Vector3 pointC)
        {
            Plane plane = new Plane(pointA, pointB, pointC);

            return plane.normal;
        }

        // Get a normal from a triangle
        public static Vector3 GetNormal(Mesh triangle)
        {
            Plane plane = new Plane(triangle.vertices[0], triangle.vertices[1], triangle.vertices[2]);

            return plane.normal;
        }

        // Gets only the border vertices of a mesh
        // SOURCE https://forum.unity.com/threads/obtain-border-vertices-of-triangulated-mesh.1232688/ by Cameron_SM
        public static List<Vector3> GetBorder(Mesh mesh)
        {
            // Remove duplicate vertices and triangles
            Mesh newMesh = Clean(mesh);

            // Store our list of vertices
            List<Vector3> border = new List<Vector3>();

            // Store our edges (two triangle indicies) in a dictionary, with the value representing if it is a border edge
            Dictionary<(int, int), bool> edges = new Dictionary<(int, int), bool>();

            for (int i = 0; i < newMesh.triangles.Length; i += 3)
            {
                // Store a triangle
                int x = newMesh.triangles[i];
                int y = newMesh.triangles[i + 1];
                int z = newMesh.triangles[i + 2];

                // Calculate the edges (with lower triangle first)
                (int, int) xEdge = x < y ? (x, y) : (y, x);
                (int, int) yEdge = y < z ? (y, z) : (z, y);
                (int, int) zEdge = z < x ? (z, x) : (x, z);

                // Add the edge to the dictionary and if it is a border edge
                edges[xEdge] = !edges.ContainsKey(xEdge);
                edges[yEdge] = !edges.ContainsKey(yEdge);
                edges[zEdge] = !edges.ContainsKey(zEdge);
            }

            foreach ((int, int) edge in edges.Keys)
            {
                if (edges[edge])
                {
                    border.Add(newMesh.vertices[edge.Item1]);
                    border.Add(newMesh.vertices[edge.Item2]);
                }
            }

            return border;
        }

        // Returns the squared distance between two vector 3s
        public static float DistanceSquared(Vector3 pointA, Vector3 pointB)
        {
            float xDistance = pointA.x - pointB.x;
            float yDistance = pointA.y - pointB.y;
            float zDistance = pointA.z - pointB.z;

            return xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
        }

        // Find the nearest vertice from a list of other vertices, return its index
        public static int GetNearestVertice(Vector3 point, List<Vector3> otherPoints)
        {
            int index = 0;

            float distance = Mathf.Infinity;

            for (int i = 0; i < otherPoints.Count; i++)
            {
                float newDistance = DistanceSquared(point, otherPoints[i]);

                if (newDistance <= distance * distance)
                {
                    index = i;

                    distance = newDistance;
                }
            }

            return index;
        }

        // Get the direct center of a list of points
        public static Vector3 GetCenter(Vector3[] vertices)
        {
            if (vertices.Length == 0)
            {
                return Vector3.zero;
            }

            Vector3 centerPoint = Vector3.zero;

            foreach (Vector3 vertice in vertices)
            {
                centerPoint += vertice;
            }

            return centerPoint / vertices.Length;
        }
    }
}
