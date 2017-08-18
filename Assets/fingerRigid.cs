using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fingerRigid : MonoBehaviour {
    GameObject fingerParent;
	// Use this for initialization
	void Start () {
        //Initial rigid part to 
        fingerParent = transform.parent.gameObject;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.GetComponent<CapsuleCollider>().center = fingerParent.GetComponent<CapsuleCollider>().center;
        this.GetComponent<CapsuleCollider>().direction = fingerParent.GetComponent<CapsuleCollider>().direction;
        this.GetComponent<CapsuleCollider>().height = fingerParent.GetComponent<CapsuleCollider>().height;
        this.GetComponent<CapsuleCollider>().radius = fingerParent.GetComponent<CapsuleCollider>().radius;

    }

    private void OnTriggerEnter(Collider col)
    {
        if (isHand(col))
            return;
        Rigidbody colRigid = col.GetComponent<Rigidbody>();
        if (!colRigid) //obj not exsit rigidbody
            return;
        if (!fingerParent.GetComponent<FingerControl>().grabbing)
        {
           // print("Enter: "+this.name + " : " + col.gameObject.name);
            fingerParent.GetComponent<FingerControl>().grabObject = col.gameObject;
            fingerParent.GetComponent<FingerControl>().stopBend = true;
            //Physics.IgnoreCollision(col.GetComponent<Collider>(), fingerParent.GetComponent<Collider>(), true);
            //fingerParent.GetComponent<FingerControl>().grabbing = true;
        }

    }
    private void OnTriggerExit(Collider col)
    {
        Rigidbody colRigid = col.GetComponent<Rigidbody>();
        if (!colRigid) //obj not exsit rigidbody
            return;
        if (col.gameObject == fingerParent.GetComponent<FingerControl>().grabObject)
        {
           // print("EXIST:  "+ this.name + " : " + col.gameObject.name);
            fingerParent.GetComponent<FingerControl>().grabObject = null;
            fingerParent.GetComponent<FingerControl>().stopBend = false;
            //Physics.IgnoreCollision(col.GetComponent<Collider>(), fingerParent.GetComponent<Collider>(), false);
            // fingerParent.GetComponent<FingerControl>().grabbing = false;
        }
    }
    private bool isHand(Collider col)
    {
        bool result = false;
        if(col.GetComponent<FingerControl>())
            return true;
        if (col.gameObject.name == "Hand_right"
            || col.gameObject.name == "Foreram_right"
            ||col.gameObject.name == "Wrist_right")
            return true;
        if (col.GetComponent<fingerRigid>())
            return true;
        return result;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
