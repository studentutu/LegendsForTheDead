using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
namespace Game
{
    public class BinarySerializer<T> : IDataSerializer<T>
    {
        public string extension { get { return ".dataPer"; } }

        public string SavePath(string name)
        {
            string path = Path.Combine(Application.persistentDataPath, name + extension);
            Debug.Log(string.Format("<Color=Blue> Saving to {0} </Color>", path));
            return path;
        }

        public bool isSaveExist(string name) { return File.Exists(SavePath(name)); }

        public void Save(T data, string name)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(SavePath(name));
            bf.Serialize(file, data);
            file.Close();
        }

        public T Load(string name)
        {
            T data = default(T);
            if (isSaveExist(name))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(SavePath(name), FileMode.Open);
                try
                {
                    data = (T)bf.Deserialize(file);
                }
                finally
                {
                    file.Close();
                }
            }
            return data;
        }

        public void Delete(string name)
        {
            if (isSaveExist(name))
            {
                File.Delete(SavePath(name));
            }
        }

        public void DeleteAllData()
        {
            string path = Application.persistentDataPath;
            Directory.Delete(path, recursive: true);
        }
    }
}