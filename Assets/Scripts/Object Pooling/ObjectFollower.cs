using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{

    public Transform objectToFollow;
    public string objectToFollowString;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (objectToFollowString != "")
        {
            objectToFollow = GameObject.Find(objectToFollowString).transform;
            transform.position = objectToFollow.transform.position;
        }
        else
        {
            transform.position = objectToFollow.transform.position;
        }

    }
}
