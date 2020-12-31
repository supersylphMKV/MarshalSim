using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Rotator : MonoBehaviour {

    public float MaxAngle = 10f;
    public float MinAngle = -10f;

    public bool isUp = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isUp)
        {
            if(transform.eulerAngles.y < MaxAngle)
            {
                transform.Rotate(Vector3.up, Time.deltaTime);
            }
            else
            {
                isUp = false;
            }
        }
        else
        {
            if(transform.eulerAngles.y > MinAngle)
            {
                transform.Rotate(-Vector3.up, Time.deltaTime);
            }
            else
            {
                isUp = true;
            }
        }
	}
}
