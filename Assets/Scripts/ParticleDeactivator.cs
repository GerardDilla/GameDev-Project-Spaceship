using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeactivator : MonoBehaviour
{

    private void LateUpdate()
    {

        StartCoroutine(deativateDelay(GetComponentInChildren<ParticleSystem>().main.duration));
    }

    // // Update is called once per frame
    // void LateUpdate()
    // {
    //     // gameObject.SetActive(false);
    //     Debug.Log(particleSystem.main.duration);
    //     StartCoroutine(deativateDelay(particleSystem.main.duration));
    //     // if (particleSystem != null && particleSystem.particleCount == 0)
    //     // {
    //     //     gameObject.SetActive(false);
    //     // }
    // }
    private IEnumerator deativateDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        // gameObject.SetActive(false);
    }
}
