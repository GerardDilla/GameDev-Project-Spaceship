using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleTemplate Config;

    private string Name;
    [HideInInspector] public int Amount;
    private float Expiration;
    private Sprite sprite;
    private RuntimeAnimatorController Animator;

    private void OnEnable()
    {
        Name = Config.name;
        Amount = Config.Amount;
        sprite = Config.sprite;
        Animator = Config.Animator;
        Expiration = Config.Expiration;

        // Sets defaul expiration if none
        if (Expiration == 0) Expiration = 10f;

        // Starts Expiration Timer
        StartCoroutine(expirationTimer());

        // Changes properties based on config
        gameObject.name = Name;
        SetSprite();
        SetAnimatorController();
    }
    private void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
    private void SetAnimatorController()
    {
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = Animator;
    }
    private IEnumerator expirationTimer()
    {
        yield return new WaitForSeconds(Expiration);
        gameObject.SetActive(false);
    }
}
