using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    Transform playerTransform;
    Rigidbody2D playerRigidbody;
    [SerializeField]
    float strength;
    [SerializeField]
    float rotSpeed = 1;
    float exitTime;
    [SerializeField]
    float destructionTime = 0.5f;
    bool leftField = false;
    float destructionTimert = 0;
    private void Start() 
    {
        playerTransform = GameObject.Find("Player").transform;
        playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    private void Update() 
    {
        if (playerTransform!= null)
        {
            ApplyForceToPlayer();
        } 
        RotateBH();
        if (leftField)
        {
            destructionTimert += Time.deltaTime;
            if (destructionTimert > destructionTime)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    float CalculateDistanceToPlayer()
    {
        return Mathf.Sqrt(((playerTransform.position.x-transform.position.x)*(playerTransform.position.x-transform.position.x))+((playerTransform.position.y-transform.position.y)*(playerTransform.position.y-transform.position.y)));
    }

    float CalculateForce()
    {
        float distance = CalculateDistanceToPlayer();
        float force = strength / (distance*distance);
        return force;
    }

    Vector3 CalculateDirection()
    {
        Vector3 targetDir = transform.position - playerTransform.position;
        return targetDir;
    }

    void ApplyForceToPlayer()
    {
        playerRigidbody.AddForce(CalculateDirection() * CalculateForce());
    }

    void RotateBH()
    {
        float zAngle = rotSpeed * Time.deltaTime;
        this.transform.Rotate(0, 0, zAngle, Space.World);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayArea")
        {
            leftField = true;
        }
    }

}
