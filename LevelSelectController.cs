using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    List<string> allowedLevels;
    GameController gameController;
    [SerializeField]
    GameObject levelButtonPrefab;
    [SerializeField]
    GameObject viewportContentObject;
    [SerializeField]
    bool testing = false;
    private void Start() 
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        allowedLevels = gameController.allowedLevels;
        Debug.Log(allowedLevels);
        if (testing)
        {
            testList();
        }
        for(int i = 0; i<allowedLevels.Count; i++)
        {
            generateButton(allowedLevels[i]);
        }
    }

    void testList()
    {
        allowedLevels.Add("Level002");
        allowedLevels.Add("Level003");
        allowedLevels.Add("Level004");
        allowedLevels.Add("Level002");
        allowedLevels.Add("Level002");
        allowedLevels.Add("Level002");
        allowedLevels.Add("Level002");
        allowedLevels.Add("Level002");
    }

    void generateButton(string levelName)
    {
        GameObject newLevelButton = GameObject.Instantiate(levelButtonPrefab,viewportContentObject.transform);
        newLevelButton.GetComponent<Button>().onClick.AddListener(delegate { loadLevel(levelName); });
        newLevelButton.transform.Find("Text").GetComponent<Text>().text = levelName;
    }

    public void loadLevel(string levelName)
    {
        gameController.LoadLevel(levelName);
    }

    public void backToMainMenu()
    {
        gameController.LoadLevel("MainMenu");
    }
}
