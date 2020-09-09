using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Zupagood.Helpers
{
    public class SerializationHelper
    {
        public void Serialize<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null)
                return;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, serializableObject);

            stream.Close();
        }

        public T Deserialize<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return default(T);
            }

            T objectOut = default(T);

            FileStream fs = new FileStream(fileName, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                objectOut = (T)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Error:  " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

            return objectOut;
        }
    }
}
