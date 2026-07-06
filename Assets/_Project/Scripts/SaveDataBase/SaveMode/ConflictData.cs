namespace Asteroid.Database
{

    public struct ConflictData
    {
        public DataSave CloudData { get; set; }
        public DataSave LocalData { get; set; }
        public DataSave CloudTime { get; set; }
        public DataSave LocalTime { get; set; }
        public bool IsCloudNewer { get; set; }
        public bool IsLocalNewer { get; set; }

        public bool HasConflict => IsCloudNewer && IsLocalNewer;
    }
}