using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    string filePath;
    public GameObject obj;
    public List<float[]> listD = new List<float[]>();

    // Start is called before the first frame update
    private void Start()
    {
        filePath = Application.persistentDataPath + "/save.scene";
    }

    public void SaveScene()
    {
        listD = Camera.main.GetComponent<CameraMove>().list;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);

        Save save = new Save();

        save.SaveData(listD);
        bf.Serialize(fs, save);
        fs.Close();
    }

    public void LoadScene()
    {
        if (!File.Exists(filePath))
            return;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open);

        Save save = (Save)bf.Deserialize(fs);
        fs.Close();
        obj.GetComponent<Load>().DelChilds();
        foreach (var item in save.Details)
        {
            obj.GetComponent<Load>().LoadS(item);
        }
    }   
}

[System.Serializable]
public class Save
{
    [System.Serializable]
    public struct Vec3
    {
        public float x, y, z;

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [System.Serializable]
    public struct DetailSaveData
    {
        public Vec3 Position, Rotation, Scale;
        public float ID;

        public DetailSaveData(Vec3 pos, Vec3 rot, Vec3 sca, float id)
        {
            Position = pos;
            Rotation = rot;
            Scale = sca;
            ID = id;
        }
    }

    public List<DetailSaveData> Details =
        new List<DetailSaveData>();

    public void SaveData(List<float[]> data)
    {
        foreach (float[] dataItem in data)
        {
            Vec3 pos = new Vec3(dataItem[0], dataItem[1], dataItem[2]);
            Vec3 rot = new Vec3(dataItem[3], dataItem[4], dataItem[5]);
            Vec3 sca = new Vec3(dataItem[6], dataItem[6], 1);
            float id = dataItem[7];

            Details.Add(new DetailSaveData(pos, rot, sca, id));
        }
    }
}
