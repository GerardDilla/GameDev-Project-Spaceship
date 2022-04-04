using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible", menuName = "Collectible")]
public class CollectibleTemplate : ScriptableObject
{
    public string Name;
    public int Amount;
    public float Expiration;
    public Sprite sprite;
    public RuntimeAnimatorController Animator;

}
