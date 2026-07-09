using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;
using Asteroid.Generation;
using UnityEditor.Overlays;
using System.Text;
using System;
using Newtonsoft.Json;

namespace Asteroid.Database
{
    public class LocalSaveStrategyPresenter : SaveStrategy
    {
        private LocalSaveData _localSaveData;
        private IInstanceLoader _instanceLoader;

        public async UniTask Initialize(DataSave dataSave, LocalSaveData localSaveData, IInstanceLoader instanceLoader, ShopUI shopUI = null)
        {
            base.Initialize(dataSave, shopUI);
            _localSaveData = localSaveData;
            _instanceLoader = instanceLoader;

            if (!File.Exists(_localSaveData.FullPath))
            {
                File.Create(_localSaveData.FullPath);
            }
            string jsonData = await LoadDataFromFileAsync(_localSaveData.FullPath);
            DataSave = JsonConvert.DeserializeObject<DataSave>(jsonData) ?? _instanceLoader.CreateInstance<DataSave>();
        }

        public override UniTask AddCountCoins(int coinsToAdd)
        {
            DataSave[KeyData.COINS_COUNT] = (int)DataSave[KeyData.COINS_COUNT] + coinsToAdd;
            UpdateLastSaveTime(KeyData.LAST_SAVE_TIME);
            string jsonData = JsonConvert.SerializeObject(DataSave);
            return WriteDataFromFileAsync(_localSaveData.FullPath,jsonData);
        }

        public override UniTask AddCountDeadEnemies(int enemiesToAdd)
        {
            DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = (int)DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] + enemiesToAdd;
            UpdateLastSaveTime(KeyData.LAST_SAVE_TIME);
            string jsonData = JsonConvert.SerializeObject(DataSave);
            return WriteDataFromFileAsync(_localSaveData.FullPath, jsonData);
        }

        public override SaveChoice GetMode()
        {
            return SaveChoice.UseLocal;
        }

        public override UniTask RemoveCountCoins(int coinsToRemove)
        {
            DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = (int)DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] - coinsToRemove;
            UpdateLastSaveTime(KeyData.LAST_SAVE_TIME);
            string jsonData = JsonConvert.SerializeObject(DataSave);
            return WriteDataFromFileAsync(_localSaveData.FullPath, jsonData);
        }

        protected override void UpdateLastSaveTime(string key)
        {
            DataSave[key] = DateTime.Now;
        }

        public override UniTask UpdateNoAdsStatus(bool adevertisementIsCanceled)
        {
            DataSave[KeyData.ADS_DISABLED] = (bool)adevertisementIsCanceled;
            UpdateLastSaveTime(KeyData.LAST_SAVE_TIME);
            string jsonData = JsonConvert.SerializeObject(DataSave);
            return WriteDataFromFileAsync(_localSaveData.FullPath, jsonData);
        }

        private async UniTask<string> LoadDataFromFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = string.Empty;
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    jsonData = await streamReader.ReadToEndAsync().AsUniTask();
                }
                return jsonData;
            }

            else
            {
                Debug.Log(string.Format($"Файла по пути {filePath} не существует. Чтение невозможно"));
                return string.Empty;
            }
        }

        private UniTask WriteDataFromFileAsync(string filePath, string jsonData)
        {
            if (!File.Exists(filePath))
            {
                Debug.Log(string.Format($"Файла по пути {filePath} не существует. Создается новый и записывается в него инфа"));
            }

            using (StreamWriter streamWriter = new StreamWriter(filePath,false,encoding:Encoding.UTF8))
            {
                return streamWriter.WriteAsync(jsonData).AsUniTask();
            }

        }
    }
}