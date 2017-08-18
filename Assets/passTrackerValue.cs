using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class passTrackerValue : MonoBehaviour
{
    GameObject forearm;
    GameObject wrist, handCenter;
    bool isLeft;
    // Use this for initialization
    float rotationX;
    float preRotationW, startW,currentW;
    float xyScale;
    Quaternion wristRotation;
    Vector3 moveDirection;
    Vector3 prePosition;
    SteamVR_Controller.Device device;
    public SteamVR_TrackedObject trackedObj;
    void Start()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        xyScale = 2.0F;
        wrist = GameObject.Find("Wrist_right");
        forearm = GameObject.Find("Forearm_right");
        
       // forearm.transform.position = this.transform.position;
        //prePosition = forearm.transform.position;
        //wristRotation.eulerAngles = new Vector3(90, 0, 0);
    }

    private void FixedUpdate()
    {
        // print(device.velocity);
        // print("Tracker: " + this.transform.position + "  Obj: " + forearm.transform.position);
        // moveDirection = this.transform.position - forearm.transform.position;
        // print(moveDirection);
        //prePosition = this.transform.position;
        forearm.GetComponent<moveForearm>().getDirection(device.velocity);
        //hand.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1.5F, this.transform.position.z);

        //forearm.GetComponent<moveForearm>().getForearmRotation(this.transform.rotation.eulerAngles.x - rotationX);
        forearm.GetComponent<moveForearm>().getWristRotation(this.transform.rotation);
       
        forearm.GetComponent<moveForearm>().getForearmRotation(this.transform.rotation.x, this.transform.rotation.w,  device.angularVelocity.x);
        preRotationW = this.transform.rotation.w;
        
        // forearm.GetComponent<Rigidbody>().angularVelocity = device.angularVelocity;
        //forearm.transform.rotation = Quaternion.Euler(device.angularVelocity);
        //wristRotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);

        //if (wristRotation.eulerAngles.x < 360)
        //{
        //    wristRotation *= Quaternion.Euler(90, 0, 0);
        //}
        //wrist.transform.rotation = wristRotation;

        //moveHand();
    }
    // Update is called once per frame
    void Update()
    {
        if (rotationX == 0)
        {
            rotationX = this.transform.rotation.eulerAngles.x;
            startW = this.transform.rotation.w;
        }
    }
}
