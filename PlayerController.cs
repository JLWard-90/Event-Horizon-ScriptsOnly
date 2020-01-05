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

    Rigidbody2D playerRigidBody;
    //The following parameters all refer to the constant forward burn mode:
    [SerializeField]
    float autoThrustFuelBurnRate;
    [SerializeField]
    bool constantForwardBurn;
    [SerializeField]
    float constantForwardBurnStrength;
    [SerializeField]
    float maxForwardSpeed;
    float initPlayerPosition;
    float playerPosition;
    float playerSpeed;
    [SerializeField]
    float smoothTime;
    float thrustTimer = 0;
    //End of constant forward burn mode parameters
    //The following parameters are to enable the player to use different ship types:
    public int currentPlayerShip = 0; //The index of the ship model
    [SerializeField]
    public GameObject[] shipModelArray;

    //Get the sound effects manager:
    sfxManager soundEffects;

    // Start is called before the first frame update

    void Start()
    {
        if (PlayerPrefs.HasKey("currentShipIndex"))
        {
            currentPlayerShip = PlayerPrefs.GetInt("currentShipIndex");
        }
        GameObject shipModel = GameObject.Instantiate(shipModelArray[currentPlayerShip]);
        shipModel.transform.SetParent(transform);
        shipModel.transform.position = new Vector3(-7, 0, 0);
        animator = GetComponentInChildren<Animator>();
        if (!shieldOn)
        {
            shieldObject.SetActive(false);
        }
        uI = GameObject.Find("Canvas").GetComponent<UIController>();
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        Debug.Log(levelController);
        fuel = levelController.startingFuel;
        
        
        //Getting initial position info:
        initPlayerPosition = transform.position.x;
        playerPosition = initPlayerPosition;
        
        //Making sure initial speed is set to 0:
        playerSpeed = 0;
        //Grabbing the Rigidbody2D component
        playerRigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        //Checking that we have the animator:
        if (animator == null)
        {
            animator = GameObject.Find("thrustAnimation").GetComponent<Animator>();
        }
        if (constantForwardBurn)
        {
            Debug.Log("Turning animation on");
            animator.SetBool("UnderThrust", true);
        }
        else
        {
            Debug.Log("Animation not on");
        }
        soundEffects = GameObject.Find("GameController").GetComponent<sfxManager>();
        //Update the fuel counter:
        uI.OnUpdateFuelText(fuel);
        Debug.Log("Completed ship startup");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRigidBody == null)
        {
            playerRigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void FixedUpdate()
    {
        bool leftMouse = Input.GetMouseButton(0);
        if (leftMouse)
        {
            Debug.Log("Pressed button 0");
            //TurnToFacePosition();
            if (fuel > 0 && !gameOver)
            {
                AddThrustMouse();
            }
            else if (!constantForwardBurn)
            {
                animator.SetBool("UnderThrust", false);
            }
        }
        else
        {
            keyThrust();
        }
        if (constantForwardBurn)
        {
            Autothrust();
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
            soundEffects.PlayExplosion2();
            GameObject.Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "Missile")
        {
            if (shieldOn)
            {
                GameObject boom = GameObject.Instantiate(explosionPrefab);
                //Direction in which the missile explosion will push the player:
                Vector3 targetDir = new Vector3(-1, 0, 0); //The explosion wil push the player backwards
                //Get the strength of the explosion from the missile object:
                float explosionStrength = other.gameObject.GetComponent<MissileController>().explosionStrength;
                //Apply a force of strength explosionStrength to the player object
                GetComponent<Rigidbody2D>().AddForce(targetDir * explosionStrength);
                boom.transform.localScale = new Vector3(4,4,4);
                boom.transform.position = transform.position;
                GameObject.Destroy(other.gameObject);
                shieldOn = false;
                shieldObject.SetActive(false);
                soundEffects.PlayExplosion2();
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
                soundEffects.PlayExplosion1();
            }
        }
        else if(other.gameObject.tag == "Shield")
        {
            soundEffects.PlayShieldSound();
            GameObject.Destroy(other.gameObject);
            shieldOn = true;
            shieldObject.SetActive(true);
            
        }
        else if(other.gameObject.tag == "Fuel")
        {
            soundEffects.PlayFuelSound();
            GameObject.Destroy(other.gameObject);
            fuel += 50;
            uI.UpdateFuelGauge();
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
        //uI.OnUpdateFuelText(fuel);
        Vector3 targetDir = MousePos() - transform.position;
        if (constantForwardBurn)
        {
            targetDir.x = 0;
            if (targetDir.y == 0)
            {
                //Do nothing so no thrust is added to the rigidBody.
            }
            else
            {
                targetDir = targetDir / Vector3.Magnitude(targetDir); //else normalise as normal
                fuel -= fuelBurnRate * Time.deltaTime;
            }
        }
        else
        {
            targetDir = targetDir / Vector3.Magnitude(targetDir);
            fuel -= fuelBurnRate * Time.fixedDeltaTime;
        }
        playerRigidBody.AddForce(targetDir * strength * Time.fixedDeltaTime);
        animator.SetBool("UnderThrust",true);
    }

    void keyThrust()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        if ((hAxis != 0 && fuel > 0 && !constantForwardBurn) || (vAxis != 0 && fuel > 0))
        {
            fuel -= fuelBurnRate * Time.fixedDeltaTime;
            Vector3 targetDir = new Vector3(hAxis,vAxis,0);
            
            targetDir = targetDir / Vector3.Magnitude(targetDir);
            
            playerRigidBody.AddForce(targetDir * strength * Time.fixedDeltaTime);
            animator.SetBool("UnderThrust",true);
        }
        else if (!constantForwardBurn)
        {
            animator.SetBool("UnderThrust",false);
        }
    }

    public void Autothrust()
    {
        if (thrustTimer > smoothTime && smoothTime != 0)
        {
            playerSpeed = (transform.position.x - initPlayerPosition - playerPosition) / smoothTime;
            playerPosition = (transform.position.x - initPlayerPosition);
            if (fuel > 0 && playerSpeed < maxForwardSpeed)
            {
                Vector3 targetDir = new Vector3(0, 0, 0);
                targetDir.x = 1;
                playerRigidBody.AddForce(targetDir * constantForwardBurnStrength * smoothTime);
                fuel -= autoThrustFuelBurnRate;
                animator.SetBool("UnderThrust", true);
            }
            thrustTimer = 0;
        }
        else if (smoothTime != 0)
        {
            thrustTimer += Time.fixedDeltaTime;
        }
        else
        {
            playerSpeed = (transform.position.x - initPlayerPosition - playerPosition) / Time.fixedDeltaTime;
            playerPosition = (transform.position.x - initPlayerPosition);
            if (fuel > 0 && playerSpeed < maxForwardSpeed)
            {
                Vector3 targetDir = new Vector3(0, 0, 0);
                targetDir.x = 1;
                playerRigidBody.AddForce(targetDir * constantForwardBurnStrength * Time.fixedDeltaTime);
                fuel -= autoThrustFuelBurnRate * Time.fixedDeltaTime;
            }
        }
    }
}
