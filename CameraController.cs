using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerTransform;
    // Start is called before the first frame update
    [SerializeField]
    float playerOffsetX = -5;
    public Vector2 offsetLimits = new Vector2(-8,-4);
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x+playerOffsetX,transform.position.y,transform.position.z);
        }
    }
}
