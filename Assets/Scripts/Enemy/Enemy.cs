using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Config")]
    public EnemyClass EnemyClass;

    [Header("Enemy State")]
    public EnemyState enemyState;
    public enum EnemyState { Spawning, Idle, Moving, Attacking };


    [Header("References")]
    public GameObject destroyedEffect;
    [HideInInspector] public CameraBoundaries cameraBoundaries;

    [Header("Attributes from Enemy Class")]
    [HideInInspector] public float Health;
    [HideInInspector] public float Damage;
    [HideInInspector] public float FireRate;
    [HideInInspector] public float Speed;
    [HideInInspector] public float BehaviorInterval;
    [HideInInspector] public float pointsUponDeath;
    [HideInInspector] public bool rotateToPlayer;
    [HideInInspector] public float rotationMultiplier;
    [HideInInspector] public Sprite Sprite;
    [HideInInspector] public RuntimeAnimatorController animatorController;
    [HideInInspector] public GameObject enemyProjectile;

    [Header("Object References")]
    [HideInInspector] public ObjectPooler objectPooler;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Camera mainCamera;
    private HitEffect hitEffect;
    private PointTracker pointTracker;
    private LootGenerator lootGenerator;

    private void OnEnable()
    {
        // Assign Values acquired from scriptable object
        Health = EnemyClass.Health;
        Damage = EnemyClass.Damage;
        FireRate = EnemyClass.FireRate;
        Speed = EnemyClass.Speed;
        BehaviorInterval = EnemyClass.BehaviorInterval;
        pointsUponDeath = EnemyClass.pointsUponDeath;
        rotateToPlayer = EnemyClass.rotateToPlayer;
        rotationMultiplier = EnemyClass.rotationMultiplier;
        enemyProjectile = EnemyClass.Projectile;
        Sprite = EnemyClass.Sprite;
        animatorController = EnemyClass.animatorController;

        // Get Sprite Renderer and change sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite;

        // Get Pooler
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();

        // Get Animator
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;

        // Stops Animation
        animator.enabled = false;

        // Gets Camera
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        // Get Camera Bounds
        cameraBoundaries = GetComponent<CameraBoundaries>();

        // Allow object to pass through camera bounds
        if (cameraBoundaries != null) cameraBoundaries.excludeY = true;

        // Get on hit effect
        hitEffect = GetComponent<HitEffect>();

        // Get Point tracker
        pointTracker = GameObject.Find("PointCounter").GetComponent<PointTracker>();

        lootGenerator = GetComponent<LootGenerator>();

        // Set state default
        enemyState = EnemyState.Spawning;

    }

    public void RegisterDamage(float damageInflicted)
    {
        if (enemyState == EnemyState.Spawning) return;
        Health = Health - damageInflicted;
        if (hitEffect != null) hitEffect.RegisterHit();
        if (Health <= 0) Destroy();
    }

    private void Destroy()
    {
        LootHandler();
        DestroyedEffect();
        gameObject.SetActive(false);

        // Credit Points
        if (pointTracker != null) pointTracker.AddPoints(pointsUponDeath);
    }

    private void LootHandler()
    {
        if (lootGenerator == null) return;
        lootGenerator.DropLoot();

    }

    private void DestroyedEffect()
    {
        if (destroyedEffect == null) return;
        var effect = objectPooler.GetPooledObject(destroyedEffect);
        effect.transform.position = transform.position;
        effect.SetActive(true);
        effect.GetComponentInChildren<ParticleSystem>().Play();
    }

}
