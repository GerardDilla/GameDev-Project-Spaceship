using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("- Config")]

    public bool rotateToPlayer = false;

    [Header("- Object References")]
    public Enemy enemy;

    public GameObject parent;

    public Camera MainCamera;

    public ParticleSystem Weapon;

    public CameraBoundaries cameraBoundaries;
    public bool inPosition;

    public enum MovementBehavior { Idle, test1 };

    public MovementBehavior movementBehavior;

    private GameObject enemyArea;
    [SerializeField] private Vector3 randomPosition;

    [SerializeField] private Vector3 currentPosition;
    private Rect screenBounds;
    private float RandomY;
    private float RandomX;

    private float fireRate;

    private float fireTimer;

    private void OnEnable()
    {
        // Get Enemy script attribute
        enemy = GetComponentInChildren<Enemy>();
        enemyArea = GameObject.Find("EnemyArea");
        fireRate = enemy.fireRate;

        // Get position status
        inPosition = enemy.inPosition;
        inPosition = false;

        // Get Camera dimensions and boundaries
        if (MainCamera == null)
        {
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        cameraBoundaries = GetComponent<CameraBoundaries>();

        // Produce random position within enemyarea
        RandomY = Random.Range(enemyArea.transform.localPosition.y, MainCamera.orthographicSize);
        randomPosition = new Vector3(transform.localPosition.x, RandomY, transform.position.z);

        // Get particle system and stop initially
        Weapon = GetComponentInChildren<ParticleSystem>();
        Weapon.Stop();
        fireTimer = fireRate;
    }

    private void LateUpdate()
    {
        currentPosition = transform.localPosition;
        currentPosition.y = Mathf.Clamp(currentPosition.y, enemyArea.transform.localPosition.y, MainCamera.orthographicSize);
        if (inPosition == true)
        {
            cameraBoundaries.excludeY = false;
            FireWeapon();
            // StartCoroutine(FireDelay(Weapon.main.duration));
        }
        if (inPosition == false)
        {
            currentPosition = Vector3.MoveTowards(transform.localPosition, randomPosition, enemy.speed * Time.deltaTime);
            cameraBoundaries.excludeY = true;
            // parent.transform.localPosition = currentPosition;
            if (currentPosition.y <= randomPosition.y)
            {

                inPosition = true;
            }
        }
        transform.localPosition = currentPosition;

    }

    private void FireWeapon()
    {
        if (fireTimer >= fireRate)
        {
            Weapon.Emit(1);
            fireTimer = 0f;
        }
        fireTimer = fireTimer + Time.deltaTime;
    }




}
