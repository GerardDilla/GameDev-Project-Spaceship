using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [Header("Config")]

    public EnemyProjectileClass EnemyProjectileClass;
    private float health;
    private float damage;
    private float speed;
    private float pushForce;
    private float lifespan;
    private Sprite Sprite;
    private GameObject destoyedEffect;
    private Rigidbody2D projectileRB;
    private SpriteRenderer spriteRenderer;
    private ObjectPooler objectPooler;
    private void OnEnable()
    {
        if (EnemyProjectileClass == null)
            Debug.Log("No config assigned to " + gameObject.name);

        // Assign config data to variable
        health = EnemyProjectileClass.health;
        damage = EnemyProjectileClass.damage;
        speed = EnemyProjectileClass.speed;
        pushForce = EnemyProjectileClass.pushForce;
        lifespan = EnemyProjectileClass.lifespan;
        Sprite = EnemyProjectileClass.Sprite;
        destoyedEffect = EnemyProjectileClass.destroyEffect;

        // Assigns default lifespan if non is assigned
        if (lifespan == 0) lifespan = 15f;

        // Get Rigidbody
        projectileRB = GetComponent<Rigidbody2D>();

        // Get and Change sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite;

        // Get object Pooler
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();

        // Start Lifespan Countdown
        StartCoroutine(ProjectileLifespan());
    }
    private void FixedUpdate()
    {
        transform.position += -transform.up * speed * Time.deltaTime;
    }
    public void RegisterDamage(float damage = 0f)
    {
        health = health - damage;
        if (health <= 0)
        {
            DestroyProjectile();
        }
    }
    private IEnumerator ProjectileLifespan()
    {
        yield return new WaitForSeconds(lifespan);
        DestroyProjectile();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<ShipControl>().RegisterDamage(damage);
            PushOnHit(other.gameObject);
            DestroyProjectile();
        }
        if (other.gameObject.tag == "Obstacle" && other.gameObject.name != "Meteor")
        {
            PushOnHit(other.gameObject);
            DestroyProjectile();
        }
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Buff")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }
    }
    private void PushOnHit(GameObject objectCollided)
    {
        var objectRB = objectCollided.GetComponent<Rigidbody2D>();
        var difference = objectCollided.transform.position - transform.position;
        objectRB.velocity = difference * pushForce;
    }
    private void playEffect()
    {
        var effect = objectPooler.GetPooledObject(destoyedEffect);
        effect.transform.position = transform.position;
        effect.SetActive(true);

    }
    private void DestroyProjectile()
    {
        playEffect();
        gameObject.SetActive(false);
    }
}


