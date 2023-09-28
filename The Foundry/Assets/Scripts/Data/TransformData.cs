using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [System.Serializable]
    public class TransformData : ComponentData
    {
        [Header("Transform")]

        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        /// <summary>
        /// Updates the transform data from the owning component.
        /// </summary>
        public void UpdateTransform(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            scale = transform.lossyScale;
        }
    }
}
