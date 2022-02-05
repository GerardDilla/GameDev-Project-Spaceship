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
    public ObjectPooler objPooler;
    public float nextSpawn;

    public float spawnTime = 2f;

    public float spawnChance;

    // Start is called before the first frame update
    void Start()
    {
        // platformWidth = platform.GetComponent<BoxCollider2D>().size.x;
        Camera = Camera.main;

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
            var randomIndex = Random.Range(0, 2);


            // Debug.Log(randomIndex);
            GameObject newObject = objPooler.GetPooledObject(randomIndex);
            transform.position = new Vector3(randomX, generationPoint.position.y, newObject.transform.position.z);
            if (randomIndex == 0)
            {
                newObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 180));
            }

            newObject.transform.position = transform.position;
            newObject.SetActive(true);
            // Instantiate(newObject, transform.position, transform.rotation);
            // Debug.Log("spawned " + newObject + " - position:" + newObject.transform.position);
        }

    }
}
