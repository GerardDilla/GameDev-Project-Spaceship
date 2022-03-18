using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Enemy Instance")]
public class EnemyClass : ScriptableObject
{
    public string Name;
    public float Health;
    public float Damage;
    public float FireRate;
    public float Speed;
    public float BehaviorInterval;
    public float pointsUponDeath;
    public bool rotateToPlayer;
    public float rotationMultiplier;
    public RuntimeAnimatorController animatorController;
    public Sprite Sprite;
    // public float pushForce;
    public GameObject Projectile;
    public List<GameManager> possibleDrops;


}
