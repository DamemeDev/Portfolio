using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpManager : MonoBehaviour {


    static powerUpManager instance;

    public GameObject[] powerUpAtPosition = new GameObject[3];

    public Transform[] powerupPlacement = new Transform[3];

    //0: Health, 1: Slo-mo 2: Fast Shot
    private int typeOfPower;
    private int currentPositionChecker;

    //keeps track of how many shooting power ups each player has
    public static int powersForOne;
    public static int powersForTwo;

    

    //FOR SLOMO
    public static int slomoPlayer;
    public static float slomoCoefficcient;

    //FOR FASTSHOT
    public static int fastShotPlayer;
    //Decides how fast the fast shot goes
    public static float fastShotCoefficient;

    //Keeps track of status for UI
    public static bool fastShotShot1;
    public static bool fastShotShot2;

    //use to lockout check status of fast shot
    private static bool fastLockOut;



    public GameObject health;
    public GameObject fastShot;
    public GameObject slomo;
    public GameObject nothing;

    private bool healthLockout;
    private bool fastShotLockout;
    private bool slomoLockout;
    private bool roundResetLockout;
    private bool roundResetLockout2;
    private bool powerTypeIsAvailable;

    static private bool[] hasBeenUsed = new bool[3];

    SingleLinkedList willBeUsed = new SingleLinkedList();

    public static bool flyTime;

    public float timeToWait;

    //allows to refer to the left and right spawns
    public GameObject leftSpawn;
    public GameObject rightSpawn;

    //allows to refer to the first and second player
    public GameObject player1;
    public GameObject player2;

    //Locksoutwaitforplanning
    private bool roundResetLockout3;

    //counts how many powerups have flown all the way
    public static int flownCounter;

    //flying speed
    public static float flyingSpeed;

    //distance away so that it gets detected well
    public static float distanceAway;


    internal class Node
    {
        internal int data;
        internal Node next;
        public Node(int d)
        {
            data = d;
            next = null;
        }
    }
    internal class SingleLinkedList
    {
        internal Node head;
    }
    internal void InsertLast(SingleLinkedList singlyList, int new_data)
    {
        Node new_node = new Node(new_data);
        if (singlyList.head == null)
        {
            singlyList.head = new_node;
            return;
        }
        Node lastNode = GetLastNode(singlyList);
        lastNode.next = new_node;
    }
    internal Node GetLastNode(SingleLinkedList singlyList)
    {
        Node temp = singlyList.head;
        while (temp.next != null)
        {
            temp = temp.next;
        }
        return temp;
    }
    internal void removeFirst(SingleLinkedList singlyList)
    {
        singlyList.head = singlyList.head.next;
    }
    internal int checkPositionData(SingleLinkedList singlyList, int index)
    {
        Node temp = singlyList.head;
        for (int x = 0; x < index; x++)
        {
            temp = temp.next;
        }
        return temp.data;
    }
    int amountOfElementsInList(SingleLinkedList singlylist)
    {
        int counter = 0;
        Node temp = singlylist.head;
        while (temp != null)
        {
            temp = temp.next;
            counter++;
        }
        return counter;
    }
  






    public static powerUpManager Instance
    {
        get
        {
            return instance;
        }
    }

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
    void Start()
    {

        //Use to change flying speed
        flyingSpeed = 5f;

        //use to change distance away
        distanceAway = 4.0f;

        fastShotShot1 = false;
        fastShotShot2 = false;
        fastShotPlayer = 0;
        fastShotCoefficient = 3f;
        powersForOne = 0;
        powersForTwo = 0;
        roundResetLockout = false;
        slomoPlayer = 0;
        roundResetLockout2 = false;
        roundResetLockout3 = false;


        //USE THIS TO SET SLOMO DIFFERENTLY
        slomoCoefficcient = 0.6f;

        flownCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.combatIsOver)
        {
            flyTime = true;
            if (!roundResetLockout3)
            {
                if (flownCounter == gameManager.powerPerRound)
                {
                    StartCoroutine(waitForPlanning());
                    roundResetLockout3 = true;
                }
            }
            

        }
        else
        {
            roundResetLockout3 = false;
        }
        
        if (gameManager.planning)
        {
            flownCounter = 0;
            roundResetLockout2 = false;

            if (!fastShotLockout)
            {
                if (fastShotPlayer == 1)
                {
                    fastShotLockout = true;
                    fastShotShot1 = true;
                }

                if (fastShotPlayer == 2)
                {
                    fastShotLockout = true;
                    fastShotShot2 = true;
                }
            }




            if (!roundResetLockout)
            {
                
                for (int x = 0; x < powerUpAtPosition.Length; x++)
                {
                    powerUpAtPosition[x] = nothing;
                    hasBeenUsed[x] = false;
                    

                }
                
                    
                for (int x = 0; x < gameManager.powerPerRound; x++)
                {
                    powerTypeIsAvailable = false;
                    if (gameManager.printPower)
                    {
                        print("New go around!");
                    }
                    
                    if (gameManager.powertypes == 1)
                    {
                        typeOfPower = Random.Range(0, 3);
                        currentPositionChecker = Random.Range(0, 3);

                        while (checkIfEmpty(powerUpAtPosition, currentPositionChecker, health, slomo, fastShot) != true)
                        {
                            currentPositionChecker = Random.Range(0, 3);
                        }
                        /*for (int r = 0; r < hasBeenUsed.Length; r++)
                        {
                            print(hasBeenUsed[r]);
                            if (hasBeenUsed[r] == false && typeOfPower == r)
                                powerTypeIsAvailable = true;
                        }*/

                        do
                        {
                            if (gameManager.printPower)
                            {
                                print("availability checker status:");
                                print(powerTypeIsAvailable);
                            }
                            for (int r = 0; r < hasBeenUsed.Length; r++)
                            {
                                if (hasBeenUsed[r] == false && typeOfPower == r)
                                    powerTypeIsAvailable = true;
                                if (gameManager.printPower)
                                {
                                    print("index being checked:");
                                    print(r);
                                    print("state of hasbeenUsed:");
                                    print(hasBeenUsed[r]);
                                    print("type of power:");
                                    print(typeOfPower);
                                }
                            }
                            if (powerTypeIsAvailable == true)
                            {
                                break;
                            } else
                            {
                                typeOfPower = Random.Range(0, 3);
                            }
                        } while (powerTypeIsAvailable != true);

                        for (int y = 0; y < hasBeenUsed.Length; y++)
                        {
                           

                            if (hasBeenUsed[y] == false)
                            {
                                if (gameManager.printPower)
                                {
                                    print("POWER BEING INSERTED:");
                                    print(y);
                                }
                                InsertLast(willBeUsed, y);
                            }
                        }
                        
                        

                        for (int y = 0; y < amountOfElementsInList(willBeUsed); y++)
                        {
                            if (gameManager.printPower)
                            {
                                print("amountOfElementsInList:");
                                print(amountOfElementsInList(willBeUsed));
                                print("the power type at the index");
                                print(checkPositionData(willBeUsed, y));
                                print("the current power type being checked:");
                                print(typeOfPower);
                            }
                          
                            if (checkPositionData(willBeUsed, y) == typeOfPower)
                            {
                                
                                if (typeOfPower == 0)
                                {
                                    powerUpAtPosition[currentPositionChecker] = health;
                                    hasBeenUsed[0] = true;
                                    GameObject temp = Instantiate(health, powerupPlacement[currentPositionChecker].position, health.transform.rotation);
                                    temp.SetActive(true);
                                    temp.GetComponent<HealthUp>().lane = currentPositionChecker;

                                    break;
                                }
                                else if (typeOfPower == 1)
                                {
                                    powerUpAtPosition[currentPositionChecker] = slomo;
                                    hasBeenUsed[1] = true;
                                    GameObject temp = Instantiate(slomo, powerupPlacement[currentPositionChecker].position, slomo.transform.rotation);
                                    temp.SetActive(true);
                                    temp.GetComponent<Slomo>().lane = currentPositionChecker;


                                    break;
                                }
                                else if (typeOfPower == 2)
                                {
                                    powerUpAtPosition[currentPositionChecker] = fastShot;
                                    hasBeenUsed[2] = true;
                                    GameObject temp = Instantiate(fastShot, powerupPlacement[currentPositionChecker].position, slomo.transform.rotation);
                                    temp.SetActive(true);
                                    temp.GetComponent<FastShot>().lane = currentPositionChecker;


                                    break;
                                }
                            }
                      
                        }

                    }
                    for (int d = 0; d < amountOfElementsInList(willBeUsed); d++)
                    {
                        removeFirst(willBeUsed);
                    }
                    powerTypeIsAvailable = true;
                }
                
                roundResetLockout = true;
            }

        }


        if (gameManager.combat)
        {
            if (roundResetLockout2)
            {
                fastShotShot1 = false;
                fastShotShot2 = false;
                roundResetLockout2 = true;
            }
            roundResetLockout = false;
            fastShotLockout = false;
        }

    }
    static bool checkIfEmpty(GameObject[] checkedArray, int placement, GameObject health, GameObject slomo, GameObject fastShot)
    {
        if (checkedArray[placement] == health || checkedArray[placement] == slomo || checkedArray[placement] == fastShot)
        {
            return false;
        }
        else { return true; }
    }

    IEnumerator waitForPlanning()
    {
        yield return new WaitForSeconds(timeToWait);
        flyTime = false;
        gameManager.startOfPlanning = true;
        powersForOne = 0;
        powersForTwo = 0;
        fastShotPlayer = 0;
        slomoPlayer = 0;
        gameManager.round++;
        gameManager.combatEndChecker = 0;
        if (gameManager.basicPrint)
            print("back to planning");
        gameManager.initialTimerValue = Time.time;
        gameManager.combatIsOver = false;
        gameManager.combat = false;
        gameManager.planning = true;
    }
}

    
   
   
