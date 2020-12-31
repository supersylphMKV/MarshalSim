using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamLoader : MonoBehaviour {

    public UnityEngine.UI.Toggle camEnable;
    public UnityEngine.UI.Dropdown camList;
    public UnityEngine.UI.Dropdown ratio;
    public UnityEngine.UI.Dropdown orientation;
    public UnityEngine.UI.Slider size;

    UnityEngine.UI.RawImage display;
    WebCamTexture tex;
    public RectTransform rt;
    int[] widths = new int[] { 800, 720, 640, 480, 854, 768 };

    // Use this for initialization
    void Start () {
        display = GetComponent<UnityEngine.UI.RawImage>();
        SetSize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitiatingCam(bool val)
    {
        if(tex == null)
        {
            tex = new WebCamTexture(camList.options[camList.value].text);
        }
        if(display == null)
        {
            display = GetComponent<UnityEngine.UI.RawImage>();
        }

        display.texture = tex;

        if (val)
        {
            tex.Play();
        }
        else
        {
            tex.Stop();
        }
        
    }

    public void SetRatio(int val)
    {
        if(val == 3)
        {
            orientation.interactable = false;
        }
        else
        {
            orientation.interactable = true;
        }

        SetSize();
    }

    public void SetSize()
    {
        
        if(rt == null)
        {
            rt = transform.GetComponentInParent<RectTransform>();
        }

        int or = orientation.value;
        int ro = ratio.value;

        Debug.Log("set size " + ro);
        if (or == 0)
        {
            rt.sizeDelta = new Vector2(widths[ro] * size.value, 480 * size.value);
        }
        else
        {
            Debug.Log("portrait");
            rt.sizeDelta = new Vector2(480 * size.value, widths[ro] * size.value);
        }
    }

    public void SelectCam()
    {
        tex = new WebCamTexture(camList.options[camList.value].text);

        InitiatingCam(camEnable.isOn);
    }

    public void DetectCam()
    {
        if(WebCamTexture.devices.Length > 0)
        {
            camList.options.Clear();
            List<string> dList = new List<string>();

            for(int d = 0; d < WebCamTexture.devices.Length; d++)
            {
                Debug.Log(d);
                dList.Add(WebCamTexture.devices[d].name);
            }

            camList.AddOptions(dList);

            camEnable.interactable = true;
            camList.interactable = true;

            SelectCam();
        }
        else
        {
            camEnable.interactable = false;
            camList.interactable = false;
        }
    }
}
