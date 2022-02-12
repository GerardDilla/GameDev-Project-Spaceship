using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;
// using Cinemachine;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    // public CinemachineVirtualCamera cam;
    public GameObject explosionPrefab;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        part.Stop();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {

        Debug.Log(other);
        if (other.tag == "obstacle")
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);
            ParticleSystem p = explosion.GetComponent<ParticleSystem>();
            var pmain = p.main;

            // cam.GetComponent<CinemachineImpulseSource>().GenerateImpulse();

            if (other.GetComponent<Rigidbody2D>() != null)
            {
                other.GetComponent<Rigidbody2D>().AddForceAtPosition(collisionEvents[0].intersection * 2 - transform.position, collisionEvents[0].intersection + Vector3.up);
            }
            // other.GetComponent<Obstacle>().DestroyObstacle();
        }

    }


}
