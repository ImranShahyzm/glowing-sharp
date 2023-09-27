using System.Runtime.Serialization.Formatters.Binary;

namespace SagErpBlazor.DbClasses
{
    public class CustomErrorClass
    {
        public bool IsError { get; set; }
        public string UserMessage { get; set; }
        public string ActualException { get; set; }
        public string InnerException { get; set; }
        public T DeepCopy<T>(T obj)
        {
            if (obj == null)
                return default;

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
