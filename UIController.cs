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
    private void Start() 
    {
        playerTransform = GameObject.Find("Player").transform;
        initPlayerPosition = playerTransform.position.x;
        playerPosition = initPlayerPosition;
        playerSpeed = 0;
        player = playerTransform.gameObject.GetComponent<PlayerController>();
        UpdateCoinText();
    }
    private void FixedUpdate() 
    {
        if (playerTransform!=null)
        {
            OnUpdateFuelText(player.fuel);
        }
        
    }
    public void OnUpdateFuelText(float fuel)
    {
        int fuelint = (int)fuel;
        playerSpeed = (playerTransform.position.x-initPlayerPosition - playerPosition) / Time.deltaTime;
        playerPosition = (playerTransform.position.x-initPlayerPosition);
        string fuelstring = string.Format("Fuel: {0}       Distance: {1}       Speed: {2}",fuelint,(int)playerPosition,(int)playerSpeed);
        fuelText.text = fuelstring;
    }

    public void UpdateCoinText()
    {
        int playerCoins = PlayerPrefs.GetInt("coins");
        coinsText.text = string.Format("Coins: ${0}", playerCoins);
    }

    public void UpdateFuelGauge()
    {
        float playerFuel = player.fuel;
        int nMarkers = (int) (playerFuel / fuelPerGaugeMarker);
        for (int i=0; i<nMarkers; i++)
        {
            fuelGauge[i].SetActive(true);
        }
        for (int i=nMarkers; i<fuelGauge.Length; i++)
        {
            fuelGauge[i].SetActive(false);
        }
    }
}
