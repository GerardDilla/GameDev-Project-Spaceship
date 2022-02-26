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
    public bool destroyed = false;
    public float fireRate = 2f;

    [SerializeField]
    private float fireRateTimer = 0f;

    // public float destroyedTime = 1.0f;

    // private float destroyedCurrentTime = 0f;
    public Vector3 direction;

    [Header("Object References")]
    public Rigidbody2D spaceShip;
    public Rigidbody2D backThruster;
    public Rigidbody2D leftThruster;
    public Rigidbody2D rightThruster;
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


    // Start is called before the first frame update
    void Start()
    {
        spaceShip = GetComponent<Rigidbody2D>();
        spaceShipTransform = GetComponent<Transform>();
        healthNumber = healthUI.transform.Find("Number");
        backThruster = GameObject.Find("BackThruster").GetComponent<Rigidbody2D>();
        leftThruster = GameObject.Find("LeftThruster").GetComponent<Rigidbody2D>();
        rightThruster = GameObject.Find("RightThruster").GetComponent<Rigidbody2D>();
        currentHealth = health;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        objectPooler = gameManager.GetComponent<ObjectPooler>();
        objectPooler.InstantiateObjects(destroyEffect, 0);
        fireRateTimer = fireRate;
        initialFire = false;
    }

    void Awake()
    {
        shipWeapon.Stop();
        if (hitEffect)
        {
            hitEffect.GetComponent<ParticleSystem>().Stop();
        }

    }

    void update()
    {


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spaceShip.AddForce(transform.up * horizontalPush * 10);
        if (Input.GetKey(KeyCode.W))
        {
            // spaceShip.AddForce(transform.up * horizontalPush * 10);
        }
        if (Input.GetKey(KeyCode.D))
        {
            spaceShip.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * -verticalPush, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            spaceShip.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * verticalPush, Space.World);
        }

    }
    void LateUpdate()
    {

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
            if (fireRateTimer >= fireRate)
            {
                startLFireTimer = false;
                fireRateTimer = 0f;

            }
        }


    }

    void DestroyShip()
    {
        GameObject objectDestroyParticle = objectPooler.GetPooledObject(destroyEffect);
        objectDestroyParticle.transform.position = transform.position;
        objectDestroyParticle.SetActive(true);
        objectDestroyParticle.GetComponentInChildren<ParticleSystem>().Play();
        StartCoroutine(DestroyShipCo());
    }

    private IEnumerator DestroyShipCo()
    {
        yield return new WaitForSeconds(0.1f);
    }
    public void RegisterObstacleHit(GameObject obstacle)
    {

        //Reactive Color Changes
        healthUI.GetComponent<Image>().color = new Color(255, 0, 0, 100);
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 100);
        if (hitEffect != null)
        {
            hitEffect.GetComponent<ParticleSystem>().Play();
        }
        StartCoroutine(RegisterObstacleHitCo(obstacle));

    }

    public IEnumerator RegisterObstacleHitCo(GameObject obstacle)
    {

        yield return new WaitForSeconds(0.1f);

        // Change Current Health
        if (obstacle.GetComponent<Obstacle>() != null)
        {
            currentHealth -= obstacle.GetComponent<Obstacle>().damageInflicted;
        }
        if (obstacle.GetComponent<Enemy>() != null)
        {
            currentHealth -= obstacle.GetComponent<Enemy>().collisionDamage;
        }

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



}
