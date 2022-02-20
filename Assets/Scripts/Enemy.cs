using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damage;
    public float collisionDamage;

    public float Health;

    [SerializeField]
    private float currentHealth;
    public GameManager gameManager;

    private ObjectPooler objectPooler;

    public GameObject destroyEffect;

    public GameObject hitEffect;

    public Collider2D enemyCollider;
    private float collisionDamageTimer = 1.0f;
    private float currentCollisionDamageTimer = 0.0f;

    public bool canBeKnockedBack = false;

    bool firstHit = false;

    // public float 
    // Start is called before the first frame update
    void Start()
    {


        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        objectPooler = gameManager.GetComponent<ObjectPooler>();
        objectPooler.InstantiateObjects(destroyEffect, 0);

    }

    private void Awake()
    {
        currentHealth = Health;
        Debug.Log(currentHealth);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 100);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Physics2D.IgnoreCollision(enemyCollider, other.gameObject.GetComponent<Collider2D>());
        }
        if (other.gameObject.tag == "Player")
        {
            var ship = other.gameObject.GetComponent<ShipControl>();
            if (firstHit == false)
            {
                // Debug.Log(currentCollisionDamageTimer);
                var objectCollided = other.gameObject.GetComponent<ShipControl>();
                if (objectCollided != null)
                {
                    ship.RegisterObstacleHit(gameObject);
                }
                firstHit = true;
            }
        }



    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Physics2D.IgnoreCollision(enemyCollider, other.gameObject.GetComponent<Collider2D>());

        }
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



    }
    private void OnCollisionExit2D(Collision2D other)
    {
        var ship = other.gameObject.GetComponent<ShipControl>();
        if (ship != null)
        {
            currentCollisionDamageTimer = 0.0f;

        }
    }
    public void RegisterDamage(float damage)
    {

        //Reactive Color Changes
        Debug.Log(damage);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 100);
        if (hitEffect != null)
        {
            hitEffect.GetComponent<ParticleSystem>().Play();
        }
        // Change Current Health
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            DestroyEnemy();
        }
        else
        {
            //Reactive Color Changes
            StartCoroutine(HitColorDelay());
        }


    }

    private IEnumerator HitColorDelay()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 100);


    }

    public void DestroyEnemy()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 100);
        canBeKnockedBack = true;
        StartCoroutine(DestroyEnemyCo());
    }

    private IEnumerator DestroyEnemyCo()
    {
        // gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.position, transform.position + Vector3.up * 100);
        GameObject objectDestroyParticle = objectPooler.GetPooledObject(destroyEffect);
        objectDestroyParticle.transform.position = transform.position;
        objectDestroyParticle.SetActive(true);
        objectDestroyParticle.GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 100);
        canBeKnockedBack = false;
        currentHealth = Health;
        gameObject.SetActive(false);
    }



}
