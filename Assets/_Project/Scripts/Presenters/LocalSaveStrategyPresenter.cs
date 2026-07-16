using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;
using Asteroid.Generation;
using System.Text;
using System;
using Newtonsoft.Json;

namespace Asteroid.Database
{
    public class LocalSaveStrategyPresenter : SaveStrategy
    {
        private LocalSaveMetaData _localSaveData;
        private IInstanceLoader _instanceLoader;

        public async UniTask Initialize(DataSave dataSave, LocalSaveMetaData localSaveData, IInstanceLoader instanceLoader, ShopUI shopUI = null)
        {
            base.Initialize(dataSave,_instanceLoader, shopUI);
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
            DataForSave[KeyData.COINS_COUNT] = (int)DataForSave[KeyData.COINS_COUNT] + coinsToAdd;
            UpdateLastSaveTime(KeyData.LAST_SAVE_TIME);
            string jsonData = JsonConvert.SerializeObject(DataForSave);
            return WriteDataFromFileAsync(_localSaveData.FullPath,jsonData);
        }

        public override UniTask AddCountDeadEnemies(int enemiesToAdd)
        {
            DataForSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = (int)DataForSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] + enemiesToAdd;
            UpdateLastSaveTime(KeyData.LAST_SAVE_TIME);
            string jsonData = JsonConvert.SerializeObject(DataForSave);
            return WriteDataFromFileAsync(_localSaveData.FullPath, jsonData);
        }

        public override SaveChoice GetMode()
        {
            return SaveChoice.UseLocal;
        }

        public override UniTask RemoveCountCoins(int coinsToRemove)
        {
            DataForSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = (int)DataForSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] - coinsToRemove;
            UpdateLastSaveTime(KeyData.LAST_SAVE_TIME);
            string jsonData = JsonConvert.SerializeObject(DataForSave);
            return WriteDataFromFileAsync(_localSaveData.FullPath, jsonData);
        }

        protected override void UpdateLastSaveTime(string key)
        {
            DataForSave[key] = DateTime.Now;
        }

        public override UniTask UpdateNoAdsStatus(bool adevertisementIsCanceled)
        {
            DataForSave[KeyData.ADS_DISABLED] = (bool)adevertisementIsCanceled;
            UpdateLastSaveTime(KeyData.LAST_SAVE_TIME);
            string jsonData = JsonConvert.SerializeObject(DataForSave);
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