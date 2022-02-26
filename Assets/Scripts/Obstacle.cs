using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float damageInflicted;

    public bool baseDamageOnSize;

    public GameManager gameManager;

    private ObjectPooler objectPooler;

    public GameObject destroyEffect;

    public GameObject hitEffect;
    private float collisionDamageTimer = 1.0f;
    private float currentCollisionDamageTimer = 0.0f;

    public bool canBeKnockedBack = false;

    public bool ignoreEnemies = true;

    bool firstHit = false;

    // public float 
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        objectPooler = gameManager.GetComponent<ObjectPooler>();
        if (hitEffect)
        {
            objectPooler.InstantiateObjects(hitEffect, 0);
        }
        if (baseDamageOnSize == true)
        {
            damageInflicted = damageInflicted + (transform.localScale.x / 2);
        }


    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (firstHit == false)
            {
                // Debug.Log(currentCollisionDamageTimer);
                var ship = other.gameObject.GetComponent<ShipControl>();
                if (ship != null)
                {
                    ship.RegisterObstacleHit(gameObject);
                }
                firstHit = true;
            }

        }
        if (other.gameObject.tag == "Enemy")
        {
            // Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }




    }
    private void OnCollisionStay2D(Collision2D other)
    {
        // Debug.Log(currentCollisionDamageTimer);
        if (other.gameObject.tag == "Player")
        {
            var ship = other.gameObject.GetComponent<ShipControl>();
            if (firstHit == true)
            {
                if (ship != null)
                {
                    if (currentCollisionDamageTimer >= collisionDamageTimer)
                    {
                        currentCollisionDamageTimer = 0.0f;
                    }
                    else
                    {
                        if (currentCollisionDamageTimer == 0.0f)
                        {
                            ship.RegisterObstacleHit(gameObject);
                        }
                        currentCollisionDamageTimer = currentCollisionDamageTimer + 0.1f;
                    }
                }

            }
        }
        if (other.gameObject.tag == "Enemy")
        {
            // Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            var ship = other.gameObject.GetComponent<ShipControl>();
            if (ship != null)
            {
                currentCollisionDamageTimer = 0.0f;

            }
        }
    }

    public void DestroyObstacle()
    {

        GameObject explosion = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }



}
