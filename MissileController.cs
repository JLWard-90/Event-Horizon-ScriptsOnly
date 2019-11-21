using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField]
    float missileSpeed = 1.5f;
    float destructionTime = 0.5f;
    bool leftField = false;
    float destructionTimert = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (transform.position.x - (missileSpeed*Time.deltaTime),transform.position.y,transform.position.z);
        if (leftField)
        {
            destructionTimert += Time.deltaTime;
            if (destructionTimert > destructionTime)
            {
                GameObject.Destroy(this.gameObject);
            }
        }   
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayArea")
        {
            leftField = true;
        }
    }
}
