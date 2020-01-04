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
    [SerializeField]
    bool newPlayerTest = false;
    [SerializeField]
    string[] levelNames;
    AdController adController;
    public int coinsToScore = 100;
    void Awake()
    {
        adController = this.gameObject.GetComponent<AdController>();
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
        if(newPlayerTest)
        {
            PlayerPrefs.DeleteAll();
        }
        if (!PlayerPrefs.HasKey("HighScore")) //If a high score does not exist add one
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        if (!PlayerPrefs.HasKey("levelProgress"))
        {
            PlayerPrefs.SetInt("levelProgress", 0);
        }
        if (!PlayerPrefs.HasKey("coins")) //If the player preferences has no key called coins
        {
            //Set the number of coins in the player preference to zero.
            PlayerPrefs.SetInt("coins", 0);
        }
        if (!PlayerPrefs.HasKey("removeAds")) //If the player preferences has no key called removeAds
        {
            PlayerPrefs.SetInt("removeAds", 0); //The remove ads key signifies whether or not ads are enabled. removeAds == 0 -> ads enabled, removeAds == 1 -> ads are turned off.
        }
        //Check that there is a ship available:
        if (!PlayerPrefs.HasKey("EscapePod1"))
        {
            PlayerPrefs.SetInt("EscapePod1", 1);
        }

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
        if (PlayerPrefs.HasKey("levelProgress"))
        {
            Debug.Log(PlayerPrefs.GetInt("levelProgress"));
            int levelProgress = PlayerPrefs.GetInt("levelProgress");
            for (int i=0; i<levelProgress;i++)
            {
                Debug.Log(i);
                AddLevelToList(levelNames[i]);
            }
        }
        uIController.UpdateCoinText();
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
        if (Input.GetKey("escape"))
        {
            OnEscButtonPress();
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
        player.gameOver = true;
        Debug.Log("Game Over.");
        gameover = true;
        GameObject gameoverscreenobject = GameObject.Instantiate(gameOverScreenPrefab,canvasObject.transform);
        gameoverscreenobject.transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(delegate { OnExitButton(); });
        gameoverscreenobject.transform.Find("RestartButton").GetComponent<Button>().onClick.AddListener(delegate { RestartLevel(); });
        if (SceneManager.GetActiveScene().name == "InfScroll")
        {
            AddPlayerCoinsFromScore((int)uIController.playerPosition);
            uIController.UpdateCoinText();
            //Then show high score text
            if (uIController != null && uIController.playerPosition > ReturnCurrentHighScore())
            {
                highScore = (int)uIController.playerPosition;
                UpdateHighScore(highScore);
                Debug.Log("new High Score!");
                Debug.Log(highScore);
                GameObject.Find("HighScoreText").GetComponent<Text>().text = string.Format("New High Score!\n{0}", highScore);
            }
            else if (uIController != null)
            {
                GameObject.Find("HighScoreText").GetComponent<Text>().text = string.Format("High Score: {0}\nYour Score: {1}", ReturnCurrentHighScore(), (int)uIController.playerPosition);
            }
            else
            {
                GameObject.Find("HighScoreText").GetComponent<Text>().text = ""; //If cannot get the UI controller for whatever reason, just set the text to blank
            }
        }
        else
        {
            GameObject.Find("HighScoreText").GetComponent<Text>().text = "";
        }
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
        adController.AdCounter++;
        adController.AdRunner();
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
        adController.AdCounter++;
        adController.AdRunner();
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
            PlayerPrefs.SetInt("levelProgress", PlayerPrefs.GetInt("levelProgress") + 1);
            Debug.Log("New level progress:");
            Debug.Log(PlayerPrefs.GetInt("levelProgress"));
        }
        
    }

    void OnEscButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void UpdateHighScore(int newhighscore)
    {
        PlayerPrefs.SetInt("HighScore", newhighscore);
    }

    int ReturnCurrentHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    public int ReturnPlayerCoins()
    {
        if (PlayerPrefs.HasKey("coins"))
        {
            return PlayerPrefs.GetInt("coins");
        }
        else
        {
            PlayerPrefs.SetInt("coins", 0);
            return 0;
        }
    }

    public void AddPlayerCoinsFromScore(int score)
    {
        int currentCoins = PlayerPrefs.GetInt("coins");
        int coinsToAdd = 0;
        if (coinsToScore != 0)
        {
            coinsToAdd = score / coinsToScore;
        }
        currentCoins += coinsToAdd;
        PlayerPrefs.SetInt("coins", currentCoins);
    }

    public void AddPlayerCoins(int coinsToAdd)
    {
        int currentCoins = PlayerPrefs.GetInt("coins");
        currentCoins += coinsToAdd;
        PlayerPrefs.SetInt("coins", currentCoins);
    }

    public bool SpendPlayerCoins(int coinsToSpend)
    {
        bool canAfford = false;
        int currentCoins = PlayerPrefs.GetInt("coins");
        if (coinsToSpend <= currentCoins) //If the player can afford the purchase then
        {
            canAfford = true;
            currentCoins -= coinsToSpend;
            PlayerPrefs.SetInt("coins", currentCoins); //Save the new number of coins in PlayerPrefs
        }
        else
        {
            Debug.Log("Not enough coins!");
            canAfford = false;
        }
        return canAfford;
    }

    public string ReturnSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
