using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Knight : Player
{
    //Prefab
    public GameObject block;

    public GameObject ghostBlock;

    public GameObject invisiBLock;


    //Clones used temporarily to showcase location
    private GameObject blockClonesTemp;

    //Used to keep track of blockClonesTemp's existence
    bool cloneTempIsExistent;

    //The ray
    Ray ray;
    //The ray's hit reference
    RaycastHit hit;

    //Knows whether player is inside the block
    public bool isInside;
    //public bool oneMoreFrame;

    int blockGhostMask = 1 << 8;
    int playerMask = 1 << 9;

    int multiMask;

    // Start is called before the first frame update
    void Start()
    {
        isInside = false;
        cloneTempIsExistent = false;



    }

    private void FixedUpdate()
    {


    }

    // Update is called once per frame
    void Update()
    {
       // oneMoreFrame = false;
        //used to check if the clone exists or not to avoid deleting something that doesn't exist
        if (cloneTempIsExistent)
        {
            Destroy(GameObject.FindGameObjectWithTag("ghostBlock"));
            
            //oneMoreFrame = true;

            cloneTempIsExistent = false;
        }

        multiMask = blockGhostMask | playerMask;

        multiMask = ~multiMask;
        /*
        Debug.Log("isInside");
        Debug.Log(isInside);
        Debug.Log("Cloneisexistent");
        Debug.Log(cloneTempIsExistent);
        */
        //if game is on
        // if proper phase
        // if not loading
        //if not in menu



        //MAKES SURE THAT PLAYER IS NOT INSIDE OF WHERE BLOCK WOULD BE PLACED
        //if (!isInside)
        // {
       
        Vector3 hitPosition;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        /*
       ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       if (Physics.Raycast(ray, out hit, 100.0f, multiMask))
       {
            Vector3 temp = findPosition(hit);
            temp.y += 0.5f;
            hitPosition = hit.point;
            blockClonesTemp = invisiBLock;
            blockClonesTemp = Instantiate(blockClonesTemp, temp, findRotation(hit));
            //blockClonesTemp.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            


            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       //used to create the ghost
       */

        if (Physics.Raycast(ray, out hit, 100.0f, multiMask))
        {
            Vector3 temp = findPosition(hit);
            //Quaternion temp2 = findRotation(hit);
            temp.y += 0.5f;
            hitPosition = hit.point;
            blockClonesTemp = ghostBlock;
            //temp2 = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            blockClonesTemp = Instantiate(blockClonesTemp, temp, findRotation(hit));
            //blockClonesTemp.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            cloneTempIsExistent = true;
           
        }

        if (!isInside)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, multiMask))
                {
                    Rigidbody thisCubesRb;
                    GameObject placedBlock;
                    Vector3 temp = findPosition(hit);
                    temp.y += 0.5f;
                    hitPosition = hit.point;
                    placedBlock = Instantiate(block, temp, findRotation(hit));
                    thisCubesRb = placedBlock.GetComponent<Rigidbody>();
                    //thisCubesRb.Sleep();
                    
                }

            }
        }
        else
        {
            isInside = false;
            blockClonesTemp.GetComponent<MeshRenderer>().enabled = false;
        }

            
        
        

    } 
    




        Vector3 findPosition(RaycastHit position)
    {
        return position.point;

    }

    Quaternion findRotation(RaycastHit rotation)
    {
        return rotation.transform.rotation;
    }
    
}  

    
