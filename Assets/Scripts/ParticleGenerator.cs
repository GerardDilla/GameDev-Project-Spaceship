using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{

    public GameObject particle;

    public ObjectPooler objectPooler;

    public string parentName;

    public int numberPooled;

    public bool allowOverlapingActive;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler.InstantiateObjects(particle, numberPooled);
    }
    public void PlayParticle(GameObject particleObject, Transform location)
    {
        GameObject particle = objectPooler.GetPooledObject(particleObject);
        particle.transform.position = location.position;
        particle.SetActive(true);
    }
}
