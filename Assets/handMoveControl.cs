using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handMoveControl : MonoBehaviour {

    Vector3 moveDirection;
    float speed = 5.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        moveHand1();
	}

    void moveHand1()
    {
        CharacterController controller = GetComponent<CharacterController>();
        //if (controller.isGrounded)
        // {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // print(moveDirection);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        //}
        // moveDirection.y -= gravity * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q))
        {

            moveDirection.y -= 5;
        }
        if (Input.GetKey(KeyCode.E))
        {
            moveDirection.y += 5;
        }

        controller.Move(moveDirection * Time.deltaTime);
    }
}
