using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GameManager : MonoBehaviour {
    static GameManager instance;
  
    //bool that keeps track of the state of map
    public static bool mapIsOn;

    //camera for map
    public Camera mapCam;

    //camera for playing
    public Camera gameCam;

    //Camera for theatrics
    public Camera flyCam;

    //the player as well?
    public GameObject characterController;

    //whether or not the map can be opened, prevents flickering
    public bool mapCanBeInteracted;

    //the boolean that serves to switch between map and playing states
    public static bool mapSwitch;

    //lockout for efficiency
    private bool lockout1;

    //Keeps track of whether to use the alternate start or not
    public bool otherStart;

    //Keeps track of starting position
    public GameObject alternateStart;

    //Keeps a hold of map numbers
    public GameObject mapNumbers;


    //Keeps track of whether currently tranferring
    public static bool flyCamIsOn;

    private Vector3 firstPos;
    private Vector3 upPos;
    public Vector3 secondUpPos;
    public Vector3 endPos;

    private bool flyLock;
    private bool atFirst;
    private bool atUpPos;
    private bool atSecond;
    private bool atEnd;

    private float lerpNumber;

    public static bool flyIsDone;

    public Transform padLocation;

    public Vector3 padLocationVector;

    public static float cameraHeight = 100f;

    public float cameraSpeed = 1.5f;

    public float distanceFromPoints = 0.2f;

    //DO NOT REMOVE THIS, THIS ALLOWS THE GAME MANAGER TO BE STATIC
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    //DO NOT REMOVE THIS, THIS ALLOWS THE GAME MANAGER TO BE STATIC
    void Awake()
    {

        if (instance != this)
        {
            Destroy(instance);

        }

        instance = this;

        DontDestroyOnLoad(this);






    }

    // Use this for initialization
    void Start () {

        //map starts off
        mapIsOn = false;

        //map can be opened from get go
        mapCanBeInteracted = true;

        //efficiency lockout
        lockout1 = false;

        flyCamIsOn = false;
        flyCam.enabled = false;

        atFirst = false;
        atUpPos = false;
        atSecond = false;
        atEnd = false;
        flyLock = false;
        flyIsDone = false;

       

        


        if (otherStart)
            characterController.transform.position = alternateStart.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        /*print("pad location at beginning of update");
        print(padLocation.position);*/

        //makes game exit
        if (Input.GetKeyDown(KeyCode.Escape) && !mapIsOn)
        {
            print("application has quit");
            Application.Quit();
        } else if (Input.GetKeyDown(KeyCode.Escape) && mapIsOn)
        {
            mapSwitch = true;
        }
        //Cheat to get back to spot easily
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            characterController.transform.position = alternateStart.transform.position;
        }*/

        //if M is pressed and the map can be opened activate map and switch all things
        if (Input.GetKeyDown(KeyCode.M)  && mapCanBeInteracted)
        {
            mapSwitch = true;
        }

       
        //turns on everything for map and off everything for player and vice versa
        if (mapSwitch) {
            if (mapIsOn && !flyCamIsOn)
            {
                mapNumbers.SetActive(false);
                //turns map is on off and re-locks cursor
                mapIsOn = false;
                flyCamIsOn = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (!flyCamIsOn)
            {
                mapNumbers.SetActive(true);
                //turns map is on on and unlocks cursor
                mapIsOn = true;
                flyCamIsOn = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.lockState = CursorLockMode.Confined;
            }
            //print(mapIsOn);
            mapCanBeInteracted = false;
            //if (!lockout1)
            //{
            mapSwitch = false;
            //gives a second before map opens for all operations to go through
            StartCoroutine(waitForMapOpen());
                //lockout1 = true;
           // }
            
           
        }

        if (flyCamIsOn)
        {
            

            lerpNumber = cameraSpeed * Time.deltaTime;
            mapCanBeInteracted = false;
            mapNumbers.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (!flyLock)
            {
                //print("atfirst");
                //print(padLocationVector);
                atFirst = true;
                characterController.SetActive(true);
                //print(GameObject.FindGameObjectWithTag("Player").transform.position);
                firstPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                upPos = new Vector3(firstPos.x, firstPos.y + cameraHeight, firstPos.z);
                characterController.SetActive(false);
                flyCam.transform.position = firstPos;
                flyCam.enabled = true;
                gameCam.enabled = false;
                mapCam.enabled = false;
                flyLock = true;
                
            }

           /* print(firstPos.position);
            print(upPos.position);
            print(secondUpPos.position);
            print(endPos.position);
            */


            if (atEnd)
            {
                flyIsDone = true;
                mapIsOn = false;
                GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().characterController.SetActive(true);
                flyCam.enabled = false;
                flyCamIsOn = false;
                flyLock = false;
                mapCanBeInteracted = true;
                characterController.SetActive(true);
                //print("right before arrival:");
                //print(padLocation.position);
                //Debug.Log(padLocationVector);
                characterController.transform.position = padLocationVector;
                atEnd = false;
                atSecond = false;
                atUpPos = false;
                atFirst = false;
                //print("atEnd");
            }
            else if (atSecond)
            {
               
                flyCam.transform.position = Vector3.Lerp(flyCam.transform.position, endPos, lerpNumber);
            }
            else if (atUpPos)
            {
                
                flyCam.transform.position = Vector3.Lerp(flyCam.transform.position, secondUpPos, lerpNumber);
            }
            else if (atFirst)
            {
                
                flyCam.transform.position = Vector3.Lerp(flyCam.transform.position, upPos, lerpNumber);
            }

            if ((Vector3.Distance(flyCam.transform.position, upPos) < distanceFromPoints) && atFirst)
            {
                //print("atUp");
                atFirst = false;
                atUpPos = true;
            }
            else if ((Vector3.Distance(flyCam.transform.position, secondUpPos) < distanceFromPoints) && atUpPos)
            {
                //print("atSecond");
                atUpPos = false;
                atSecond = true;
            }
            else if ((Vector3.Distance(flyCam.transform.position, endPos) < distanceFromPoints) && atSecond)
            {
                atSecond = false;
                atEnd = true;
            }
            
            //ADD THIS AT THE END OF THE PROCESS
            //mapIsOn = false;
            //GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().characterController.SetActive(true);

        }

        //print(Cursor.lockState);
        //switches camera states and turns cursor on
        if (mapIsOn && !flyCamIsOn)
        {
            characterController.SetActive(false);
            gameCam.enabled = false;
            mapCam.enabled = true;
            Cursor.visible = true;
            

        }
        //switches camera states and turns cursor off
        else if(!flyCamIsOn && !mapIsOn)
        {

            characterController.SetActive(true);
            gameCam.enabled = true;
            mapCam.enabled = false;
            Cursor.visible = false;
            
        }
       /* print("pad location at end of update");
        print(padLocation.position);*/
    }
    //gives it a second so map can't be spammed and thus breaks
    IEnumerator waitForMapOpen()
    {
        yield return new WaitForSeconds(0.2f);
        //lockout1 = false;
        
        mapCanBeInteracted = true;
    }
}
