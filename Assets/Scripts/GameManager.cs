using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject ship;
    private ShipControl shipControl;
    public GameObject objectGenerator;
    public CameraController Camera;
    public GameObject health;
    private Transform healthNumber;
    public Text scoreUI;
    public Text highScoreUI;
    public PointTracker pointTracker;
    public float highScore = 0;
    public Vector3 shipStart;





    // Start is called before the first frame update
    void Start()
    {
        shipStart = ship.transform.position;
        shipControl = ship.GetComponent<ShipControl>();
        healthNumber = health.transform.Find("Number");
        if (PlayerPrefs.HasKey("HighScore"))
        {
            Debug.Log("has key");
            highScore = PlayerPrefs.GetFloat("HighScore");
        }
        else
        {
            Debug.Log("has no key");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shipControl.currentHealth <= 0)
        {
            RestartGame();
        }

    }

    public void RegisterObstacleHit(GameObject obstacle, ShipControl shipControl)
    {

        StartCoroutine(RegisterObstacleHitCo(obstacle, shipControl));

    }
    public void RestartGame()
    {

        StartCoroutine("RestartGameCo");

    }

    public IEnumerator RegisterObstacleHitCo(GameObject obstacle, ShipControl shipControl)
    {


        //Reactive Color Changes
        health.GetComponent<Image>().color = new Color(255, 0, 0, 100);
        ship.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 100);

        yield return new WaitForSeconds(0.1f);

        // Change Current Health
        shipControl.currentHealth -= obstacle.GetComponent<Obstacle>().damageInflicted;
        // UI Changes
        healthNumber.GetComponent<Text>().text = "" + (int)shipControl.currentHealth;

        //Reactive Color Changes
        ship.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 100);
        health.GetComponent<Image>().color = new Color(255, 255, 255, 100);
        // obstacle.SetActive(false);
    }
    public IEnumerator RestartGameCo()
    {
        ship.gameObject.SetActive(false);
        shipControl.currentHealth = shipControl.health;
        healthNumber.GetComponent<Text>().text = "0";
        yield return new WaitForSeconds(0.5f);
        healthNumber.GetComponent<Text>().text = "" + (int)shipControl.currentHealth;
        InactivateObjectPooled();
        shipTransformReset();
        ResetTallyScore();
        Camera.CameraReset();
        objectGenerator.SetActive(true);
    }

    private void InactivateObjectPooled()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        if (obstacles.Length != 0)
        {
            foreach (GameObject obstacle in obstacles)
            {
                obstacle.SetActive(false);
            }

        }
        if (obstacles.Length != 0)
        {
            GameObject[] prop = GameObject.FindGameObjectsWithTag("prop");
            foreach (GameObject obstacle in obstacles)
            {
                obstacle.SetActive(false);
            }
        }
    }

    private void shipTransformReset()
    {
        ship.transform.position = shipStart;
        ship.transform.rotation = Quaternion.Euler(0, 0, 0);
        ship.gameObject.SetActive(true);
    }

    public void TallyScore(float points)
    {

        if (points > highScore)
        {
            highScore = points;
            PlayerPrefs.SetFloat("HighScore", Mathf.Round(highScore));
        }
        scoreUI.text = "Score: " + (int)points;
        highScoreUI.text = "High Score: " + (int)highScore;
    }

    public void ResetTallyScore()
    {
        pointTracker.points = 0f;
        pointTracker.lastFramePosition = ship.transform.position;
        pointTracker.transform.position = ship.transform.position;
        // pointTracker.distanceToMove = ship.transform.position.y - pointTracker.lastFramePosition.y;
        scoreUI.text = "Score: " + (int)pointTracker.points;
    }
}
