using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [Header("- Attributes")]
    public float Health;
    private float currentHealth;
    public float damageInflicted;
    public float pushForce;

    [Header("- Config")]
    public bool canBeKnockedBack = false;
    public bool baseDamageOnSize;
    public List<GameObject> includeDamage = new List<GameObject>();

    [Header("- Object References")]

    private ObjectPooler objectPooler;
    public GameObject destroyEffect;
    public GameObject hitEffect;
    private float collisionDamageTimer = 2.0f;
    private float currentCollisionDamageTimer;
    private LootGenerator lootGenerator;

    private void OnEnable()
    {
        currentHealth = Health;
        currentCollisionDamageTimer = 0f;
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
        lootGenerator = GetComponent<LootGenerator>();
    }
    void Start()
    {
        if (hitEffect)
        {
            objectPooler.InstantiateObjects(hitEffect, 0);
        }
        if (destroyEffect)
        {
            objectPooler.InstantiateObjects(destroyEffect, 0);
        }
        if (baseDamageOnSize == true)
        {
            damageInflicted = damageInflicted + (transform.localScale.x / 2);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (gameObject.name == "Meteor")
        {
            if (other.gameObject.tag == "Enemy")
            {
                var enemy = other.gameObject.GetComponentInChildren<Enemy>();
                ForceObject(other.gameObject);
                enemy.RegisterDamage(damageInflicted);
            }
            if (other.gameObject.tag == "Obstacle")
            {
                var obstacle = other.gameObject.GetComponent<Obstacle>();
                obstacle.Destroy();
            }
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        // Debug.Log(currentCollisionDamageTimer);
        if (other.gameObject.tag == "Player")
        {
            var ship = other.gameObject.GetComponent<ShipControl>();
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
                        ForceObject(other.gameObject);

                        ship.RegisterDamage(damageInflicted);
                    }
                    currentCollisionDamageTimer = currentCollisionDamageTimer + Time.deltaTime;
                }
            }
        }

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            var ship = other.gameObject.GetComponent<ShipControl>();
            if (ship != null)
            {
                currentCollisionDamageTimer = collisionDamageTimer;
            }
        }
    }

    public void RegisterDamage(float damage)
    {

        // Change Current Health
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy();
        }

    }
    public void Destroy()
    {
        GameObject objectDestroyParticle = objectPooler.GetPooledObject(destroyEffect);
        objectDestroyParticle.transform.position = transform.position;
        objectDestroyParticle.SetActive(true);
        objectDestroyParticle.GetComponentInChildren<ParticleSystem>().Play();
        LootHandler();
        gameObject.SetActive(false);
    }

    private void ForceObject(GameObject objectCollided)
    {
        if (objectCollided.GetComponent<Rigidbody2D>() != null)
        {
            var distance = objectCollided.transform.position - transform.position;
            var difference = distance.normalized;
            objectCollided.GetComponent<Rigidbody2D>().velocity = difference * pushForce;

            var fromEnemyDistance = transform.position - objectCollided.transform.position;
            var fromEnemyDifference = -distance.normalized;
            GetComponent<Rigidbody2D>().velocity = fromEnemyDifference * pushForce;
            // objectCollided.GetComponent<Rigidbody2D>().AddForce(difference * pushForce);
        }
    }
    private void LootHandler()
    {
        if (lootGenerator == null) return;
        lootGenerator.DropLoot();

    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, 1f);
    // }



}
