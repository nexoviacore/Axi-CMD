using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer
{
    public Serializer()
    {
    }

    public void SerializeObject(string filename, TStructData objectToSerialize)
    {
        Stream stream = File.Open(filename, FileMode.Create);
        BinaryFormatter bFormatter = new BinaryFormatter();
        bFormatter.Serialize(stream, objectToSerialize);
        stream.Close();
    }

    public TStructData DeSerializeObject(string filename)
    {
        TStructData objectToSerialize;
        Stream stream = File.Open(filename, FileMode.Open);
        BinaryFormatter bFormatter = new BinaryFormatter();
        objectToSerialize = (TStructData)bFormatter.Deserialize(stream);
        stream.Close();
        return objectToSerialize;
    }
}