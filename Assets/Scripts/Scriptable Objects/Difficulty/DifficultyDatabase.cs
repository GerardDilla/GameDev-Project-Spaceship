using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty Database", menuName = "Difficulty/Database")]
public class DifficultyDatabase : ScriptableObject
{
    [Header("Randomizes Between Presets")]
    public List<difficultyTemplate> difficultyDatabase;
}

[System.Serializable]
public class difficultyTemplate
{

    [Header("List of Spawner presets")]
    public DifficultyPreset[] difficultyPreset;

    [Header("List of Boss presets")]
    public DifficultyPreset[] bossPreset;
    [Space(10)] private int space;


}

