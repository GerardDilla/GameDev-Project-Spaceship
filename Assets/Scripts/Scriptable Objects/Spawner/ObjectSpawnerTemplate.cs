using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawner", menuName = "Spawner")]
public class ObjectSpawnerTemplate : ScriptableObject
{
    public string Name;
    public string customParentString;
    public float spawnTime;
    public float spawnChance;
    public int spawnBatch;
    public float spawnGap;
    public int numberPooled;
    public int maxActiveObjects;
    public bool randomRotation;
    public Vector2 randomSize;
    public GameObject SpawnObject;
    public List<GameObject> SpawnVariants;


}
