using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInit : MonoBehaviour {
    GameObject Forearm;
    GameObject Wrist;
    GameObject[] fingers;
    Vector3 forearmPrePos;
    Vector3 wristPrePos;
    public bool setToPre = false;
	// Use this for initialization
	void Start () {
        Forearm = GameObject.Find("Forearm_right");
        Wrist = GameObject.Find("Wrist_right");
        Physics.IgnoreCollision(Forearm.GetComponent<Collider>(), Wrist.GetComponent<Collider>(), true);

        foreach (Transform child in Wrist.transform)
        {
            //child.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //child.GetComponent<Rigidbody>().mass = Mathf.Infinity;
            Physics.IgnoreCollision(Wrist.GetComponent<Collider>(), child.GetComponent<Collider>(), true);
            foreach (Transform grandC in child.transform)
            {
                Physics.IgnoreCollision(grandC.GetComponent<Collider>(), child.GetComponent<Collider>(), true);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        //if(!checkRigid())
        //{
        //    forearmPrePos = Forearm.transform.position;
        //    wristPrePos = Wrist.transform.position;
        //    //foreach (Transform child in Wrist.transform)
        //    //{
        //    //    child.GetComponent<FingerControl>().preFingerPos = child.transform.position;
        //    //    foreach(Transform grandC in child.transform)
        //    //    {
        //    //        grandC.GetComponent<FingerControl>().preFingerPos = grandC.transform.position;
        //    //    }
        //    //}
        //    // print(Forearm.GetComponent<Rigidbody>().velocity);
        //}
        //else{
        //    setToPre = true;
        //    //print("Forearm");
        //    Forearm.transform.position = forearmPrePos;
        //    Forearm.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    Forearm.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //    //print("wrist");
        //    Wrist.transform.position = wristPrePos;
        //    Wrist.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    Wrist.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //    //foreach(Transform child in Wrist.transform)
        //    //{
        //    //    child.transform.position = child.GetComponent<FingerControl>().preFingerPos;
        //    //    child.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    //    child.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //    //    foreach(Transform grandC in child.transform)
        //    //    {
        //    //        grandC.transform.position = grandC.GetComponent<FingerControl>().preFingerPos;
        //    //        grandC.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    //        grandC.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //    //    }
        //    //}
        //}
       // print(forearmPrePos);
    }
    bool checkRigid()
    {
        //bool isRigidMove = false;
        if(Forearm.GetComponent<Rigidbody>().velocity != Vector3.zero || Forearm.GetComponent<Rigidbody>().angularVelocity != Vector3.zero)
        {
            return true;
        }
        if (Wrist.GetComponent<Rigidbody>().velocity != Vector3.zero || Wrist.GetComponent<Rigidbody>().angularVelocity != Vector3.zero)
        {
            return true;
        }
        //foreach (Transform child in Wrist.transform)
        //{
        //    //child.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //    //child.GetComponent<Rigidbody>().mass = Mathf.Infinity;
        //    if (child.GetComponent<Rigidbody>().velocity != Vector3.zero || child.GetComponent<Rigidbody>().angularVelocity != Vector3.zero)
        //    {
        //        return true;
        //    }
        //    foreach (Transform grandC in child.transform)
        //    {
        //        if (grandC.GetComponent<Rigidbody>().velocity != Vector3.zero || grandC.GetComponent<Rigidbody>().angularVelocity != Vector3.zero)
        //        {
        //            return true;
        //        }
        //        //Physics.IgnoreCollision(Wrist.GetComponent<Collider>(), child.GetComponent<Collider>(), true);
        //    }
        //}
        return false;
    }
}
