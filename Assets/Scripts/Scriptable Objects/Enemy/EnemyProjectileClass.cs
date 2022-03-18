using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Projectile", menuName = "Enemy/Enemy Projectile")]
public class EnemyProjectileClass : ScriptableObject
{
    public float health;
    public float damage;
    public float speed;
    public float pushForce;
    public float lifespan;
    public Sprite Sprite;
    public GameObject destroyEffect;
}
