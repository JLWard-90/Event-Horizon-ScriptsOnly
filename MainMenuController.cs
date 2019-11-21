using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    string firstLevelName;
    GameController gameController;
    private void Awake() 
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    public void loadFirstLevel()
    {
        gameController.LoadLevel(firstLevelName);
    }
    public void loadLevelSelect()
    {
        gameController.LoadLevel("LevSelectMenu");
    }
}
