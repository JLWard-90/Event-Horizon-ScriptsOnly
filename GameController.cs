using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    LevelController levelController;
    GameObject canvasObject;
    [SerializeField]
    GameObject gameOverScreenPrefab;
    [SerializeField]
    GameObject levelCompleteScreenPrefab;
    UIController uIController;
    PlayerController player;
    bool inLevel = false;
    public static GameController instance;
    public List<string> allowedLevels;
    [SerializeField]
    public int highScore;
    bool gameover = false;
    void Awake()
    {
        //check if instance exists
        if (instance == null)
        {
            //assign it to the current object:
            instance = this;
        }
        //make sure instance is the current object
        else if (instance != this)
        {
            //destroy the current game object
            Destroy(gameObject);
        }
        //don't destroy on changing scene
        DontDestroyOnLoad(gameObject);
        allowedLevels.Add("Level 1");
        gameover = false;
    }
    private void Start() 
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if(currentScene.Contains("Level"))
        {
            inLevel = true;
        }
        if(inLevel)
        {
            GetRequiredComponents();
        }
        Debug.Log(allowedLevels);
        AddLevelToList("Level 1");
    }

    private void Update() 
    {
        if (gameover && Input.GetKey("space"))
        {
            RestartLevel();
        }
        if (gameover && Input.GetKey("escape"))
        {
            OnExitButton();
        }
    }

    public void LoadLevel(string levelName)
    {
        gameover = false;
        SceneManager.LoadScene(levelName);
        GetRequiredComponents();
    }

    public void OnGameOver()
    {
        uIController = GameObject.Find("Canvas").GetComponent<UIController>();
        if(uIController != null)
        {
            if(uIController.playerPosition > highScore)
            {
                highScore = (int)uIController.playerPosition;
                Debug.Log("new High Score!");
                Debug.Log(highScore);
            }
            
        }
        player.gameOver = true;
        Debug.Log("Game Over.");
        gameover = true;
        GameObject gameoverscreenobject = GameObject.Instantiate(gameOverScreenPrefab,canvasObject.transform);
        gameoverscreenobject.transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(delegate { OnExitButton(); });
        gameoverscreenobject.transform.Find("RestartButton").GetComponent<Button>().onClick.AddListener(delegate { RestartLevel(); });
    }

    public void OnLevelComplete()
    {
        player.gameOver = true;
        Debug.Log("Congratulations! You win");
        GameObject gameoverscreenobject = GameObject.Instantiate(levelCompleteScreenPrefab,canvasObject.transform);
        gameoverscreenobject.transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(delegate { OnExitButton(); });
        gameoverscreenobject.transform.Find("NextButton").GetComponent<Button>().onClick.AddListener(delegate { NextLevel(); });
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(levelController.SceneName);
    }

    public void GetRequiredComponents()
    {
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        canvasObject = GameObject.Find("Canvas");
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void OnExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel()
    {
        Debug.Log("Moving to next zone");
        SceneManager.LoadScene(levelController.nextScene);
    }

    public void OnEscapeButtonPressed()
    {
        Application.Quit();
    }

    public void AddLevelToList(string levelName)
    {
        if(allowedLevels.Contains(levelName))
        {
            Debug.Log("Level already available.");
        }
        else if (levelName != "InfScroll")
        {
            allowedLevels.Add(levelName);
        }
        
    }
}
