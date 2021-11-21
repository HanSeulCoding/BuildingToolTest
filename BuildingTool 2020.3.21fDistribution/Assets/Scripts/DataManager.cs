using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;


public class DataManager
{
    // Start is called before the first frame update
    //public static void BinarySerialize<T>(T t, string filePath)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    FileStream stream = new FileStream(filePath, FileMode.Create);
    //    formatter.Serialize(stream, t);
    //    stream.Close();
    //}

    //public static T BinaryDeserialize<T>(string filePath)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    FileStream stream = new FileStream(filePath, FileMode.Open);
    //    T t = (T)formatter.Deserialize(stream);
    //    stream.Close();
        
    //    return t;
    //}
    public static void Save()
    {

    }
    // Update is called once per frame
}
