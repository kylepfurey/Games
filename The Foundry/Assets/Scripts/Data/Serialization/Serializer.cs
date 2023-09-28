namespace GameFramework
{
    /// <summary>
    /// Based class for serializers that convert objects into data that can be stored in files.
    /// </summary>
    public abstract class Serializer
    {
        public abstract object Serialize(object data);
        public abstract bool Deserialize<T>(object rawData, ref T data);
    }
}
