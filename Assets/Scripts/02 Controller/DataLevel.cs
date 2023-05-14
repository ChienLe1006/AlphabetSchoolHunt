using System;
using UnityEngine;

[Serializable]
public class DataLevel
{
    [SerializeField] private int level;
    [SerializeField] private bool isKeyCollected;
    [SerializeField] private int displayLevel;

    public DataLevel(int level, int displayLevel, bool isKeyCollected = false)
    {
        this.Level = level;
        this.IsKeyCollected = isKeyCollected;
        this.DisplayLevel = displayLevel;
    }

    public int Level { get => level; set => level = value; }
    public bool IsKeyCollected { get => isKeyCollected; set => isKeyCollected = value; }
    public int DisplayLevel { get => displayLevel; set => displayLevel = value; }
}