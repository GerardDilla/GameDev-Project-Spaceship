using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{

    public Transform generationPoint;

    public Camera Camera;

    // public float distanceBetween;
    // private float platformWidth;

    // public float platformWidthMin;
    // public float platformWidthMax;

    public GameObject SpawnObject;

    public int numberPooled = 10;
    public ObjectPooler objPooler;
    public float nextSpawn;

    public float spawnTime = 2f;

    public float spawnChance;

    public bool randomRotation = false;

    public Vector2 randomSize;

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
        float cameraHeight = Camera.orthographicSize;
        float cameraWidth = Camera.aspect * cameraHeight;
        // Debug.Log("halfHeight:" + cameraHeight + " halfWidth:" + cameraWidth);

        //Checks spawn rate
        if (Time.time > nextSpawn)
        {
            //Increments Spawn rate checker
            nextSpawn = Time.time + spawnTime;

            // Randomizes position of spawn within camera dimensions
            var randomX = Random.Range(-cameraWidth, cameraWidth);
            var randomY = Random.Range(0, cameraHeight);

            // Activates an object from a queue
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
            newObject.transform.position = transform.position;
            newObject.SetActive(true);
            // Instantiate(newObject, transform.position, transform.rotation);
            // Debug.Log("spawned " + newObject + " - position:" + newObject.transform.position);
        }

    }
}
