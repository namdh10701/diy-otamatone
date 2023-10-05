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

        public GameData GameDatas { get; private set; }

        public GameDataManager()
        {
            OnGoldUpdate = new UnityEvent<int>();
            dataFilePath = Path.Combine(Application.persistentDataPath, "GameDatas.json");

            if (!File.Exists(dataFilePath))
            {
                GameDatas = CreateDefaultDatas();
                SaveDatas();
            }
            GameDatas = LoadDatas();
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
    }
}