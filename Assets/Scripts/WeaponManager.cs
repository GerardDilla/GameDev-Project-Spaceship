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
            Debug.Log("ENEMY");
            EnemyHandler(other);
        }

    }

    private void ObstacleHandler(GameObject other)
    {

        if (other.GetComponent<Rigidbody2D>() != null)
        {
            if (other.GetComponent<Obstacle>().canBeKnockedBack == true)
            {
                other.GetComponent<Rigidbody2D>().AddForceAtPosition(collisionEvents[0].intersection * 2 - transform.position, collisionEvents[0].intersection);
            }

        }

        GameObject hitEffect = other.GetComponent<Obstacle>().hitEffect;
        // Debug.Log(hitEffect);
        if (hitEffect != null)
        {
            GameObject objectHitParticle = objectPooler.GetPooledObject(hitEffect);
            objectHitParticle.transform.position = collisionEvents[0].intersection;
            objectHitParticle.SetActive(true);
        }
        // other.GetComponent<Obstacle>().DestroyObstacle();
    }


    private void EnemyHandler(GameObject other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            if (other.GetComponentInChildren<Enemy>().canBeKnockedBack == true)
            {
                other.GetComponent<Rigidbody2D>().AddForceAtPosition(collisionEvents[0].intersection * 2 - transform.position, collisionEvents[0].intersection + Vector3.up);
            }

        }

        GameObject hitEffect = other.GetComponentInChildren<Enemy>().hitEffect;
        // Debug.Log(hitEffect);
        other.GetComponentInChildren<Enemy>().RegisterDamage(damage);


        // other.GetComponent<Obstacle>().DestroyObstacle();
    }


}
