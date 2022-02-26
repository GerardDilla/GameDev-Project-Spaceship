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

    public Text gameOver;
    public PointTracker pointTracker;
    public float highScore = 0;
    public Vector3 shipStart;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        shipStart = ship.transform.position;
        shipControl = ship.GetComponent<ShipControl>();
        healthNumber = health.transform.Find("Number");
        gameOver.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetFloat("HighScore");
        }
    }

    public void GameOver()
    {
        ship.gameObject.SetActive(false);
        pointTracker.gameObject.SetActive(false);
        healthNumber.GetComponent<Text>().text = "0";
        gameOver.gameObject.SetActive(true);

    }
    public void RestartGame()
    {

        StartCoroutine("RestartGameCo");

    }
    public IEnumerator RestartGameCo()
    {
        ship.gameObject.SetActive(false);
        healthNumber.GetComponent<Text>().text = "0";
        gameOver.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        InactivateObjectPooled();
        shipTransformReset();
        ResetTallyScore();
        Camera.CameraReset();
        pointTracker.gameObject.SetActive(true);
        shipControl.currentHealth = shipControl.health;
        healthNumber.GetComponent<Text>().text = "" + (int)shipControl.currentHealth;
        objectGenerator.SetActive(true);
    }

    private void InactivateObjectPooled()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        if (obstacles.Length != 0)
        {
            foreach (GameObject obstacle in obstacles)
            {
                obstacle.SetActive(false);
            }

        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length != 0)
        {
            foreach (GameObject enemy in enemies)
            {
                // if(){

                // }
                enemy.transform.parent.gameObject.SetActive(false);
            }

        }
        // if (obstacles.Length != 0)
        // {
        //     GameObject[] prop = GameObject.FindGameObjectsWithTag("prop");
        //     foreach (GameObject obstacle in obstacles)
        //     {
        //         obstacle.SetActive(false);
        //     }
        // }
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
