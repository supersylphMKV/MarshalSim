using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragScreen : MonoBehaviour
{
    RectTransform rt;
    Vector2 initPos;
    Vector2 initPvt;
    private bool dragging;

    public void Start()
    {
        rt = GetComponent<RectTransform>();
        initPos = rt.anchoredPosition;
        initPvt = rt.pivot;
    }

    public void Update()
    {
        if (dragging)
        {
            float x = Input.mousePosition.x - (rt.sizeDelta.x * (0.5f - rt.pivot.x));
            float y = (Input.mousePosition.y - Screen.height) - (rt.sizeDelta.y * (0.5f - rt.pivot.y));
            Vector2 tempPos = new Vector2( x, y);
            rt.anchoredPosition = tempPos;
            //Debug.Log(Input.mousePosition);
            //Debug.Log(tempPos);
        }
    }

    public void OnPointerDown()
    {
        dragging = true;
        //rt.pivot = Vector2.one * 0.5f;
    }

    public void OnPointerUp()
    {
        dragging = false;
        //rt.pivot = initPvt;
    }

    public void OnReset()
    {
        rt.anchoredPosition = initPos;
    }
}