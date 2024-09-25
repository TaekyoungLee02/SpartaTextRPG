using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using TextRPG.Character;

namespace TextRPG.GameManager
{
    public class DataManager
    {
        private static DataManager _instance;
        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataManager();
                }
                return _instance;
            }
            private set { }
        }
        private DataManager() { }

        GameData data;
        readonly string path = @"./Save\\";

        public void InitSave(Player player, bool[] shopData)
        {
            data = new GameData
            {
                _player = player,
                _shopData = shopData
            };
        }

        public void Save()
        {
            DirectoryInfo info = new DirectoryInfo(path);
            if(info.Exists == false) info.Create();

            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            TextWriter writer = new StreamWriter(path + "savedata");

            serializer.Serialize(writer, data);
            writer.Close();
        }

        public bool Load(out Player player, out bool[] shopData)
        {
            FileInfo info = new FileInfo(path + "savedata");
            if(info.Exists == false)
            {
                player = null;
                shopData = null;
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GameData));
                TextReader reader = new StreamReader(path + "savedata");

                data = serializer.Deserialize(reader) as GameData;
                reader.Close();
            }
            catch
            {
                player = null;
                shopData = null;
                return false;
            }

            if(data == null)
            {
                player = null;
                shopData = null;
                return false;
            }
            else
            {
                player = data._player;
                shopData = data._shopData;
                return true;
            }
        }

        public bool DeleteSaveData()
        {
            FileInfo info = new FileInfo(path + "savedata");
            if(info.Exists)
            {
                info.Delete();
                return true;
            }
            return false;
        }

        [Serializable]
        public class GameData
        {
            public Player _player;
            public bool[] _shopData;
        }
    }
}
