using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public Enemy enemy;

    public GameObject parent;

    public Camera MainCamera;
    public bool inPosition;

    public enum MovementBehavior { Idle, test1 };

    public MovementBehavior movementBehavior;

    private GameObject enemyArea;

    public float idleMovementInterval = 10f;
    public float idleMovementCount = 0f;
    public float idleMovementDistance = 1f;
    public Vector3 randomPosition;
    private Rect screenBounds;

    [SerializeField]
    private float RandomY;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyArea = enemy.enemyArea;
        parent = gameObject.transform.parent.gameObject;
        if (MainCamera == null)
        {
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        float cameraHeight = MainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * MainCamera.aspect;
        Debug.Log(MainCamera.orthographicSize);
        Vector2 cameraSize = new Vector2(cameraWidth, cameraHeight);
        Vector2 cameraCenterPosition = MainCamera.transform.position;
        Vector2 cameraBottomLeftPosition = cameraCenterPosition - (cameraSize / 2);
        screenBounds = new Rect(cameraBottomLeftPosition, cameraSize);
        RandomY = Random.Range(MainCamera.orthographicSize, enemyArea.transform.localPosition.y);
    }

    void Awake()
    {
        idleMovementCount = 0f;
        // if (MainCamera == null)
        // {
        //     MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        // }
        // RandomY = Random.Range(MainCamera.orthographicSize, enemyArea.transform.localPosition.y);
        // randomPosition = 
    }

    // void OnEnable()
    // {
    //     if (MainCamera == null)
    //     {
    //         MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    //     }
    //     RandomY = Random.Range(MainCamera.orthographicSize, enemyArea.transform.localPosition.y);
    // }
    // void OnDisable()
    // {
    //     // if (MainCamera == null)
    //     // {
    //     //     MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    //     // }
    //     RandomY = Random.Range(MainCamera.orthographicSize, enemyArea.transform.localPosition.y);
    // }

    // Update is called once per frame
    void LateUpdate()
    {
        inPosition = enemy.inPosition;
        if (inPosition == true)
        {
        }

        float step = enemy.speed * Time.deltaTime;
        if (inPosition == false)
        {
            Vector3 randomSpawnLocation = new Vector3(parent.transform.position.x, RandomY, enemyArea.transform.position.z);
            Debug.Log(MainCamera.orthographicSize + ":" + enemyArea.transform.localPosition.y + "=" + RandomY);
            parent.transform.localPosition = Vector3.MoveTowards(parent.transform.localPosition, randomSpawnLocation, step);
            if (parent.transform.position.y == randomSpawnLocation.y)
            {
                enemy.inPosition = true;
            }
        }
    }

    private void idleMovement()
    {

    }


}
