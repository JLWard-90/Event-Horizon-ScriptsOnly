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
        if(player == null)
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }
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
        if (gameOver == false)
        {
            gameOver = true;
            gameController.OnGameOver();
        }
        
    }

    public void EnteredBlackHole()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameController.OnGameOver();
        }
        
    }
    public void DestroyedByMissile()
    {
        if(!gameOver)
        {
            gameOver = true;
            gameController.OnGameOver();
        }
        
    }
}


