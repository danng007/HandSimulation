using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveHandTest : MonoBehaviour {
    GameObject forearm;
    GameObject wrist;
    Vector3 _curForearmDirection;
    bool IsForearmMoving;
    public Space MovementRelativity = Space.World;
    public Vector3 MovementSpeed = new Vector3(8, 120, 8);
    // Use this for initialization
    void Start () {
        wrist = GameObject.Find("WristTest");
        forearm = GameObject.Find("ForearmTest");
        Physics.IgnoreCollision(wrist.GetComponent<Collider>(),forearm.GetComponent<Collider>(),true);
	}
	
	// Update is called once per frame
	void Update () {
        MoveForearm(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        ControlForearm();
    }
    void MoveForearm(Vector3 direction)
    {
        if (Mathf.Approximately(direction.magnitude, 0))
        {
            _curForearmDirection = Vector3.zero;
            return;
        }

        _curForearmDirection = direction;
        IsForearmMoving = true;
    }
    void ControlForearm()
    {

        if (IsForearmMoving)
        {
            forearm.GetComponent<Rigidbody>().velocity = Vector3.Lerp(
                forearm.GetComponent<Rigidbody>().velocity,
                GetForearmTargetVelocity(_curForearmDirection),
                0.2f);
            IsForearmMoving = false;
        }
       
    }
    Vector3 GetForearmTargetVelocity(Vector3 direction)
    {
        if (this.MovementRelativity == Space.Self)
            return this.transform.TransformDirection(new Vector3(
                direction.x * MovementSpeed.x,
                direction.y * MovementSpeed.y,
                direction.z * MovementSpeed.z));

        return new Vector3(
                direction.x * MovementSpeed.x,
                direction.y * MovementSpeed.y,
                direction.z * MovementSpeed.z);
    }
}
