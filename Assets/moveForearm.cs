using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveForearm : MonoBehaviour {

    Vector3 moveDirection;
    Vector3 curHandDirection;
    public Vector3 MoveSpeed;
    bool isMoving, isRotating, isWrist;
    float zeroCheck;
    float forearmRotationX, forearmRotationW, forearmangularX;
    Quaternion wristRotation;
    //float wristRotation;
    GameObject wrist;
	// Use this for initialization
	void Start () {
        wrist = GameObject.Find("Wrist_right");
        zeroCheck = 0.005f;
        moveDirection = Vector3.zero;
        //MoveSpeed = new Vector3(1, 1, 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void FixedUpdate()
    {
        MoveForearm(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        //MoveForearm(moveDirection);
        ControlForearm();
       // ControlWrist();
    }
    
    void MoveForearm(Vector3 direction)
    {
        if (Input.GetKey(KeyCode.E))
        {
            direction.y += 0.5f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction.y += -0.5f;
        }
        if (direction.magnitude < zeroCheck)
        {
            curHandDirection = Vector3.zero;
            return;
        }
        curHandDirection = direction;
        isMoving = true;
    }
    void ControlForearm()
    {
        if (isMoving)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.Lerp(
                this.GetComponent<Rigidbody>().velocity,
                new Vector3(
                curHandDirection.x * MoveSpeed.x,
                curHandDirection.y * MoveSpeed.y,
                curHandDirection.z * MoveSpeed.z),
                1.0f);

            isMoving = false;
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.Lerp(
                this.GetComponent<Rigidbody>().velocity,
                Vector3.zero,
                1.0f);
        }
        if (isRotating)
        {
            this.GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Euler(forearmangularX,0,0);
            //this.GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(forearmRotationX,0, 0, forearmRotationW);
            isRotating = false;
        }
        

    }
    void ControlWrist()
    {
       // wrist.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (isWrist)
        {
            wrist.GetComponent<ConfigurableJoint>().targetRotation = wristRotation;
            isRotating = false;
            return;
        }
        //else
        //{
        //    wrist.GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(wrist.GetComponent<ConfigurableJoint>().targetRotation, Quaternion.identity, 1.0f);
        //    //wristRotation = wrist.GetComponent<ConfigurableJoint>().targetRotation.eulerAngles.x;
        //}
    }
    public void getForearmRotation(float rotationX, float rotationW, float angularX)
    {
        forearmangularX += angularX;
        forearmangularX = Mathf.Clamp(
            forearmangularX,
            this.GetComponent<ConfigurableJoint>().lowAngularXLimit.limit,
            this.GetComponent<ConfigurableJoint>().highAngularXLimit.limit);

        forearmRotationW = rotationW;
        forearmRotationX = rotationX;
        
        isRotating = true;
       // print(rotationW);
    }
    public void getWristRotation(Quaternion rotation)
    {
        //wristRotation += rotation;
        //wristRotation = Mathf.Clamp(
        //    wristRotation,
        //    wrist.GetComponent<ConfigurableJoint>().lowAngularXLimit.limit,
        //    wrist.GetComponent<ConfigurableJoint>().highAngularXLimit.limit);

        wristRotation = rotation;
        isWrist = true;
    }
    public void getDirection(Vector3 direction)
    {
        moveDirection = direction;

    }
}
