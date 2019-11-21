using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed=1;
    [SerializeField]
    float strength=1;
    [SerializeField]
    float fuelBurnRate=1; 
    [SerializeField]

    LevelController levelController;
    [SerializeField]
    GameObject explosionPrefab;
    public float fuel=100;
    UIController uI;

    public bool gameOver=false;
    Animator animator;

    public bool shieldOn = false;
    [SerializeField]
    GameObject shieldObject;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (!shieldOn)
        {
            shieldObject.SetActive(false);
        }
        uI = GameObject.Find("Canvas").GetComponent<UIController>();
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        Debug.Log(levelController);
        fuel = levelController.startingFuel;
        uI.OnUpdateFuelText(fuel);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator==null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        bool leftMouse = Input.GetMouseButton(0);
        if (leftMouse)
        {
            Debug.Log("Pressed button 0");
            //TurnToFacePosition();
            if(fuel > 0 && !gameOver)
            {
                AddThrustMouse();
            }
            else
            {
                animator.SetBool("UnderThrust",false);
            }
        }
        else
        {
            keyThrust();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Goal")
        {
            levelController.OnGoalEnter();
        }
        else if(other.gameObject.tag == "BlackHole")
        {
            Debug.Log("Destroyed by black hole");
            GameObject boom = GameObject.Instantiate(explosionPrefab);
            boom.transform.localScale = new Vector3(4,4,4);
            boom.transform.position = transform.position;
            levelController.EnteredBlackHole();
            GameObject.Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "Missile")
        {
            if (shieldOn)
            {
                GameObject boom = GameObject.Instantiate(explosionPrefab);
                boom.transform.localScale = new Vector3(4,4,4);
                boom.transform.position = transform.position;
                GameObject.Destroy(other.gameObject);
                shieldOn = false;
                shieldObject.SetActive(false);
            }
            else
            {
                Debug.Log("Destroyed by missile");
                GameObject boom = GameObject.Instantiate(explosionPrefab);
                boom.transform.localScale = new Vector3(4,4,4);
                boom.transform.position = transform.position;
                levelController.DestroyedByMissile();
                GameObject.Destroy(other.gameObject);
                GameObject.Destroy(this.gameObject);
            }
        }
        else if(other.gameObject.tag == "Shield")
        {
            GameObject.Destroy(other.gameObject);
            shieldOn = true;
            shieldObject.SetActive(true);
        }
        else if(other.gameObject.tag == "Fuel")
        {
            GameObject.Destroy(other.gameObject);
            fuel += 75;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayArea")
        {
            Debug.Log("Left play area");
            levelController.playAreaLeft();
        }
    }

    Vector3 MousePos()
    {
        Vector3 mouse = Input.mousePosition;
        Debug.Log(mouse);
        Vector3 position = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(mouse);
        Debug.Log(position);
        position = new Vector3(position.x,position.y,0);
        return position;
    }

    void TurnToFacePosition()
    {
        float angle = Vector3.Angle(MousePos(), transform.position);
        //Debug.Log(MousePos()-transform.position);
        float updown = (MousePos().y - transform.position.y) / Mathf.Abs(MousePos().y - transform.position.y);
        float leftright = (MousePos().x - transform.position.x) / Mathf.Abs(MousePos().x - transform.position.x);
        //Debug.Log(angle);
        Vector3 newRot = new Vector3(0,0,0);
        if (leftright >= 0)
        {
            newRot = new Vector3(0,0,(angle*updown));
        }
        else
        {
            newRot = new Vector3(0,0,(updown*90+angle*updown));
        }
        
        transform.rotation = Quaternion.Euler(newRot);
    }

    void AddThrustMouse()
    {
        fuel -= fuelBurnRate * Time.deltaTime;
        //uI.OnUpdateFuelText(fuel);
        Vector3 targetDir = MousePos() - transform.position;
        targetDir = targetDir / Vector3.Magnitude(targetDir);
        GetComponent<Rigidbody2D>().AddForce(targetDir * strength);
        animator.SetBool("UnderThrust",true);
    }

    void keyThrust()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        if (hAxis != 0 && fuel > 0 || vAxis != 0 && fuel > 0)
        {
            fuel -= fuelBurnRate * Time.deltaTime;
            Vector3 targetDir = new Vector3(hAxis,vAxis,0);
            targetDir = targetDir / Vector3.Magnitude(targetDir);
            GetComponent<Rigidbody2D>().AddForce(targetDir * strength);
            animator.SetBool("UnderThrust",true);
        }
        else
        {
            animator.SetBool("UnderThrust",false);
        }
    }
}
