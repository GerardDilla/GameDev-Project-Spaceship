using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [Header("Spawner Config")]
    public ObjectSpawnerTemplate spawnerConfig;

    [Header("Config")]
    private GameObject SpawnObject;
    private string customParentString;
    private float spawnTime;
    private float spawnChance;
    private float spawnGap;
    private int numberPooled;
    private int maxActiveObjects;
    private bool randomRotation = false;
    private Vector2 randomSize;
    private List<GameObject> SpawnVariants;
    private List<Sprite> SpriteVariants;
    private Camera Camera;
    private float cameraHeight;
    private float cameraWidth;
    private Transform generationPoint;
    private ObjectPooler objPooler;
    private GameObject customParent;
    private float nextSpawn;
    private void OnEnable()
    {
        // Set Config Data
        SpawnObject = spawnerConfig.SpawnObject;
        customParentString = spawnerConfig.customParentString;
        spawnTime = spawnerConfig.spawnTime;
        spawnChance = spawnerConfig.spawnChance;
        spawnGap = spawnerConfig.spawnGap;
        numberPooled = spawnerConfig.numberPooled;
        maxActiveObjects = spawnerConfig.maxActiveObjects;
        randomRotation = spawnerConfig.randomRotation;
        randomSize = spawnerConfig.randomSize;
        SpawnVariants = spawnerConfig.SpawnVariants;

        // Get Camera
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Camera = Camera.main;

        // Get Object Pooler
        objPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();

        // Sets custom parent and instantiates
        SetCustomParent();

        // Instantiate objects with specified number
        InstantiateObjectsAdvance();

        // Sets Generation Point
        generationPoint = gameObject.transform;

        // Change name of object according to config
        gameObject.name = spawnerConfig.Name + "_Spawner";

    }
    void Update()
    {
        //Gets camera dimensions
        cameraHeight = Camera.orthographicSize;
        cameraWidth = Camera.aspect * cameraHeight;

        //Checks spawn rate
        if (Time.time > nextSpawn)
        {
            if (Random.value <= spawnChance)
            {
                SpawnPoolObject();
            }
            else
            {
                nextSpawn = Time.time + spawnTime;
            }
        }
    }
    private void SpawnPoolObject()
    {
        //Increments Spawn rate checker
        nextSpawn = Time.time + spawnTime;

        // Randomizes position of spawn within camera dimensions
        var randomX = Random.Range(-cameraWidth, cameraWidth);
        generationPoint.position = new Vector3(randomX, generationPoint.position.y, 50);

        // Check if there are variants and set which will spawn
        if (SpawnVariants.Count != 0)
        {
            var RandomIndex = Random.Range(0, SpawnVariants.Count);
            SpawnObject = SpawnVariants[RandomIndex];
        }

        // Activates an object from a queue
        if (maxActiveObjects != 0)
        {
            // Prevents spawning when pooled reaches the maximum number of objects 
            int activeChild = CountActive();
            if (activeChild < maxActiveObjects)
            {
                Spawn();
            }
        }
        else
        {
            Spawn();
        }

        // Reset random range
        randomX = 0;

    }
    private void Spawn()
    {
        GameObject newObject = objPooler.GetPooledObject(SpawnObject, customParent, spawnerConfig.Name);
        newObject.transform.position = generationPoint.position;
        newObject.name = SpawnObject.name;

        if (randomRotation == true)
        {
            if (newObject.GetComponent<SpriteRenderer>() != null)
            {
                newObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 180));
            }
            else
            {
                newObject.transform.Find("SpriteMask").rotation = Quaternion.Euler(0, 0, Random.Range(0, 180));
            }

        }

        if (randomSize != Vector2.zero)
        {
            newObject.transform.localScale = Vector2.one * Random.Range(randomSize.x, randomSize.y);

        }

        if (newObject.transform.position != generationPoint.position)
        {
            Debug.Log("Not Match" + newObject.transform.position + " - " + generationPoint.position);
        }

        newObject.SetActive(true);
    }
    private void SetCustomParent()
    {
        if (customParentString != "")
        {
            customParent = GameObject.Find(customParentString);
            if (customParent == null)
            {
                customParent = new GameObject(customParentString);
            }
        }
    }
    private void InstantiateObjectsAdvance()
    {
        // if (numberPooled == 0) return;
        if (SpawnVariants.Count != 0)
        {
            foreach (GameObject variant in SpawnVariants)
            {
                variant.name = SpawnVariants[0].name;
                objPooler.InstantiateObjects(variant, numberPooled, customParent);
            }
        }
        else
        {
            objPooler.InstantiateObjects(SpawnObject, numberPooled, customParent);
        }
    }
    private int CountActive()
    {

        GameObject parent = null;
        if (customParent != null)
        {
            parent = customParent;
        }
        else
        {
            parent = GameObject.Find(SpawnObject.name + "_Parent");
        }

        int childActiveCount = 0;
        for (int i = 0; i < parent.transform.childCount; i++)
        {

            Transform child = parent.transform.GetChild(i);
            if (parent.name == "EnemyFrame")
            {
                Debug.Log(child.gameObject.name + ":" + SpawnObject.name);
            }
            if (child.gameObject.name == SpawnObject.name && child.gameObject.activeInHierarchy)
            {
                childActiveCount++;
            }
        }
        // Debug.Log(SpawnObject.name + ":" + parent.name + ":" + childActiveCount);
        return childActiveCount;
    }

}
