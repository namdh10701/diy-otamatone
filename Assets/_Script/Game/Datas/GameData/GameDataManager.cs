using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Datas
{
    public class GameDataManager
    {
        public UnityEvent<int> OnGoldUpdate;
        public UnityEvent<int> OnNoteUpdate;
        private static GameDataManager instance;
        public static GameDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameDataManager();
                }
                return instance;
            }
        }

        private string dataFilePath;
        private string dataFilePath2;

        public GameData GameDatas { get; private set; }
        public GameData2 GameDatas2 { get; private set; }

        public GameDataManager()
        {
            OnGoldUpdate = new UnityEvent<int>();
            OnNoteUpdate = new UnityEvent<int>();
            dataFilePath = Path.Combine(Application.persistentDataPath, "GameDatas.json");
            dataFilePath2 = Path.Combine(Application.persistentDataPath, "GameData2.json");

            if (!File.Exists(dataFilePath))
            {
                GameDatas = CreateDefaultDatas();
                SaveDatas();
            }
            if (!File.Exists(dataFilePath2))
            {
                GameDatas2 = CreateDefaultDatas2();
                SaveDatas2();
            }
            GameDatas = LoadDatas();
            GameDatas2 = LoadDatas2();
        }
        public GameData2 LoadDatas2()
        {
            string json = File.ReadAllText(dataFilePath2);
            return JsonUtility.FromJson<GameData2>(json);
        }


        private GameData2 CreateDefaultDatas2()
        {
            TextAsset defaultJsonAsset = Resources.Load<TextAsset>("DefaultGameDatas2");
            return JsonUtility.FromJson<GameData2>(defaultJsonAsset.text);
        }

        public void SaveDatas2()
        {
            string json = JsonUtility.ToJson(GameDatas2);
            File.WriteAllText(dataFilePath2, json);
        }
        public void SaveDatas()
        {
            string json = JsonUtility.ToJson(GameDatas);
            File.WriteAllText(dataFilePath, json);
        }

        public GameData LoadDatas()
        {
            string json = File.ReadAllText(dataFilePath);
            return JsonUtility.FromJson<GameData>(json);
        }

        private GameData CreateDefaultDatas()
        {
            TextAsset defaultJsonAsset = Resources.Load<TextAsset>("DefaultGameDatas");
            return JsonUtility.FromJson<GameData>(defaultJsonAsset.text);
        }

        public void UpdateGold(int amount)
        {
            GameDatas.Coin += amount;
            OnGoldUpdate.Invoke(amount);
        }

        public void UpdateNote(int amount)
        {
            //GameDatas2.Notes += amount;
            //OnNoteUpdate.Invoke(amount);
        }
    }
}