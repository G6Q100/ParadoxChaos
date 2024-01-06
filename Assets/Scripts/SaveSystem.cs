using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    [DllImport("__Internal")]
    private static extern void SyncDB();

    public static void SaveGameData(ActionController gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        if (!Directory.Exists(Application.persistentDataPath + "/paradoxChaos"))
            Directory.CreateDirectory(Application.persistentDataPath + "/paradoxChaos");

        string path = Application.persistentDataPath + "/paradoxChaos" + "/data.this";

        FileStream stream = File.Create(path);

        GameData data = new GameData(gameData);
        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log(!File.Exists(path));
        #if UNITY_WEBGL
                SyncDB();
        #endif
    }
    public static GameData LoadGameData()
    {
        string path = Application.persistentDataPath + "/paradoxChaos" + "/data.this";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
