using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
public struct FingerData
{
    public short Index;
    public short Middle;
    public short Ring;
    public short Pinky;
    public short Thumb;
};

public class InputManager : MonoBehaviour {

    //Dll function (P5 Glove)
    [DllImport("P5GloveDLL")]
    //private static extern int PrintANumber(); 
    private static extern void P5Glove_Reset();
    [DllImport("P5GloveDLL")]
    private static extern void P5Glove_Start();
    [DllImport("P5GloveDLL")]
    private static extern void P5Glove_Stop();
    [DllImport("P5GloveDLL")]
    private static extern char P5Glove_GetIndexFinger();
    [DllImport("P5GloveDLL")]
    private static extern FingerData P5Glove_GetFingers();

    //Hand part objects
    private GameObject forearm;
    private GameObject wristLocation;
    private GameObject wrist;
    private GameObject handCenter;

    //Fingers array
    public GameObject[] thumbFinger;
    public GameObject[] indexFinger;
    public GameObject[] middleFinger;
    public GameObject[] ringFinger;
    public GameObject[] pinkyFinger;

    // Transform Index, Middle, Ring, Pinky, Thumb;
    float indexValue = 0, middleValue = 0, ringValue = 0, pinkyValue = 0, thumbValue = 0;

    //Move parameters
    private Vector3 moveDirection;
    GameObject viveTracker;
    private Vector3 curHandDirection;
    public Vector3 MoveSpeed;
    bool isMoving;

    //Grab Parameters
    GameObject thumbGrabObj;
    GameObject grabbingObj;
    public bool grabbing = false;
    public List<GameObject[]> grabbingFingers;

    // Use this for initialization
    void Start() {
        grabbingFingers = new List<GameObject[]>();
        MoveSpeed = new Vector3(8, 120, 8);
        Debug.Log("Start");
        P5Glove_Start();

        forearm = GameObject.Find("Forearm_right");
        handCenter = GameObject.Find("Hand_right");
        wrist = GameObject.Find("Wrist_right");
        thumbFinger = GameObject.FindGameObjectsWithTag("thumb");
        indexFinger = GameObject.FindGameObjectsWithTag("index");
        middleFinger = GameObject.FindGameObjectsWithTag("middle");
        ringFinger = GameObject.FindGameObjectsWithTag("ring");
        pinkyFinger = GameObject.FindGameObjectsWithTag("pinky");
        wristLocation = GameObject.Find("wristLocation");
        moveDirection = Vector3.zero;

        grabbingObj = null;
    }


    void Update() {
        FingerData fingerdata = P5Glove_GetFingers();
        indexValue = (float)fingerdata.Index;
        middleValue = (float)fingerdata.Middle;
        ringValue = (float)fingerdata.Ring;
        pinkyValue = (float)fingerdata.Pinky;
        thumbValue = (float)fingerdata.Thumb;


        //Configuration all fingers at one time.
        if (Input.GetKeyDown(KeyCode.Z))
        {
            configurationAllFingersMax();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            configurationAllFingersMin();
        }

        //Object Grab function
        setObjectGrabbed();

        //Update finger bend value
        passFingerBendValue();

        if (countBendingFingers() < 2)
        {
           
            grabbing = false;
        }

    }


    void passFingerBendValue()
    {
        foreach (GameObject thumbPart in thumbFinger)
        {
            thumbPart.GetComponent<FingerControl>().passBendValue(thumbValue);
        }
        foreach (GameObject indexPart in indexFinger)
        {
            indexPart.GetComponent<FingerControl>().passBendValue(indexValue);
        }
        foreach (GameObject middlePart in middleFinger)
        {
            middlePart.GetComponent<FingerControl>().passBendValue(middleValue);
        }
        foreach (GameObject ringPart in ringFinger)
        {
            ringPart.GetComponent<FingerControl>().passBendValue(ringValue);
        }
        foreach (GameObject pinkyPart in pinkyFinger)
        {
            pinkyPart.GetComponent<FingerControl>().passBendValue(pinkyValue);
        }
    }

    void configurationAllFingersMax()
    {
        foreach (GameObject thumbPart in thumbFinger)
        {
            thumbPart.GetComponent<FingerControl>().setFingerMax(thumbValue);
        }
        //thumbFinger.GetComponent<FingerControl>().deviceValue = thumbValue;
        foreach (GameObject indexPart in indexFinger)
        {
            indexPart.GetComponent<FingerControl>().setFingerMax(indexValue);
        }
        foreach (GameObject middlePart in middleFinger)
        {
            middlePart.GetComponent<FingerControl>().setFingerMax(middleValue);
        }
        foreach (GameObject ringPart in ringFinger)
        {
            ringPart.GetComponent<FingerControl>().setFingerMax(ringValue);
        }
        foreach (GameObject pinkyPart in pinkyFinger)
        {
            pinkyPart.GetComponent<FingerControl>().setFingerMax(pinkyValue);
        }
    }
    void configurationAllFingersMin()
    {
        foreach (GameObject thumbPart in thumbFinger)
        {
            thumbPart.GetComponent<FingerControl>().setFingerMin(thumbValue);
        }
        //thumbFinger.GetComponent<FingerControl>().deviceValue = thumbValue;
        foreach (GameObject indexPart in indexFinger)
        {
            indexPart.GetComponent<FingerControl>().setFingerMin(indexValue);
        }
        foreach (GameObject middlePart in middleFinger)
        {
            middlePart.GetComponent<FingerControl>().setFingerMin(middleValue);
        }
        foreach (GameObject ringPart in ringFinger)
        {
            ringPart.GetComponent<FingerControl>().setFingerMin(ringValue);
        }
        foreach (GameObject pinkyPart in pinkyFinger)
        {
            pinkyPart.GetComponent<FingerControl>().setFingerMin(pinkyValue);
        }
    }



    GameObject getGrabObject(GameObject[] finger)
    {

        GameObject res = null;
        foreach (GameObject fingerPart in finger)
        {
            if (fingerPart.GetComponent<FingerControl>().grabObject != null)
            {
                res = fingerPart.GetComponent<FingerControl>().grabObject;
            }
        }
        return res;
    }

    void setObjectGrabbed()
    {
        thumbGrabObj = getGrabObject(thumbFinger);
        if (thumbGrabObj == null)
        {
            grabbing = false;

        } else
        {
            if (grabbingObj == null) // if already holding one, not processing grab.
            {
                processFingerGrab();
            }

        }
        if (!grabbing && grabbingObj != null)
        {
            setObjectRelease();
        }
        //grabbing = false;

    }

    void processFingerGrab()
    {
        if (thumbGrabObj == getGrabObject(indexFinger))
        {
            grabbingFingers.Add(indexFinger);
            ignoreFingerCollision(indexFinger, thumbGrabObj);
            grabbing = true;
        }
        if (thumbGrabObj == getGrabObject(middleFinger))
        {
            grabbingFingers.Add(middleFinger);
            ignoreFingerCollision(middleFinger, thumbGrabObj);
            grabbing = true;
        }
        if (thumbGrabObj == getGrabObject(ringFinger))
        {
            grabbingFingers.Add(ringFinger);
            ignoreFingerCollision(ringFinger, thumbGrabObj);
            grabbing = true;
        }
        if (thumbGrabObj == getGrabObject(pinkyFinger))
        {
            grabbingFingers.Add(pinkyFinger);
            ignoreFingerCollision(pinkyFinger, thumbGrabObj);
            grabbing = true;
        }
        if (grabbing)
        {
            grabbingFingers.Add(thumbFinger);
            ignoreFingerCollision(thumbFinger, thumbGrabObj);
            grabObject();
        }
        return;

    }
    void grabObject()
    {
        print("grab obj");
        //print(thumbGrabObj.name);
        grabbingObj = thumbGrabObj;
        thumbGrabObj.AddComponent<FixedJoint>();
        thumbGrabObj.GetComponent<FixedJoint>().connectedBody = wrist.GetComponent<Rigidbody>();
        thumbGrabObj.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
        //thumbGrabObj.transform.parent = wristLocation.transform;

        //reset grab obj position
        //thumbGrabObj.transform.position = wristLocation.transform.position;

        float objY;
        objY = thumbGrabObj.GetComponent<Renderer>().bounds.size.y;
        //thumbGrabObj.transform.position += new Vector3(0, -objY/2, 0);

        //thumbGrabObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //hold in hand, freeze holding object's rotation and position (only can be changed by parent transform)

       
        // thumbGrabObj.GetComponent<Collider>().enabled = false;
        thumbGrabObj.GetComponent<Rigidbody>().useGravity = false;
       
        return;
    }
    void setObjectRelease()
    {
        print("Release");

        Destroy(grabbingObj.GetComponent<FixedJoint>());
        if (grabbingFingers != null)
        {
            foreach (GameObject[] grabbingFinger in grabbingFingers)
            {
                setFingerRelease(grabbingFinger);
               
            }
        }
        grabbingFingers.Clear();
        //grabbingObj.transform.parent = null;
        grabbingObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        grabbingObj.GetComponent<Collider>().enabled = true;
        grabbingObj.GetComponent<Rigidbody>().useGravity = true;
        grabbingObj = null;
        grabbing = false;

    }
    int countBendingFingers()
    {
        int result = 0;
        if(grabbingFingers != null)
        {
            foreach (GameObject[] grabbingFinger in grabbingFingers)
            {
               if(grabbingFinger[0].GetComponent<FingerControl>().isBending)
                {
                    result++;
                }
                //foreach (GameObject fingerPart in grabbingFinger)
                //{
                //    if (!fingerPart.GetComponent<FingerControl>().isBending)
                //    {
                //        result++;
                //        break;
                //    }
                //}
            }
        }
        //print("Count Fingers: " + result);
        return result;
    }
    void setFingerRelease(GameObject[] finger)
    {
        foreach (GameObject fingerPart in finger)
        {
            fingerPart.GetComponent<FingerControl>().stopBend = false;
            if (fingerPart.GetComponent<FingerControl>().grabObject != null)
            {
               // fingerPart.GetComponent<FingerControl>().stopBend = false;
                Physics.IgnoreCollision(fingerPart.GetComponent<FingerControl>().grabObject.GetComponent<Collider>(), fingerPart.GetComponent<Collider>(), false);
                fingerPart.GetComponent<FingerControl>().grabObject = null;
                
            }
        }
    }
    void ignoreFingerCollision(GameObject[] finger, GameObject col)
    {
        foreach (GameObject fingerPart in finger)
        {
            fingerPart.GetComponent<FingerControl>().stopBend = true;

            if (fingerPart.GetComponent<FingerControl>().grabObject != null)
            {
                
                Physics.IgnoreCollision(fingerPart.GetComponent<FingerControl>().grabObject.GetComponent<Collider>(), fingerPart.GetComponent<Collider>(), true);
               
            }
        }
    }
}
