using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;


public class FingerControl : MonoBehaviour
{
    public Quaternion fingerRotationTarget = new Quaternion(0, 0, 0, 1);
    public Quaternion fingerRotationStart = new Quaternion(1, 0, 0, 1);
    public AnimationCurve fingerCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    private float deviceValue = 0;
    float fingerRotationValue, smoothRotationValue, preRotationValue;
    public bool grabbing;
    public bool isBending;
    public bool stopBend;
    public Vector3 preFingerPos;
    float bendSpeed = 5;
    float maxValue = 63, minValue = 0;
    public float bendThreshold;
    // Use this for initialization
    public GameObject grabObject;
    public Transform grabObjectPar;
    public FixedJoint Joint { get; private set; }
    private Transform childFinger;
    void Start()
    {
        childFinger = null;
        foreach(Transform chil in this.transform)
        {
            if(chil.CompareTag(this.transform.tag))
            {
                childFinger = chil;
            }
        }
        
        preRotationValue = 0;
        grabbing = false;
        grabObject = null;
        if (this.GetComponent<Rigidbody>() != null)
        {
            this.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            this.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Rigidbody>().useGravity = false;
        }
        this.GetComponent<Collider>().isTrigger = false;
        isBending = false;
        fingerRotationStart = this.transform.localRotation;
        bendThreshold = (maxValue - minValue) / 3 + minValue;
        stopBend = false;
    }

    //void OnCollisionStay(Collision collision)
    //{
    //    //print(collision.gameObject.name);
    //    if (isBending)
    //    {
           
    //        grabObject = collision.gameObject;
    //        //print("Grab: "+grabObject.name);
    //        grabObjectPar = grabObject.transform.parent;
       
    //    }

    //}
    // Update is called once per frame
    void Update()
    {
        fingerRotationValue = (deviceValue - minValue) / (maxValue - minValue);
        if ((!stopBend||fingerRotationValue<preRotationValue))
        {
            if (childFinger!=null)
            {
                if (!childFinger.GetComponent<FingerControl>().stopBend){
                    this.transform.localRotation = Quaternion.Lerp(fingerRotationStart, fingerRotationTarget, fingerCurve.Evaluate(fingerRotationValue));
                    preRotationValue = fingerRotationValue;
                }
            }else
            {
                this.transform.localRotation = Quaternion.Lerp(fingerRotationStart, fingerRotationTarget, fingerCurve.Evaluate(fingerRotationValue));
                preRotationValue = fingerRotationValue;
            }
           
            //if(fingerRotationValue > preRotationValue)
            //    smoothRotationValue = Mathf.SmoothStep(smoothRotationValue, 1, fingerRotationValue - preRotationValue);
            //else
            //    smoothRotationValue = Mathf.SmoothStep(smoothRotationValue, 0, preRotationValue - fingerRotationValue);

            //print(fingerRotationValue - preRotationValue + " : "+smoothRotationValue);
            
        }
        //else
        //{
        //    fingerRotationValue = (deviceValue - minValue) / (maxValue - minValue);
        //    //smoothRotationValue = Mathf.SmoothStep(smoothRotationValue, 0, fingerRotationValue);
        //    this.transform.localRotation = Quaternion.Lerp(fingerRotationTarget, fingerRotationStart, fingerCurve.Evaluate(smoothRotationValue));
        //    stopBend = false;
        //}
        //print(this.name+" : "+collisionCount);
        
        //if (!isBending)
        //{
        //    grabObject = null;
        //    grabObjectPar = null;
        //}



        // print()

        if (deviceValue >= bendThreshold)
        {
            isBending = true;
        }
        else
        {
            isBending = false;
        }
        //valueChange = Time.deltaTime* 3;

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    fingerRotating = true;
        //}
        //else if(Input.GetKeyUp(KeyCode.Q))
        //{
        //    fingerRotating = false;
        //}

        //if (fingerRotating)
        //{
        //    fingerRotationValue = Mathf.SmoothStep(fingerRotationValue, 1, GetRotationChangeValue());
        //    this.transform.localRotation = Quaternion.Lerp(fingerRotationStart, fingerRotationTarget, fingerCurve.Evaluate(fingerRotationValue));
        //   // print(fingerRotationValue);
        //}
        //else
        //{
        //    fingerRotationValue = Mathf.SmoothStep(fingerRotationValue, 0, GetRotationChangeValue());
        //    //print(fingerRotationValue);
        //    this.transform.localRotation = Quaternion.Lerp(fingerRotationStart, fingerRotationTarget, fingerCurve.Evaluate(fingerRotationValue));
        //}
    }
    public void setFingerMax(float giveMax)
    {
        maxValue = giveMax;
        bendThreshold = (maxValue - minValue) / 3 + minValue;
    }
    public void setFingerMin(float giveMin)
    {
        minValue = giveMin;
        bendThreshold = (maxValue - minValue) / 3 + minValue;
    }
    float GetRotationChangeValue()
    {
        return Time.deltaTime * bendSpeed;
    }
    public void passBendValue(float passDeviceValue)
    {
        //if (!stopBend)
        //{
        //    deviceValue = passDeviceValue;
        //}else
        //{
        //    print(passDeviceValue + " L: " + deviceValue);
        //}
        deviceValue = passDeviceValue;
    }
}
