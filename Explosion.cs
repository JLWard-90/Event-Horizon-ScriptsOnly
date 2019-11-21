using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    float timeTodeath = 0.85f;
    float timePassed = 0;

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed > timeTodeath)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
