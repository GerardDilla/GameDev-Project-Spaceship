using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float damageInflicted;

    public bool baseDamageOnSize;

    public GameManager gameManager;

    public GameObject destroyEffect;
    private float collisionDamageTimer = 1.0f;
    private float currentCollisionDamageTimer = 0.0f;

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
        // Debug.Log(currentCollisionDamageTimer);
        var objectCollided = other.gameObject.GetComponent<ShipControl>();
        if (objectCollided != null)
        {
            gameManager.RegisterObstacleHit(gameObject, objectCollided);
        }


    }
    private void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log(currentCollisionDamageTimer);
        var objectCollided = other.gameObject.GetComponent<ShipControl>();

        if (objectCollided != null)
        {
            if (currentCollisionDamageTimer >= collisionDamageTimer)
            {
                currentCollisionDamageTimer = 0.0f;
            }
            else
            {
                if (currentCollisionDamageTimer == 0.0f)
                {
                    gameManager.RegisterObstacleHit(gameObject, objectCollided);
                }
                currentCollisionDamageTimer = currentCollisionDamageTimer + 0.1f;
            }
        }


    }
    private void OnCollisionExit2D(Collision2D other)
    {
        var objectCollided = other.gameObject.GetComponent<ShipControl>();
        if (objectCollided != null)
        {
            currentCollisionDamageTimer = 0.0f;

        }
    }

    public void DestroyObstacle()
    {

        GameObject explosion = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }



}
