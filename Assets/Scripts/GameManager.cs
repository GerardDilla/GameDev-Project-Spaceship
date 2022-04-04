using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("- Config")]
    public GameState gameState;
    public enum GameState { Menu, InGame, GameOver, Restart };
    public List<string> resetableTags;
    private float highScore = 0;
    private Vector3 shipStart;

    [Header("- Object References")]
    public GameObject ship;
    private ShipControl shipControl;
    public GameObject mainMenu;
    public GameObject health;
    private GameObject healthNumber;
    public GameObject scoreUI;
    public GameObject highScoreUI;
    public GameObject gameOver;
    public GameObject CoinsUI;
    private DifficultyHandler difficultyHandler;
    public CameraController Camera;
    public PointTracker pointTracker;
    public GameObject PlanetMenu;

    // Start is called before the first frame update
    void Start()
    {

        // Sets Default State of Game
        gameState = GameState.Menu;

        // Gets Default position of Ship
        shipStart = ship.transform.position;

        // Get Object References
        shipControl = ship.GetComponent<ShipControl>();
        difficultyHandler = GetComponent<DifficultyHandler>();

        // Hides GameOver Ui
        gameOver.gameObject.SetActive(false);

        // Get Current Score and displays to Ui
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetFloat("HighScore");
        }
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
        CoinsUI.GetComponent<Text>().text = "Coins: " + (int)PlayerPrefs.GetInt("Coin");
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
        CoinsUI.SetActive(false);
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
        CoinsUI.SetActive(true);
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
        CoinsUI.SetActive(false);
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
        foreach (string tags in resetableTags)
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag(tags);
            if (obj.Length != 0)
            {
                foreach (GameObject obstacle in obj)
                {
                    obstacle.SetActive(false);
                }
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
