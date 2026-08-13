using Asteroid.Generation;
using Asteroid.SpaceShip;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using UnityEngine;

namespace Asteroid.Database
{
    public class LocalSaveStrategy : SaveStrategy
    {
        private LocalSaveMetaData _localSaveData;
        private IInstanceCreator _instanceLoader;

        public async UniTask Initialize(DataSave dataSave, LocalSaveMetaData localSaveData, IInstanceCreator instanceLoader, ShopView shopUI = null, GameOverPresenter shipStatisticsPresenter = null)
        {
            base.Initialize(dataSave,_instanceLoader, shopUI,shipStatisticsPresenter);
            _localSaveData = localSaveData;
            _instanceLoader = instanceLoader;

            if (!File.Exists(_localSaveData.FullPath))
            {
                File.Create(_localSaveData.FullPath);
            }
            string jsonData = await LoadDataFromFileAsync(_localSaveData.FullPath);
            _dataForSave = JsonConvert.DeserializeObject<DataSave>(jsonData) ?? _instanceLoader.CreateInstance<DataSave>();
            await UpdateLastSaveTime();
        }

        public override async UniTask AddCountCoins(int coinsToAdd)
        {
            DataSave[KeyData.COINS_COUNT] = (int)DataSave[KeyData.COINS_COUNT] + coinsToAdd;
            string jsonData = JsonConvert.SerializeObject(DataSave);
            await WriteDataFromFileAsync(_localSaveData.FullPath,jsonData);
            await UpdateLastSaveTime();
        }

        public override async UniTask AddCountDeadEnemies(int enemiesToAdd)
        {
            if(enemiesToAdd<=0) return;

            DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = (int)DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] + enemiesToAdd;
            string jsonData = JsonConvert.SerializeObject(DataSave);
            await WriteDataFromFileAsync(_localSaveData.FullPath, jsonData);
            await UpdateLastSaveTime();
        }

        public override SaveChoice GetMode()
        {
            return SaveChoice.UseLocal;
        }

        public override async UniTask RemoveCountCoins(int coinsToRemove)
        {
            DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] = (int)DataSave[KeyData.DEAD_ENEMIES_COUNT_SUMMARY] - coinsToRemove;
            string jsonData = JsonConvert.SerializeObject(DataSave);
            await WriteDataFromFileAsync(_localSaveData.FullPath, jsonData);
            await UpdateLastSaveTime();
        }

        protected override UniTask UpdateLastSaveTime()
        {
            DataSave[KeyData.LAST_SAVE_TIME] = File.GetLastWriteTime(_localSaveData.FullPath);
            return UniTask.CompletedTask;
        }

        public override async UniTask UpdateNoAdsStatus(bool advertisementIsCanceled)
        {
            DataSave[KeyData.ADS_DISABLED] = (bool)advertisementIsCanceled;
            string jsonData = JsonConvert.SerializeObject(DataSave);
            await WriteDataFromFileAsync(_localSaveData.FullPath, jsonData);
            await UpdateLastSaveTime();
        }

        private async UniTask<string> LoadDataFromFileAsync(string filePath)
        {
            FileInfo file = new FileInfo( filePath);
            if (file.Exists && file.Length>0 )
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