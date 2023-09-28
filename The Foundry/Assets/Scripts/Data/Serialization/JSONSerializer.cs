using Newtonsoft.Json;

namespace GameFramework
{
    public class JSONSerializer : Serializer
    {
        public override object Serialize(object data)
        {
            string jsonString = JsonConvert.SerializeObject(data);
            return jsonString;
        }

        public override bool Deserialize<T>(object rawData, ref T data)
        {
            string jsonString = rawData.ToString();
            
            data = JsonConvert.DeserializeObject<T>(jsonString);
            return data != null;
        }
    }
}
