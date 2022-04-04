using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShipControl : MonoBehaviour
{

    [Header("Ship Attributes")]
    public float verticalPush;
    public float horizontalPush;
    public float health = 100;
    public float currentHealth;

    public enum ShipState { Idle, Spawning, Active, Destroyed };
    public ShipState shipState;
    public float fireRate = 2f;
    private float currentFireRate;
    public bool canBeKnockedBack;

    [SerializeField]
    private float fireRateTimer = 0f;
    public Vector3 direction;

    [Header("Object References")]
    public Rigidbody2D spaceShip;
    public GameObject backThruster;
    private ParticleSystem backThrusterEffect;
    public GameObject leftThruster;
    public GameObject rightThruster;
    public Transform spaceShipTransform;
    public ParticleSystem shipWeapon;
    public GameObject hitEffect;
    public GameObject destroyEffect;
    public GameObject healthUI;
    private Transform healthNumber;
    private GameManager gameManager;
    private ObjectPooler objectPooler;
    private bool startLFireTimer = false;
    private bool initialFire;
    private float buffDuration;
    private float defaultGravity;
    public bool shieldBuffed;
    public bool attackBuffed;
    public GameObject shieldEffect;

    private void Awake()
    {
        spaceShip = GetComponent<Rigidbody2D>();
        spaceShipTransform = GetComponent<Transform>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (backThruster == null) backThruster = GetComponent<GameObject>();
        backThrusterEffect = backThruster.GetComponentInChildren<ParticleSystem>();
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
    }
    void Start()
    {
        // Object References

        // Initial Attributes
        currentHealth = health;
        currentFireRate = fireRate;
        fireRateTimer = fireRate;
        initialFire = false;
        defaultGravity = spaceShip.gravityScale;

        // Instantiate Effects
        objectPooler.InstantiateObjects(destroyEffect, 0);

        // Health UI Reference
        healthNumber = healthUI.transform.Find("Number");
    }

    private void OnEnable()
    {

        shipWeapon.Stop();
        currentHealth = health;
        currentFireRate = fireRate;
        shipState = ShipState.Idle;
        if (hitEffect)
        {
            hitEffect.GetComponent<ParticleSystem>().Stop();
        }
    }
    private void OnDisable()
    {
        attackBuffed = false;
        shieldBuffed = false;
    }
    void Update()
    {
        if (shipState == ShipState.Idle)
        {
            spaceShip.gravityScale = 0f;
        }
        else
        {
            spaceShip.gravityScale = defaultGravity;
        }
        healthNumber.GetComponent<Text>().text = "" + (int)currentHealth;
    }
    void FixedUpdate()
    {
        if (shipState == ShipState.Active)
        {
            ShipControls();
        }
        if (shieldBuffed == true)
        {

            ShieldBuffed();
        }
    }
    void LateUpdate()
    {
        if (shipState == ShipState.Active)
        {
            ShipWeapon();
        }
    }
    private void ShipWeapon()
    {
        if (attackBuffed == true)
        {
            if (startLFireTimer == false)
            {
                if (initialFire == true)
                {
                    shipWeapon.Play();

                }
                initialFire = true;
                startLFireTimer = true;

            }
        }

        if (Input.GetMouseButton(0))
        {

            if (startLFireTimer == false)
            {
                if (initialFire == true)
                {
                    shipWeapon.Play();

                }
                initialFire = true;
                startLFireTimer = true;

            }

        }
        if (startLFireTimer == true)
        {
            fireRateTimer = fireRateTimer + Time.deltaTime;
            if (fireRateTimer >= currentFireRate)
            {
                startLFireTimer = false;
                fireRateTimer = 0f;

            }
        }
    }
    private void ShipControls()
    {
        spaceShip.AddForce(transform.up * verticalPush * 10);

        backThrusterEffect.Emit(1);
        if (Input.GetKey(KeyCode.W))
        {
            // backThrusterEffect.Play();

        }
        if (Input.GetKey(KeyCode.D))
        {
            spaceShip.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * -horizontalPush, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            spaceShip.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * horizontalPush, Space.World);
        }
    }
    public void RegisterDamage(float damage)
    {
        if (shieldBuffed == true) return;
        //Reactive Color Changes
        healthUI.GetComponent<Image>().color = new Color(255, 0, 0, 100);
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 100);
        if (hitEffect != null)
        {
            hitEffect.GetComponent<ParticleSystem>().Play();
        }
        StartCoroutine(RegisterDamageCo(damage));

    }
    public IEnumerator RegisterDamageCo(float damage)
    {
        yield return new WaitForSeconds(0.1f);
        currentHealth -= damage;

        // UI Changes
        healthNumber.GetComponent<Text>().text = "" + (int)currentHealth;

        //Reactive Color Changes
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 100);
        healthUI.GetComponent<Image>().color = new Color(255, 255, 255, 100);

        if (currentHealth <= 0)
        {
            DestroyShip();
            gameManager.GameOver();
            // gameManager.RestartGame();
        }

    }
    public void AttackSpeedBuff(float amount = 0.1f, float duration = 5)
    {
        Debug.Log("buffed");
        currentFireRate = amount;
        buffDuration = buffDuration + duration;
        StartCoroutine(AttackSpeedBuffCo(duration));

    }
    IEnumerator AttackSpeedBuffCo(float duration)
    {
        attackBuffed = true;
        yield return new WaitForSeconds(buffDuration);
        attackBuffed = false;
        buffDuration = 0f;
        currentFireRate = fireRate;
    }
    private void ShieldBuffed()
    {
        Collider2D[] dodgeCollider = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D objectCollided in dodgeCollider)
        {
            if (objectCollided.gameObject.tag == "Enemy")
            {
                // objectCollided.GetComponentInChildren<Enemy>().DestroyEnemy();
            }
            if (objectCollided.gameObject.tag == "Obstacle")
            {
                objectCollided.GetComponent<Obstacle>().Destroy();
            }
        }
    }
    public void ShielfBuff(float speedbuff = 8, float duration = 5)
    {
        if (shieldBuffed != true)
        {
            StartCoroutine(ShielfBuffCo(speedbuff, duration));
        }


    }
    IEnumerator ShielfBuffCo(float speedbuff, float duration)
    {
        var previousSpeed = verticalPush;
        verticalPush = verticalPush + speedbuff;
        shieldBuffed = true;
        GameObject objectDestroyParticle = objectPooler.GetPooledObject(shieldEffect);
        objectDestroyParticle.SetActive(true);
        objectDestroyParticle.transform.position = transform.position;
        yield return new WaitForSeconds(duration);
        objectDestroyParticle.SetActive(false);
        verticalPush = previousSpeed;
        shieldBuffed = false;

    }
    public void Repair(float amount = 50)
    {
        if (currentHealth != health)
        {
            if ((currentHealth + amount) > health)
            {
                currentHealth = health;
            }
            else
            {
                currentHealth += amount;
            }
        }

    }
    void DestroyShip()
    {
        GameObject objectDestroyParticle = objectPooler.GetPooledObject(destroyEffect);
        objectDestroyParticle.transform.position = transform.position;
        objectDestroyParticle.SetActive(true);
        objectDestroyParticle.GetComponentInChildren<ParticleSystem>().Play();
        healthNumber.GetComponent<Text>().text = "0";
        gameManager.SetGameState("GameOver");

    }
    public void SetShipState(string stateString)
    {
        ShipState state = (ShipState)System.Enum.Parse(typeof(ShipState), stateString);
        shipState = state;
    }
    public void ResetState()
    {

    }
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, 1f);
    // }





}
