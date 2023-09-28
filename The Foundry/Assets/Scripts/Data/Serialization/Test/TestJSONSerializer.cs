using System.IO;
using UnityEngine;

namespace GameFramework
{
    public class TestJSONSerializer : MonoBehaviour
    {
        public enum TestMode
        {
            Serialization = 0,
            Deserialization = 1
        }

        [System.Serializable]
        public class ExampleData
        {
            public string Name;
            public string Faction;
            public string Occupation;
        }

        [SerializeField]
        private TestMode testMode;

        [SerializeField]
        private ExampleData exampleData;

        JSONSerializer jsonSerializer = null;

        private void Awake()
        {
            jsonSerializer = new JSONSerializer();
        }

        private void Start()
        {
            if (testMode == TestMode.Serialization)
                TestSerialization();
            else
                TestDeserialization();
        }

        private void TestSerialization()
        {
            object serializedData = jsonSerializer.Serialize(exampleData);

            // If the directory to the file hasn't existed, create one.
            FileInfo fileInfo = new FileInfo($"{Application.persistentDataPath}/Examples/ExampleSaving.txt");
            fileInfo.Directory.Create();

            StreamWriter streamWriter = new StreamWriter(fileInfo.FullName);
            streamWriter.Write(serializedData);
            streamWriter.Close();

            Debug.Log($"Successfully wrote data to {fileInfo.FullName}");
        }

        private void TestDeserialization()
        {
            // If the directory to the file hasn't existed, create one.
            FileInfo fileInfo = new FileInfo($"{Application.persistentDataPath}/Examples/ExampleSaving.txt");
            fileInfo.Directory.Create();

            StreamReader streamReader = new StreamReader(fileInfo.FullName);
            object rawData = streamReader.ReadToEnd();

            jsonSerializer.Deserialize(rawData, ref exampleData);
        }
    }
}
