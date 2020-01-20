using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Text fuelText;
    [SerializeField]
    Text coinsText;
    float initPlayerPosition;
    public float playerPosition = 0;
    float playerSpeed;
    Transform playerTransform;
    PlayerController player;
    [SerializeField]
    int fuelPerGaugeMarker; //This is the amount of fuel that corresponds to each section of the fuel gauge
    [SerializeField]
    GameObject[] fuelGauge; //This array contains each of the objects that make up the fuel gauge in order that they are filled up.
    [SerializeField]
    int speedPerGaugeMarker = 1;
    [SerializeField]
    GameObject[] speedGauge;
    int oldSpeedInt;
    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        initPlayerPosition = playerTransform.position.x;
        playerPosition = initPlayerPosition;
        playerSpeed = 0;
        player = playerTransform.gameObject.GetComponent<PlayerController>();
        UpdateCoinText();
        UpdateSpeedGauge(playerSpeed);
    }
    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            OnUpdateFuelText(player.fuel);
        }

    }
    public void OnUpdateFuelText(float fuel)
    {
        int fuelint = (int)fuel;
        playerSpeed = (playerTransform.position.x - initPlayerPosition - playerPosition) / Time.deltaTime;
        playerPosition = (playerTransform.position.x - initPlayerPosition);
        //New string for fuelText now that the speedGauge is also added:
        string fuelstring = string.Format("Distance: {0}", (int)playerPosition);
        //New string for fuelText now that the fuel gauge is being used instead:
        //string fuelstring = string.Format("Distance: {0}       Speed: {1}", (int)playerPosition, (int)playerSpeed);
        //Below is the old string
        //string fuelstring = string.Format("Fuel: {0}       Distance: {1}       Speed: {2}",fuelint,(int)playerPosition,(int)playerSpeed);
        fuelText.text = fuelstring;
        //Now decide if the fuel gauge needs to be updated
        if (fuelint % fuelPerGaugeMarker == 0) //If the modulo of fuelint is zero then we need to update the fuel gauge
        {
            //Debug.Log("updating fuel gauge");
            UpdateFuelGauge();
        }
        if (oldSpeedInt != (int)playerSpeed && speedGauge != null && speedGauge.Length > 0)
        {
            UpdateSpeedGauge(playerSpeed);
            oldSpeedInt = (int)playerSpeed;
        }
    }

    public void UpdateCoinText()
    {
        int playerCoins = PlayerPrefs.GetInt("coins");
        coinsText.text = string.Format("Coins: ${0}", playerCoins);
    }

    public void UpdateFuelGauge()
    {
        float playerFuel = player.fuel;
        int nMarkers = (int)(playerFuel / fuelPerGaugeMarker);
        for (int i = 0; i < nMarkers; i++)
        {
            //fuelGauge[i].SetActive(true);
            fuelGauge[i].GetComponent<Image>().enabled = true;
        }
        for (int i = nMarkers; i < fuelGauge.Length; i++)
        {
            //fuelGauge[i].SetActive(false);
            fuelGauge[i].GetComponent<Image>().enabled = false;
        }
    }

    public void UpdateSpeedGauge(float playerSpeed)
    {
        //playerSpeed = (playerTransform.position.x - initPlayerPosition - playerPosition) / Time.deltaTime;
        int nMarkers = (int)(playerSpeed / speedPerGaugeMarker);
        for (int i = 0; i < nMarkers; i++)
        {
            //fuelGauge[i].SetActive(true);
            speedGauge[i].GetComponent<Image>().enabled = true;
        }
        for (int i = nMarkers; i < speedGauge.Length; i++)
        {
            //fuelGauge[i].SetActive(false);
            speedGauge[i].GetComponent<Image>().enabled = false;
        }
    }
}
