using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public ShipControl ship;
    public Camera Camera;
    // public BoxCollider2D collider;
    private Vector3 lastFramePosition;
    public float distanceToMove;

    public Vector3 cameraStartPos;


    void Start()
    {
        Camera = GetComponent<Camera>();
        ship = FindObjectOfType<ShipControl>();
        lastFramePosition = ship.transform.position;
        cameraStartPos = transform.position;
    }

    void Update()
    {
        distanceToMove = ship.transform.position.y - lastFramePosition.y;
        if (transform.position.y <= (transform.position.y + distanceToMove))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + distanceToMove, transform.position.z);
            lastFramePosition = ship.transform.position;
        }
    }
    public void CameraReset()
    {
        lastFramePosition = ship.transform.position;
        transform.position = cameraStartPos;

    }


}
