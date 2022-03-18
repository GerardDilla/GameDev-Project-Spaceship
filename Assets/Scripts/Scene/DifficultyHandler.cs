using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyHandler : MonoBehaviour
{
    [Header("Difficulty Template")]
    public DifficultyDatabase difficultyConfig;
    private List<difficultyTemplate> difficultyDatabase;

    [Header("Config")]
    public bool active;
    public float progressThreshold;
    public GameObject spawnParent;
    public GameObject SpawnerTemplate;
    private List<DifficultyPreset> DifficultyPresets;
    private PointTracker pointTracker;
    private int bossCounter;
    [HideInInspector] public float progress;
    [HideInInspector] public float nextThreshold;
    [HideInInspector] public int difficultyQueue = 0;
    [HideInInspector] public bool initialSpawnDone;

    // Start is called before the first frame update
    void Start()
    {
        // Gets database of difficulty
        difficultyDatabase = difficultyConfig.difficultyDatabase;
        pointTracker = GameObject.Find("PointCounter").GetComponent<PointTracker>();
        nextThreshold = progressThreshold;
        difficultyQueue = 0;
        // StartSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        // GetPreset(2);
        if (active == true)
        {
            if (initialSpawnDone == false)
            {
                Spawn(GetPreset(difficultyQueue));
                Debug.Log(difficultyQueue);
                initialSpawnDone = true;
            }
            ProgressCheck();
        }

    }

    private void ProgressCheck()
    {
        progress = pointTracker.progress;
        if (progress >= nextThreshold)
        {
            // if (bossCounter >= 2)
            // {
            //     foreach (Transform child in spawnParent.transform)
            //     {
            //         child.gameObject.SetActive(false);
            //     }
            //     Spawn(GetPreset(difficultyQueue));
            // }
            if (difficultyQueue < (difficultyDatabase.Count - 1))
            {
                difficultyQueue++;
                Debug.Log(difficultyQueue + ":" + (difficultyDatabase.Count - 1));
                foreach (Transform child in spawnParent.transform)
                {
                    child.gameObject.SetActive(false);
                }
                Spawn(GetPreset(difficultyQueue));

            }
            nextThreshold = nextThreshold + (progressThreshold + (progressThreshold / 2));

        }
    }

    public void Spawn(DifficultyPreset preset = null)
    {
        if (preset == null) return;

        foreach (ObjectSpawnerTemplate spawnerConfig in preset.spawners)
        {
            GameObject spawner = null;
            spawner = (GameObject)Instantiate(SpawnerTemplate);
            spawner.GetComponent<ObjectGenerator>().spawnerConfig = spawnerConfig;
            spawner.transform.parent = spawnParent.transform;
            spawner.transform.position = spawnParent.transform.position;
            spawner.SetActive(true);
        }
    }

    private DifficultyPreset GetPreset(int index = 0)
    {
        DifficultyPreset preset = null;
        var difficulty = difficultyDatabase[index];
        preset = difficulty.difficultyPreset[Random.Range(0, difficulty.difficultyPreset.Length)];
        return preset;
    }

}
