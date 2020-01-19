using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    string firstLevelName;
    GameController gameController;
    [SerializeField]
    Text coinText;
    private void Awake() 
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    private void Start()
    {
        UpdateCoinText();
    }
    public void loadFirstLevel()
    {
        gameController.LoadLevel(firstLevelName);
    }
    public void loadLevelSelect()
    {
        gameController.LoadLevel("LevSelectMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void OpenShipyardScreen()
    {
        gameController.LoadLevel("ShipyardMenu");
    }

    public void UpdateCoinText()
    {
        int playerCoins = PlayerPrefs.GetInt("coins");
        coinText.text = string.Format("Coins: ${0}", playerCoins);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        gameController.LoadLevel("MainMenu");
    }
}
