using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public ShipControl ship;
    public Camera Camera;
    // public BoxCollider2D collider;
    private Vector3 lastFramePosition;

    public float sizeX;
    public float sizeY;
    public float ratio;
    public float distanceToMove;


    void Start()
    {
        Camera = GetComponent<Camera>();
        // collider = GetComponent<BoxCollider2D>();
        ship = FindObjectOfType<ShipControl>();
        lastFramePosition = ship.transform.position;
        // CameraCollider();
    }

    void Update()
    {
        // distanceToMove = player.moveSpeed;
        // transform.position = new Vector3(0, player.position.y, -10);
        distanceToMove = ship.transform.position.y - lastFramePosition.y;
        transform.position = new Vector3(transform.position.x, transform.position.y + distanceToMove, transform.position.z);
        lastFramePosition = ship.transform.position;
        // transform.position = new Vector3(transform.position.x, transform.position.y + distanceToMove, transform.position.z);
        // lastFramePosition = ship.transform.position;
        // rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    // void CameraCollider(){

    //     sizeY = Camera.orthographicSize * 2;
    //     ratio = (float)Screen.width/(float)Screen.height;
    //     sizeX = sizeY * ratio;
    //     collider.size = new Vector2(sizeX,sizeX);

    // }
}
