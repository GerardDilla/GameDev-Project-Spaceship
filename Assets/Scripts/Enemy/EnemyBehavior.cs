using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Enemy Config")]
    private Enemy Enemy;
    public Vector3 projectileOffset;

    [Header("Camera Dimensions")]
    private Rect screenBounds;

    [Header("Random Position Variables")]
    private Vector3 randomPosition = Vector3.zero;
    private bool randomPositionGeneratorLock = false;

    [Header("Movement / Attack Interval")]
    private float movementTimer = 0f;
    private float attackTimer = 0f;
    private Enemy.EnemyState preAttackState;

    [Header("Component References")]
    private Rigidbody2D enemyRB;

    private void Start()
    {
        // Gets Enemy attributes
        Enemy = GetComponent<Enemy>();

        // Gets RigidBody
        enemyRB = GetComponent<Rigidbody2D>();

        // Calculates Camera dimensions
        CameraDimensions();
    }
    private void LateUpdate()
    {

        if (Enemy.enemyState == Enemy.EnemyState.Spawning)
        {
            Spawning();
        }
        if (Enemy.enemyState == Enemy.EnemyState.Idle)
        {
            Idle();
        }
        if (Enemy.enemyState == Enemy.EnemyState.Moving)
        {
            Moving();
        }
        if (Enemy.enemyState == Enemy.EnemyState.Attacking)
        {
            Attacking();
        }

        // Starts Attack Timer
        AttackTimer();

        RotationManager();

        // Check if going out of bounds
        if (transform.localPosition.y <= 0)
        {
            Enemy.enemyState = Enemy.EnemyState.Moving;
        }
    }
    private void Spawning()
    {
        // Generates Random Position within bounds
        GetRandomPosition();

        // Moves Enemy into random Position
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, randomPosition, Enemy.Speed * Time.deltaTime);

        // Resets State
        if (Vector3.Distance(transform.localPosition, randomPosition) <= 1)
        {
            Enemy.enemyState = Enemy.EnemyState.Idle;
            randomPositionGeneratorLock = false;
            randomPosition = Vector3.zero;
        }
    }
    private void Idle()
    {
        if (movementTimer >= Enemy.BehaviorInterval)
        {
            Enemy.enemyState = Enemy.EnemyState.Moving;
            movementTimer = 0f;
        }
        Enemy.animator.enabled = true;
        movementTimer += Time.deltaTime;
        Enemy.cameraBoundaries.excludeY = false;
    }
    private void Moving()
    {
        // Generates Random Position within bounds
        GetRandomPosition();

        // Prevents going out of bounds
        Enemy.cameraBoundaries.excludeY = false;

        // Moves Enemy into random Position
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, randomPosition, Enemy.Speed * Time.deltaTime);

        // Resets State
        if (Vector3.Distance(transform.localPosition, randomPosition) <= 1)
        {
            Enemy.enemyState = Enemy.EnemyState.Idle;
            randomPositionGeneratorLock = false;
            randomPosition = Vector3.zero;
        }

    }
    private void Attacking()
    {
        // Instantiate Projectile
        LaunchProjectile();

        // Recoil Effect
        RecoilEffect();

        // Go back to state before attacking
        Enemy.enemyState = preAttackState;

        // Reset Attack timer
        attackTimer = 0f;

    }
    private void AttackTimer()
    {
        if (Enemy.enemyState == Enemy.EnemyState.Spawning) return;
        if (attackTimer >= Enemy.FireRate)
        {
            preAttackState = Enemy.enemyState;
            Enemy.enemyState = Enemy.EnemyState.Attacking;
        }
        attackTimer += Time.deltaTime;

    }
    private void LaunchProjectile()
    {
        // Projectile References
        var Projectile = Enemy.objectPooler.GetPooledObject(Enemy.enemyProjectile);

        // Projectile Movement
        Projectile.transform.position = transform.position + projectileOffset;
        Projectile.transform.rotation = transform.rotation;
        Projectile.SetActive(true);
    }
    private void RecoilEffect()
    {
        enemyRB.velocity = transform.up * 10f;
        StartCoroutine(RecoilDelay());
    }
    private IEnumerator RecoilDelay()
    {
        yield return new WaitForSeconds(0.1f);
        enemyRB.velocity = Vector3.zero;
    }
    private void RotationManager()
    {
        if (Enemy.rotateToPlayer != true) return;
        // Debug.Log(gameObject.name + ": facing");
        GameObject[] locatedPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in locatedPlayer)
        {
            // Debug.Log(player.name + ":" + player.transform.position);
            if (player.gameObject.activeSelf == true)
            {
                // Rotate to Object
                Vector3 vectorToTarget = player.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - Enemy.rotationMultiplier;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Enemy.Speed);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }
        if (other.gameObject.tag == "Enemy")
        {
            // Enemy.enemyState = Enemy.EnemyState.Moving;
        }
    }
    private void GetRandomPosition()
    {
        if (randomPositionGeneratorLock == true) return;

        float RandomX = Random.Range(-screenBounds.x, screenBounds.x);
        float RandomY = Random.Range(0, Enemy.mainCamera.orthographicSize);
        randomPosition = new Vector3(RandomX, RandomY, transform.position.z);
        // Debug.Log(randomPosition);
        randomPositionGeneratorLock = true;

    }
    private void CameraDimensions()
    {
        var mainCamera = Enemy.mainCamera;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        Vector3 cameraCenter = mainCamera.transform.position;
        Vector2 cameraSize = new Vector2(cameraWidth, cameraHeight);
        Vector2 cameraCenterPosition = mainCamera.transform.position;
        Vector2 cameraBottomLeftPosition = cameraCenterPosition - (cameraSize / 2);
        screenBounds = new Rect(cameraBottomLeftPosition, cameraSize);
        // Debug.Log("cameraHeight: " + cameraHeight);
        // Debug.Log("cameraWidth: " + cameraWidth);
        // Debug.Log("cameraCenter: " + cameraCenter);
    }

}
