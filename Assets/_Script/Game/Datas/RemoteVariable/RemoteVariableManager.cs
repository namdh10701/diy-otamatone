using UnityEngine;

namespace Game.RemoteVariable
{
    public class RemoteVariableManager
    {
        private static RemoteVariableManager instance;
        public static RemoteVariableManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("Create new Instance");
                    instance = new RemoteVariableManager();

                }
                return instance;
            }
        }

        public RemoteVariableManager()
        {
            MyRemoteVariables = LoadDatas();
        }

        public MyRemoteVariableCollection MyRemoteVariables;

        public void SaveDatas()
        {
            string json = JsonUtility.ToJson(MyRemoteVariables);
            Debug.Log(MyRemoteVariables.FreeInterTime.Value + "SAVED");
            PlayerPrefs.SetString("RemoteVariableCollection", json);
            PlayerPrefs.Save();

        }

        public MyRemoteVariableCollection LoadDatas()
        {
            if (PlayerPrefs.HasKey("RemoteVariableCollection"))
            {
                Debug.Log(PlayerPrefs.HasKey("RemoteVariableCollection") + " 99999");
                string json = PlayerPrefs.GetString("RemoteVariableCollection");

                MyRemoteVariableCollection a = JsonUtility.FromJson<MyRemoteVariableCollection>(json);
                Debug.Log(a.FreeInterTime.Value);
                return a;
            }
            else
            {
                return new MyRemoteVariableCollection();
            }
        }
    }
}