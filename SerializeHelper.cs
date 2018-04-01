using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MyNamespace.Utils
{
    public class SerializeHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns>The message queue.</returns>
        /// <param name="msgQueue">Message queue.</param>
        public static byte[] SerializeObj<T>(T obj)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, obj);
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();

            return buffer;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <returns>The message queue.</returns>
        /// <param name="bytes">Bytes.</param>
        public static T DesrializeObj<T>(byte[] bytes)
        {
            T obj;

            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(bytes);
            obj = (T)formatter.Deserialize(stream);
            stream.Flush();
            stream.Close();

            return obj;
        }
    }
}