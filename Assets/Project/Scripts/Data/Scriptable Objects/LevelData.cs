using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 9)]
public class LevelData : ScriptableObject
{
    public List<LevelInfo> levelInfo;
}

[System.Serializable]
public class LevelInfo
{
    public int levelNumber;
    public string levelName;
    public GameObject levelPrefab;
}