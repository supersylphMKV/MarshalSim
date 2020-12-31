using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_Control : MonoBehaviour {

    public Camera mainCam;
    public Material spotMat;
    public Material redMat;
    public Material greenMat;
    public Material strobeMat;
    public ConstantForce[] engines;
    public WheelCollider frontWheel;
    public WheelCollider[] wheels;
    public Transform rudder;
    public Transform vfwheel;
    public AudioSource engine;
    public GameObject cockpitCamHolder;
    public GameObject lightsHolder;
    public AnimationClip lightsAnimClip;

    public UnityEngine.UI.Text prkInd;
    public UnityEngine.UI.Text brkInd;
    public UnityEngine.UI.Text spdInd;

    [Range(0f,100f)]
    public float throttle = 0;
    [Range(-1f,1f)]
    public float steering = 0;
    [Range(0f, 1f)]
    public float brake = 0;
    public bool parkingBrake = true;
    public Vector3 vel;
    public float speed = 0;

    public float maxSteer = 45;
    public float maxThrust = 860000;
    public float maxBrake = 2000000;

    public float currentThrust;

    Vector3 visualSteer = new Vector3(0, 270f, 0);
    Vector3 visualWheel = new Vector3(0, 90f, 180f);
    Rigidbody rb;
    bool isLightOn = false;
   

	// Use this for initialization
	void Awake () {
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        prkInd.color = Color.green;
        brkInd.color = Color.green;
        vel = rb.velocity;
        speed = Mathf.Round(((rb.velocity.magnitude * 360f)/100f)*100)/100;
        spdInd.text = "Speed : " + speed + " km/h";
        if (Input.GetAxis("Reset") > 0)
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        throttle = 100f * ((Input.GetAxis("Throttle") + 1)/2);
        steering = Input.GetAxis("Horizontal");
        brake = Input.GetAxis("Brake");
        if(Input.GetAxis("ParkingBrake") > 0)
        {
            parkingBrake = !parkingBrake;
        }
        rudder.localEulerAngles = (Vector3.right * steering * maxSteer) + visualSteer;
        if (vfwheel)
        {
            vfwheel.Rotate(Vector3.down, (frontWheel.rpm / 60f) * Time.fixedDeltaTime, Space.Self);
        }
        
        //Debug.Log(frontWheel.rpm);

        foreach (ConstantForce c in engines)
        {
            float force =  maxThrust * (throttle / 100f);
            engine.pitch = ((throttle / 100f) + 1f);
            c.relativeForce = Vector3.back * force;
            currentThrust = force;
        }
        frontWheel.steerAngle = steering * maxSteer;
        
        
        foreach(WheelCollider w in wheels)
        {
            w.brakeTorque = brake * maxBrake;
            if(brake > 0)
            {
                brkInd.color = Color.red;
            }
            if (parkingBrake)
            {
                prkInd.color = Color.red;
                w.brakeTorque = maxBrake;
            }
        }

        if (isLightOn)
        {
            
        }
	}

    public void SetupAircraft(Camera main, Camera cockpit, UnityEngine.UI.Text parkingInd, UnityEngine.UI.Text brakeInd, UnityEngine.UI.Text speedInd)
    {
        mainCam = main;
        cockpit.transform.SetParent(cockpitCamHolder.transform);
        cockpit.transform.localPosition = Vector3.zero;
        cockpit.transform.localRotation = Quaternion.identity;
        prkInd = parkingInd;
        brkInd = brakeInd;
        spdInd = speedInd;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        foreach (WheelCollider w in wheels)
        {
            w.motorTorque = 0.000001f;
        }

    }

    public void LightsCtrl(bool val)
    {
        lightsHolder.SetActive(val);
        Animation lightsAnim = lightsHolder.GetComponent<Animation>();
        lightsAnim.wrapMode = WrapMode.Loop;
        if (val)
        {
            lightsAnim.Play(lightsAnimClip.name);
        }
        else
        {
            lightsAnim.Stop();
        }
    }
}
