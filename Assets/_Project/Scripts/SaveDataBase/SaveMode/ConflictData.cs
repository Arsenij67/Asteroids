using System;

namespace Asteroid.Database
{
    public struct ConflictData
    {   
        public DataSave CloudData { get; set; }
        public DataSave LocalData { get; set; }
        public DateTime CloudTime { get; set; }
        public DateTime LocalTime { get; set; }
        public bool IsCloudNewer { get; set; }
        public bool IsLocalNewer { get; set; }

        public bool HasConflict => IsCloudNewer && IsLocalNewer;
    }
}