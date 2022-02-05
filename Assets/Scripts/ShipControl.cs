using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{

    public Vector3 direction;
    public float verticalPush;

    public Rigidbody2D leftThruster;

    public Rigidbody2D rightThruster;
    public float horizontalPush;
    public Rigidbody2D spaceShip;

    public Transform spaceShipTransform;
    // Start is called before the first frame update
    void Start()
    {
        spaceShip = GetComponent<Rigidbody2D>();
        spaceShipTransform = GetComponent<Transform>();
        leftThruster = GameObject.Find("LeftThruster").GetComponent<Rigidbody2D>();
        rightThruster = GameObject.Find("RightThruster").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // spaceShip.AddForce(transform.up * horizontalPush * 10);
        if (Input.GetKey(KeyCode.W))
        {
            // spaceShip.velocity = new Vector3(0, horizontalPush, 0);
            spaceShip.AddForce(transform.up * horizontalPush * 10);
            // spaceShip.AddRelativeForce(Vector3.forward * horizontalPush);
        }
        if (Input.GetKey(KeyCode.D))
        {
            spaceShip.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * -verticalPush, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            spaceShip.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * verticalPush, Space.World);
        }
    }
}
