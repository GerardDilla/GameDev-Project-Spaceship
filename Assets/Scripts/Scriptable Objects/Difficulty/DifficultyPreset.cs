using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty", menuName = "Difficulty/Preset")]
public class DifficultyPreset : ScriptableObject
{
    public string Name;
    public int difficultyLevel;
    public List<ObjectSpawnerTemplate> spawners;

}
