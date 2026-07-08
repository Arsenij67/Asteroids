using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalSaveData", menuName = "ScriptableObjects/LocalSaveData")]
public class LocalSaveData : ScriptableObject
{
    public const string FileName = "PlayerProgress.json";

    public string FullPath => Path.Combine(Application.persistentDataPath,FileName);
}
