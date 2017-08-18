using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pencilGrab : MonoBehaviour {

    GameObject hand;
    public int grabStatus = 0;
    public bool penGrabbing;
	// Use this for initialization
	void Start () {
        penGrabbing = false;
        hand = GameObject.Find("Hand_right");
	}
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<FixedJoint>())
            penGrabbing = true;
        else
            penGrabbing = false;
        if (penGrabbing)
        {
            checkGrabGesture();
        }
	}

    void checkGrabGesture()
    {
        print("Grabbing Fingers" + hand.GetComponent<InputManager>().grabbingFingers.Count);
       
        if(!hand.GetComponent<InputManager>().grabbingFingers.Contains(hand.GetComponent<InputManager>().ringFinger)
            && !hand.GetComponent<InputManager>().grabbingFingers.Contains(hand.GetComponent<InputManager>().pinkyFinger)
            && hand.GetComponent<InputManager>().grabbingFingers.Contains(hand.GetComponent<InputManager>().indexFinger))
        {
            grabStatus = 1; //pick hold pencil gesture
            return;
        }
        else
        {
            grabStatus = 0;
            return;
        }
    }

}
