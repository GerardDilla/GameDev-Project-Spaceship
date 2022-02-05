using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    public GameObject platform;

    public Transform generationPoint;

    public float distanceBetween;
    private float platformWidth;

    public float platformWidthMin;
    public float platformWidthMax;

    public ObjectPooler objPooler;

    // Start is called before the first frame update
    void Start()
    {
        platformWidth = platform.GetComponent<BoxCollider2D>().size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < generationPoint.position.x)
        {

            distanceBetween = Random.Range(platformWidthMin, platformWidthMax);
            transform.position = new Vector3(transform.position.x + platformWidth + distanceBetween, transform.position.y, 0);
            // Instantiate(platform, transform.position, transform.rotation);
            // GameObject newplatform = objPooler.GetPooledObject();
            // newplatform.transform.position = transform.position;
            // newplatform.transform.rotation = transform.rotation;
            // newplatform.SetActive(true);


        }
    }
}
