//using System;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Asteroid.Database
//{
//    public class CloudSaveStrategy : ISaveStrategy
//    {
//        private readonly IRemoteSavable _remoteSavable;
//        private readonly string _saveKey = "game_data";
//        private DateTime? _lastSaveTime;

//        public CloudSaveStrategy(IRemoteSavable remoteSavable)
//        {
//            _remoteSavable = remoteSavable;
//        }

//        public async Task SaveAsync(DataSave data)
//        {
//            try
//            {
//                data[CloudKeyData.LAST_SAVE_TIME] = DateTime.UtcNow;
//                await _remoteSavable.SaveKey(_saveKey, data);
//                _lastSaveTime = DateTime.UtcNow;
//                Debug.Log($"Cloud Save: Данные сохранены в облаке в {_lastSaveTime}");
//            }
//            catch (Exception ex)
//            {
//                Debug.LogError($"Cloud Save: Ошибка сохранения - {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<GameData> LoadAsync()
//        {
//            try
//            {
//                var data = await _remoteSavable.GetKey<GameData>(_saveKey);
//                if (data != null)
//                {
//                    _lastSaveTime = data.LastSaveTime;
//                    Debug.Log($"Cloud Save: Данные загружены из облака. Время: {_lastSaveTime}");
//                    return data;
//                }

//                Debug.Log("Cloud Save: Данных в облаке нет, создаем новые");
//                return new GameData();
//            }
//            catch (Exception ex)
//            {
//                Debug.LogError($"Cloud Save: Ошибка загрузки - {ex.Message}");
//                throw;
//            }
//        }

//        public DateTime? GetLastSaveTime()
//        {
//            return _lastSaveTime;
//        }

//        public bool IsAvailable()
//        {
//            // Проверка наличия интернета
//            return Application.internetReachability != NetworkReachability.NotReachable;
//        }

//        public string GetName()
//        {
//            return "Облачное сохранение";
//        }
//    }

//}