using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shipyardButtonController : MonoBehaviour
{
    public int shipIndex;
    public bool shipUnlocked;
    public int unlockCost = 200; //The number of coins required to unlock this ship
    public string shipName; //This is used to reference the correct variable in PlayerPrefs
    Image shipImage;
    Button shipButton;
    private void Start()
    {
        //Here we do all of our communication with player preferences to check whether the ship has been unlocked or not.
        if (PlayerPrefs.HasKey("shipName"))
        {
            int shipUnlockedInt = PlayerPrefs.GetInt(shipName);
            if (shipUnlockedInt == 1)
            {
                shipUnlocked = true;
            }
            else
            {
                shipUnlocked = false;
            }
        }
        else
        {
            if (shipUnlocked == true)
            {
                PlayerPrefs.SetInt(shipName, 1);//1 means ship is unlocked
            }
            else
            {
                PlayerPrefs.SetInt(shipName, 0);//0 means ship is not unlocked.
            }
        }
        //Now we use that information to check set the colour of the ships image based on whether or not it is unlocked
        shipImage = transform.Find("ShipImage").GetComponent<Image>();
        SetShipColour();
        //Finally we need to set the text of the button to be either select or unlock based on whether the ship is unlocked.
        shipButton = transform.Find("ShipButton").GetComponent<Button>();
        Debug.Log(shipButton);
        SetButtonText();
    }

    public void OnButtonPress()
    {
        Debug.Log("button pressed");
        if (shipUnlocked)
        {
            PlayerPrefs.SetInt("currentShipIndex", shipIndex);
            SetButtonText();
        }
        else
        {
            GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
            bool canAfford = gameController.SpendPlayerCoins(unlockCost);
            if (canAfford)
            {
                shipUnlocked = true;
                PlayerPrefs.SetInt(shipName, 1); //Save that the ship shipname is now available. [0] is unavailable, [1] is available
                SetShipColour();
                SetButtonText();
            }
            else
            {
                Debug.Log("Cannot afford to unlock this ship!");
            }
        }
    }
    private void SetShipColour()
    {
        if (shipUnlocked)
        {
            shipImage.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            shipImage.color = new Color32(50, 50, 50, 255);
        }
    }

    private void SetButtonText()
    {
        Text buttonText = shipButton.transform.Find("Text").GetComponent<Text>();
        Debug.Log(buttonText);
        if (shipUnlocked)
        {
            if (shipIndex != PlayerPrefs.GetInt("currentShipIndex"))
            {
                buttonText.text = "Select";
            }
            else
            {
                buttonText.text = "Current ship";
            }
        }
        else
        {
            buttonText.text = string.Format("Unlock (${0})",unlockCost);
        }
    }
}
