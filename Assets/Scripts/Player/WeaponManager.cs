using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;
// using Cinemachine;

public class WeaponManager : MonoBehaviour
{
    private ParticleSystem part;

    public List<ParticleCollisionEvent> collisionEvents;
    // public CinemachineVirtualCamera cam;
    public GameObject collisionEffectParticle;
    private int numberPooled;
    public ObjectPooler objectPooler;

    public float damage;

    public float pushForce;

    private void Awake()
    {

        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
    }
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        part.Stop();
        collisionEvents = new List<ParticleCollisionEvent>();
        objectPooler.InstantiateObjects(collisionEffectParticle, numberPooled);
    }

    void OnParticleCollision(GameObject other)
    {

        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        GameObject particle = objectPooler.GetPooledObject(collisionEffectParticle);
        particle.transform.position = collisionEvents[0].intersection;
        particle.SetActive(true);
        if (other.tag == "Obstacle")
        {
            ObstacleHandler(other);
        }
        if (other.tag == "Enemy")
        {
            EnemyHandler(other);
        }
        if (other.tag == "Projectile")
        {
            // Debug.Log("hit projectile");
            var projectile = other.GetComponent<EnemyProjectile>();
            projectile.RegisterDamage(damage);
        }

    }

    private void ObstacleHandler(GameObject other)
    {

        if (other.GetComponent<Rigidbody2D>() != null)
        {
            if (other.GetComponent<Obstacle>().canBeKnockedBack == true)
            {
                ForceObject(other);
            }

        }
        other.GetComponentInChildren<Obstacle>().RegisterDamage(damage);
    }


    private void EnemyHandler(GameObject other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            // if (other.GetComponentInChildren<Enemy>().canBeKnockedBack == true)
            // {
            //     // other.GetComponent<Rigidbody2D>().AddForceAtPosition(collisionEvents[0].intersection * 2 - transform.position, collisionEvents[0].intersection + Vector3.up);
            //     ForceObject(other);
            // }

        }
        other.GetComponentInChildren<Enemy>().RegisterDamage(damage);


        // other.GetComponent<Obstacle>().DestroyObstacle();
    }

    private void ForceObject(GameObject objectCollided)
    {
        var distance = objectCollided.transform.position - transform.position;
        var difference = distance.normalized;
        // var pushToPosition = new Vector3();
        // objectCollided.GetComponent<Rigidbody2D>().AddForce(difference * pushForce);
        objectCollided.GetComponent<Rigidbody2D>().velocity = difference * pushForce;

    }


}
