using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{

    public GameObject objectDestroyer;
    // Start is called before the first frame update
    void Start()
    {
        objectDestroyer = GameObject.Find("ObjectDeletePoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < objectDestroyer.transform.position.y)
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
