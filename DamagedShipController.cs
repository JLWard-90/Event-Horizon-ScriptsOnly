using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedShipController : MonoBehaviour
{
    [SerializeField]
    GameObject explosionPrefab;
    float TimePassed = 0;
    float TimeofDeath = 2.5f;
    GameObject playerObj;
    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        float xpos = Random.Range(-0.5f,0.5f);
        float ypos = Random.Range(-2.0f,2.0f);
        Vector3 exp_pos = new Vector3(transform.position.x+xpos,transform.position.y+ypos,0);
        GameObject newExplosion = GameObject.Instantiate(explosionPrefab, exp_pos, transform.rotation);
        TimePassed += Time.deltaTime;
        if (TimePassed > TimeofDeath)
        {
            Vector3 targetVector = new Vector3(200,0,0);
            playerObj.GetComponent<Rigidbody2D>().AddForce(targetVector);
            GameObject bigExplosion = GameObject.Instantiate(explosionPrefab, transform.position, transform.rotation);
            bigExplosion.transform.localScale = new Vector3(7,7,7);
            GameObject.Destroy(this.gameObject);
        }
    }
}
