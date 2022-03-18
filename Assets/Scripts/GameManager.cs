using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public enum GameState { Menu, InGame, GameOver, Restart };
    public GameState gameState;
    public GameObject ship;
    private ShipControl shipControl;
    public DifficultyHandler difficultyHandler;
    public CameraController Camera;
    public GameObject mainMenu;
    public GameObject health;
    private GameObject healthNumber;
    public GameObject scoreUI;
    public GameObject highScoreUI;
    public GameObject gameOver;
    public PointTracker pointTracker;
    public float highScore = 0;
    public Vector3 shipStart;
    public GameObject PlanetMenu;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Menu;
        // PlayerPrefs.DeleteAll();
        shipStart = ship.transform.position;
        shipControl = ship.GetComponent<ShipControl>();
        difficultyHandler = GetComponent<DifficultyHandler>();
        gameOver.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetFloat("HighScore");
        }
        gameOver.gameObject.SetActive(false);
        ToggleGameObjects(true);
    }

    private void Update()
    {

        if (gameState == GameState.Menu)
        {
            Home();
        }
        if (gameState == GameState.InGame)
        {
            InGame();
        }
        if (gameState == GameState.GameOver)
        {
            GameOver();
        }
        if (gameState == GameState.Restart)
        {
            RestartGame();
        }
    }
    public void ExitToMenu()
    {
        returnToDefaults();
        gameState = GameState.Menu;
    }
    public void SetGameState(string stateString)
    {
        GameState state = (GameState)System.Enum.Parse(typeof(GameState), stateString);
        gameState = state;
    }

    private void Home()
    {
        mainMenu.SetActive(true);
        health.SetActive(false);
        gameOver.SetActive(false);
        scoreUI.SetActive(false);
        highScoreUI.SetActive(false);
        pointTracker.Tracking = false;
        shipControl.SetShipState("Idle");
        difficultyHandler.active = false;

        var highscoretext = GameObject.Find("HighscoreMain").GetComponent<Text>();
        if (PlayerPrefs.GetFloat("HighScore") != 0)
        {
            GameObject.Find("HighscoreMain").SetActive(true);
            highscoretext.text = "Highscore: " + PlayerPrefs.GetFloat("HighScore");
        }
        PlanetMenu.SetActive(true);

    }

    private void InGame()
    {
        mainMenu.SetActive(false);
        health.SetActive(true);
        gameOver.SetActive(false);
        scoreUI.SetActive(true);
        highScoreUI.SetActive(true);
        pointTracker.Tracking = true;
        shipControl.SetShipState("Active");
        difficultyHandler.active = true;
    }



    public void GameOver()
    {

        ToggleGameObjects(false);
        mainMenu.SetActive(false);
        health.SetActive(false);
        gameOver.SetActive(true);
        scoreUI.SetActive(false);
        highScoreUI.SetActive(false);
        health.transform.Find("Number").GetComponent<Text>().text = "0";
        pointTracker.Tracking = false;
        shipControl.SetShipState("Idle");

    }
    public void RestartGame()
    {

        StartCoroutine("RestartGameCo");

    }
    public IEnumerator RestartGameCo()
    {

        ToggleGameObjects(false);
        yield return new WaitForSeconds(1f);
        returnToDefaults();
        ToggleGameObjects(true);
        gameState = GameState.InGame;

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

        GameObject[] props = GameObject.FindGameObjectsWithTag("prop");
        if (props.Length != 0)
        {
            foreach (GameObject prop in props)
            {
                prop.SetActive(false);
            }
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length != 0)
        {
            foreach (GameObject enemy in enemies)
            {

                enemy.gameObject.SetActive(false);
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
        scoreUI.GetComponent<Text>().text = "Score: " + (int)points;
        highScoreUI.GetComponent<Text>().text = "High Score: " + (int)highScore;
    }

    public void ResetTallyScore()
    {
        pointTracker.points = 0f;
        pointTracker.lastFramePosition = ship.transform.position;
        pointTracker.transform.position = ship.transform.position;
        // pointTracker.distanceToMove = ship.transform.position.y - pointTracker.lastFramePosition.y;
        scoreUI.GetComponent<Text>().text = "Score: " + (int)pointTracker.points;
    }

    public void ResetDifficulty()
    {
        difficultyHandler.progress = 0;
        difficultyHandler.nextThreshold = difficultyHandler.progressThreshold;
        difficultyHandler.difficultyQueue = 0;
        pointTracker.progress = 0;
        foreach (Transform child in difficultyHandler.spawnParent.transform)
        {
            Destroy(child.gameObject);
        }
        // difficultyHandler.StartSpawn();
    }

    private void ToggleGameObjects(bool config = false)
    {
        ship.gameObject.SetActive(config);
        pointTracker.Tracking = config;
        PlanetMenu.SetActive(config);
    }

    private void returnToDefaults()
    {
        InactivateObjectPooled();
        shipTransformReset();
        ResetTallyScore();
        ResetDifficulty();
        Camera.CameraReset();
        difficultyHandler.initialSpawnDone = false;
    }

}
