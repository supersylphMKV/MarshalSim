using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemControl : MonoBehaviour {

    public Camera mainCam;
    public bool isDay = true;
    public bool simStart = false;
    public GameObject[] unityLightsParent;
    public List<GameObject> spriteLights = new List<GameObject>();
    public UnityEngine.UI.Image ccam;
    public Camera cockpitCam;
    public GameObject[] aircrafts;
    public Transform spawn;
    public int activeCraft = 0;
    public GameObject activeCraftObj;
    public UnityEngine.UI.Text prkInd;
    public UnityEngine.UI.Text brkInd;
    public UnityEngine.UI.Text spdInd;

    public Skybox[] CamSkybox;
    public Light MainLight;
    public Color dayAmbient;
    public Color nightAmbient;
    public Material daySkybox;
    public Material nightSkybox;

    public Material spotlightsMtr;
    public Color spotColor;
    public int spotSize = 10;
    public Material blueOmniMtr;
    public Color blueOmniClr;
    public int blueOmniSize = 3;

    // Use this for initialization
    void Start()
    {
        SpriteLights.Init(0, 0, mainCam.fieldOfView, Screen.height);
        SpriteLights.LightData[] lightData = new SpriteLights.LightData[unityLightsParent[0].transform.childCount];

        for (int i = 0; i < lightData.Length; i++)
        {
            lightData[i] = new SpriteLights.LightData();
            lightData[i].size = spotSize;
            lightData[i].backColor = spotColor;
            lightData[i].rotation = unityLightsParent[0].transform.GetChild(i).rotation;
            lightData[i].position = unityLightsParent[0].transform.GetChild(i).position;
            lightData[i].brightness = 1;
        }
        spriteLights.AddRange(SpriteLights.CreateLights("Spots", lightData, spotlightsMtr, UnityEngine.Rendering.IndexFormat.UInt16));

        lightData = new SpriteLights.LightData[unityLightsParent[2].transform.childCount];
        for (int i = 0; i < lightData.Length; i++)
        {
            lightData[i] = new SpriteLights.LightData();
            lightData[i].size = blueOmniSize;
            lightData[i].frontColor = blueOmniClr;
            lightData[i].rotation = unityLightsParent[2].transform.GetChild(i).rotation;
            lightData[i].position = unityLightsParent[2].transform.GetChild(i).position;
            lightData[i].brightness = 1;
        }
    
        spriteLights.AddRange(SpriteLights.CreateLights("BlueOmni", lightData, blueOmniMtr, UnityEngine.Rendering.IndexFormat.UInt16));

        SetAircraft(activeCraft);
        SetDay();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetAircraft()
    {
        SetAircraft(activeCraft);
    }
    
    public void StartSim()
    {

    }

    public void StopSim()
    {

    }

    public void SetTime(int val)
    {
        if(val == 0)
        {
            SetNight();
        }
        else
        {
            SetDay();
        }
    }

    public void SetNight()
    {
        MainLight.intensity = 0f;
        foreach(Skybox s in CamSkybox)
        {
            s.material = nightSkybox;
        }
        //CamSkybox.material = nightSkybox;
        RenderSettings.ambientSkyColor = nightAmbient;
        foreach(GameObject l in unityLightsParent)
        {
            l.SetActive(true);
        }
       foreach(GameObject g in spriteLights)
        {
            g.SetActive(true);
        }

        Plane_Control craftCtrl = activeCraftObj.GetComponent<Plane_Control>();
        craftCtrl.LightsCtrl(true);
    }

    public void SetDay()
    {
        MainLight.intensity = 1f;
        foreach (Skybox s in CamSkybox)
        {
            s.material = daySkybox;
        }
        RenderSettings.ambientSkyColor = dayAmbient;
        foreach (GameObject l in unityLightsParent)
        {
            l.SetActive(false);
        }
        foreach (GameObject g in spriteLights)
        {
            g.SetActive(false);
        }

        Plane_Control craftCtrl = activeCraftObj.GetComponent<Plane_Control>();
        craftCtrl.LightsCtrl(false);
    }

    public void SetCockpitDisplay(float val)
    {
        ccam.rectTransform.sizeDelta = new Vector2(800f * val, 400 * val);
    }

    public void SetAircraft(int val)
    {
        cockpitCam.transform.parent = null;
        if (activeCraftObj)
        {
            Destroy(activeCraftObj);
        }

        activeCraftObj = Instantiate(aircrafts[val], spawn.position, spawn.rotation);
        Plane_Control craftCtrl = activeCraftObj.GetComponent<Plane_Control>();
        craftCtrl.SetupAircraft(mainCam,cockpitCam, prkInd, brkInd, spdInd);
        activeCraft = val;
    }

    public void QuitSystems()
    {
        Application.Quit();
    }
}
