using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanAndLookat : MonoBehaviour {

    public Transform[] waypointsForCamera = new Transform[2];
    public GameObject viewFocus;
    private int currentWayPoint;
    private Transform targetWayPoint;
    private float lerpNumber;
    private Vector3 actualUsedtarget;

    private bool hasPaused;

	// Use this for initialization
	void Start () {
        currentWayPoint = 0;
        hasPaused = false;
	}
	
	// Update is called once per frame
	void Update () {
        
        actualUsedtarget = new Vector3(viewFocus.transform.position.x, viewFocus.transform.position.y, viewFocus.transform.position.z);
        transform.LookAt(actualUsedtarget);
        StartCoroutine(waitBeforePan());
        if (currentWayPoint != 2)
        {


            transform.Rotate(0, 0, 90);
        }
        if (hasPaused)
        {

            
            if (currentWayPoint < this.waypointsForCamera.Length)
            {
                if (targetWayPoint == null)
                    targetWayPoint = waypointsForCamera[currentWayPoint];
                walk();
            }
            if (currentWayPoint == 2)
            {
                transform.position = waypointsForCamera[1].position;
                transform.eulerAngles = new Vector3(90, 0, 0);
            }
        }

        
    }

    void walk()
    {
        if (Vector3.Distance(transform.position, waypointsForCamera[1].position) < 0.2f)
        {
            currentWayPoint++;

        }
        //1 for 7.5 seconds / 1.65 for 3 seconds / 2.5 for 2 seconds
        lerpNumber = Time.deltaTime * 1.65f;
        
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, targetWayPoint.position, lerpNumber);
        
        
        
        if (this.gameObject.transform.position == targetWayPoint.position)
        {
            currentWayPoint++;
            targetWayPoint = waypointsForCamera[currentWayPoint];

        }
    }
    IEnumerator waitBeforePan()
    {
        //match so that gamemanager wait before start = pan time + thiswaittime
        yield return new WaitForSeconds(0.0f);
        hasPaused = true;
    }
}

