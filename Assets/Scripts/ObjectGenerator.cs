using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{

    [Header("Object / Components")]
    public GameObject SpawnObject;

    public List<Sprite> SpriteVariants;

    public Transform generationPoint;

    public Camera Camera;

    public ObjectPooler objPooler;


    [Header("Pooling Config")]
    public float spawnTime = 2f;
    public float spawnChance = 1;
    public int numberPooled = 10;

    public int maxActiveObjects;
    // public string customParentName;
    public bool randomRotation = false;
    public Vector2 randomSize;

    private float nextSpawn;
    private float cameraHeight;

    private float cameraWidth;

    // Start is called before the first frame update
    void Start()
    {
        // platformWidth = platform.GetComponent<BoxCollider2D>().size.x;
        Camera = Camera.main;
        objPooler.InstantiateObjects(SpawnObject, numberPooled);


    }

    // Update is called once per frame
    void LateUpdate()
    {


        //Gets camera dimensions
        cameraHeight = Camera.orthographicSize;
        cameraWidth = Camera.aspect * cameraHeight;

        //Checks spawn rate
        if (Time.time > nextSpawn)
        {
            float randomizer = Random.Range(0.0f, 1.0f);
            if (Random.value <= spawnChance)
            {
                if (SpawnObject.name == "Meteor")
                {
                    Debug.Log("spawned:" + "-" + SpawnObject.name + ":" + randomizer + ":" + spawnChance);
                }

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
        var randomY = Random.Range(0, cameraHeight);
        // Activates an object from a queue
        if (maxActiveObjects != 0)
        {

            int activeChild = CountActive();
            if (activeChild < maxActiveObjects)
            {
                GameObject newObject = objPooler.GetPooledObject(SpawnObject);
                transform.position = new Vector3(randomX, generationPoint.position.y, newObject.transform.position.z);
                if (randomRotation == true)
                {
                    newObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 180));
                }

                if (randomSize != Vector2.zero)
                {
                    newObject.transform.localScale = Vector2.one * Random.Range(randomSize.x, randomSize.y);

                }

                if (SpriteVariants.Count != 0)
                {
                    newObject.GetComponent<SpriteRenderer>().sprite = SpriteVariants[Random.Range(0, SpriteVariants.Count)];
                }
                newObject.transform.position = transform.position;
                newObject.SetActive(true);

            }

        }
        else
        {
            GameObject newObject = objPooler.GetPooledObject(SpawnObject);
            transform.position = new Vector3(randomX, generationPoint.position.y, newObject.transform.position.z);
            if (randomRotation == true)
            {
                newObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 180));
            }

            if (randomSize != Vector2.zero)
            {
                newObject.transform.localScale = Vector2.one * Random.Range(randomSize.x, randomSize.y);

            }

            if (SpriteVariants.Count != 0)
            {
                newObject.GetComponent<SpriteRenderer>().sprite = SpriteVariants[Random.Range(0, SpriteVariants.Count)];
            }
            newObject.transform.position = transform.position;
            newObject.SetActive(true);
        }


    }

    private int CountActive()
    {
        GameObject parent = GameObject.Find(SpawnObject.name + "_Parent");
        int childActiveCount = 0;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (child.gameObject.activeInHierarchy)
            {
                childActiveCount++;
            }
        }
        return childActiveCount;
    }

}
