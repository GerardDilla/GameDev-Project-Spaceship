using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float damageInflicted;

    public bool baseDamageOnSize;

    public GameManager gameManager;

    // public float 
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (baseDamageOnSize == true)
        {
            damageInflicted = damageInflicted + (transform.localScale.x / 2);
        }


    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        var objectCollided = other.gameObject.GetComponent<ShipControl>();
        if (objectCollided != null)
        {

            gameManager.RegisterObstacleHit(gameObject, objectCollided);
            // Debug.Log("damaged:" + damageInflicted);
        }
    }


}
