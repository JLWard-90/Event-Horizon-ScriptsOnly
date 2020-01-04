using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipyardMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject[] availableShips;
    [SerializeField]
    GameObject[] unavailableShips;
    [SerializeField]
    string[] shipNames; //Names used to reference player preferences to check whether or not the hips are available.
    GameController gameController;
    [SerializeField]
    Text coinText;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        UpdateCoinText();
    }

    public void MainMenuButtonPress()
    {
        gameController.LoadLevel("MainMenu");
    }

    public void UpdateCoinText()
    {
        int playerCoins = PlayerPrefs.GetInt("coins");
        coinText.text = string.Format("Coins: ${0}", playerCoins);
    }

}
