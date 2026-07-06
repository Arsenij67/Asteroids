using System;
using System.Threading.Tasks;

namespace Asteroid.Database
{
    public interface ISaveStrategy
    {
        public Task SaveAsync(DataSave data);
        public Task<DataSave> LoadAsync();
        public DateTime? GetLastSaveTime();
        public bool IsAvailable();
        public string GetName();
    }
}