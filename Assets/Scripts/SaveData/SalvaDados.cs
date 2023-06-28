using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SalvaDados : MonoBehaviour
{
    public string Path;
    private Vector3 temp;
  
    // Start is called before the first frame update
    void Start()
    {
        Path = Application.persistentDataPath + "/posData.data";
        temp = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Salvar();
        }
         if (Input.GetKeyDown(KeyCode.A))
        {
            LoadPos();
        }
    }
    void Salvar()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Path);

        SaveClass s = new SaveClass();
        s.posx = transform.position.x;
        temp.y = transform.position.y;
        bf.Serialize(fs, s);
        fs.Close();
    }
    void LoadPos()
    {
        if(File.Exists(Path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Path, FileMode.Open);

            SaveClass s = (SaveClass) bf.Deserialize(fs);
            fs.Close();
            temp.x = s.posx;
            
            
            transform.position = temp;

        } else
        {

        }
    }
}
[Serializable]
class SaveClass
{
    public float posx; 
}