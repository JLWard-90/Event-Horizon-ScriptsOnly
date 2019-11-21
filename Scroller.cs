using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float scrollSpeed = 0;
    GameObject[] blackHoles;
    Transform playerTransform;
    private void Awake() 
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    private void Update() 
    {
        blackHoles = GameObject.FindGameObjectsWithTag("BlackHole");
        foreach(GameObject bH in blackHoles)
        {
            Vector3 scrollVector = new Vector3(-scrollSpeed,0,0); //Negative scroll speed because we want stationary objects to move from right to left
            bH.transform.position += (scrollVector*Time.deltaTime);
        }
    }
}
