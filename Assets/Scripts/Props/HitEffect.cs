using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public Material OnhitMaterial;
    private Material currentMaterial;

    private void OnEnable()
    {
        currentMaterial = GetComponent<SpriteRenderer>().material;
    }
    private void OnDisable()
    {
        GetComponent<SpriteRenderer>().material = currentMaterial;
    }
    public void RegisterHit()
    {

        GetComponent<SpriteRenderer>().material = OnhitMaterial;
        StartCoroutine(RegisterHitCo());
    }
    private IEnumerator RegisterHitCo()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material = currentMaterial;
    }
}
