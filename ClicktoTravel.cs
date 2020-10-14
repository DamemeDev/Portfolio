using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicktoTravel : MonoBehaviour {

    //player
    public GameObject player;

    //the game manager
    GameObject gameManager;

    //the emissive material that makes stuff glow
    private Material thisMaterial;

    //the distance between the player and a pad
    private float distanceFromPlayer;

    //the boolean that stops calculation of distance between teleport pads and player
    private bool stopComputingDistance;

    //the boolean that determines whether the pad can be used or not (locked or unlocked)
    public bool locked;

    //used to hold removedpads
    public GameObject startPadSix;
    public GameObject startPadSeven;
    public GameObject EndPadSix;
    public GameObject EndPadSeven;

    public bool clickLockout;
    
    // Use this for initialization
    void Start () {
        //finds material
        thisMaterial = this.gameObject.GetComponent<Renderer>().material;
        //print(this.gameObject.GetComponent<Transform>().position);

        //starts out by computing the distance
        stopComputingDistance = false;

        clickLockout = false;
	}

    // Update is called once per frame
    void Update() {

        //only works if there is a need to find the distance between player and pad, no longer needed once unlocked
        if (!stopComputingDistance)
        {
            //This if statement prevents nullpointerreference exceptions because when map is on the player no longer exists
            if (!GameManager.mapIsOn)
            {
                distanceFromPlayer = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
            }

            //determines the distance the player needs to be from a pad to unlock it
            if (distanceFromPlayer <= 10)
            {
                locked = false;
                print("unlocked");
            }
        }

        //this makes it so that once it locks it stops computing distance
        if (!locked)
        {
            stopComputingDistance = true;
        }
        //print(distanceFromPlayer);
        //Debug.Log("player: " + GameObject.FindGameObjectWithTag("Player"));
       // Debug.Log("pad: " + this.gameObject);
       if (GameManager.flyIsDone || (!GameManager.mapIsOn && !GameManager.flyCamIsOn))
        {
            
            



           

            GameManager.flyIsDone = false;
            //print("CLICK LOCKOUT IS OFF");
            clickLockout = false;
        }
    }

    //this is what happens when the mouse hovers over the object
    private void OnMouseOver()
    {
        //this only works if the map is on and the pad is not locked, basically no emission or fast travel if locked
        if (GameManager.mapIsOn && !locked && !GameManager.flyCamIsOn)
        {
            //print(GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().secondUpPos);
            //this turns on emission, glows when hovering over
            thisMaterial.EnableKeyword("_EMISSION");
            //when the left mouse button is clicked, re-activate player, turn off map, and teleport
            if (Input.GetMouseButton(0))
            {
                if (!clickLockout)
                {
                    startPadSix.SetActive(true);
                    startPadSeven.SetActive(true);
                    EndPadSix.SetActive(true);
                    EndPadSeven.SetActive(true);
                    player.GetComponent<JetPack>().jetpackIsAllowed = false;
                    GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().endPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                    //print(GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().endPos);
                    GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().secondUpPos = new Vector3(GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().endPos.x, GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().endPos.y + GameManager.cameraHeight, GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().endPos.z);
                   // print(gameObject.transform.position);
                    //print(GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().endPos.position);
                   // print(GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().secondUpPos.position);
                    GameManager.flyCamIsOn = true;


                    //print("padlocation set to");
                    GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().padLocationVector = gameObject.transform.position;
                    //print(GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().padLocationVector);
                    clickLockout = true;
                    //GameManager.mapSwitch = true;

                    /*print("state of the characterController");
                    print(GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>().characterController.activeSelf);
                    //GameManager.player.transform.position.Set(0f, 0f, 0f); */
                }
            }
        }
    }

    //what happens when the mouse stops hovering over the object
    private void OnMouseExit()
    {
        //turn off emission when no longer hovering over
        thisMaterial.DisableKeyword("_EMISSION");
    }
}
