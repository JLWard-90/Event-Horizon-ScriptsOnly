using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Start is called before the first frame update
    public float startingFuel = 100;
    GameController gameController;
    public string SceneName = "Level001";
    public string nextScene = "Level002";
    PlayerController player;
    public bool gameOver;
    private void Awake() 
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Start() 
    {
        player.gameOver = false;
        gameController.GetRequiredComponents();
        gameController.AddLevelToList(SceneName);
        gameOver = false;
    }

    public void OnGoalEnter()
    {
        gameController.OnLevelComplete();
    }

    public void playAreaLeft()
    {
        gameController.OnGameOver();
    }

    public void EnteredBlackHole()
    {
        gameOver = true;
        gameController.OnGameOver();
    }
    public void DestroyedByMissile()
    {
        gameOver = true;
        gameController.OnGameOver();
    }
}


