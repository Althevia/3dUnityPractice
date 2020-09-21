using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private bool zoom = false;
    public GameObject miniCam;
    public RectTransform minimapSquare;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleZoom()
    {
        if (zoom == false)
        {
            zoom = true;
            miniCam.transform.localPosition = new Vector3(0,200,0);
            minimapSquare.sizeDelta = new Vector2(160, 160);
            minimapSquare.localPosition += new Vector3(-40, -40, 0);

        }
        else
        {
            zoom = false;
            miniCam.transform.localPosition = new Vector3(0, 140, 0);
            minimapSquare.sizeDelta = new Vector2(120, 120);
            minimapSquare.localPosition += new Vector3(40, 40, 0);
        }
    }

}
