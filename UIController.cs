using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Text fuelText;
    float initPlayerPosition;
    public float playerPosition = 0;
    float playerSpeed;
    Transform playerTransform;
    PlayerController player;
    private void Start() 
    {
        playerTransform = GameObject.Find("Player").transform;
        initPlayerPosition = playerTransform.position.x;
        playerPosition = initPlayerPosition;
        playerSpeed = 0;
        player = playerTransform.gameObject.GetComponent<PlayerController>();
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
}
