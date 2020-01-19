using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPrefabBehaviour : MonoBehaviour
{
    PlayerController player;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger");
        if (player == null) //Just in case the call on Start() did not execute properly
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }
        player.ShipTrigger2DColliderEnter(other);
    }
}
