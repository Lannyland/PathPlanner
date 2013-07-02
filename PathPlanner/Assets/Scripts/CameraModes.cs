using UnityEngine;
using System.Collections;

public class CameraModes : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        // Disable all cameras        
        GameObject.Find("CameraGlobal").GetComponent<Camera>().enabled = false;
        GameObject.Find("CameraFree").GetComponent<Camera>().enabled = false;
        GameObject.Find("CameraBirdEye").GetComponent<Camera>().enabled = false;
        GameObject.Find("CameraBehind").GetComponent<Camera>().enabled = false;

        // Only enable one camera.
        if (this.gameObject.name == "btnGlobal")
        {
            GameObject.Find("CameraGlobal").GetComponent<Camera>().enabled = true;
		   	GameObject.Find ("ControlCenter").GetComponent<StartUpManual>().curCam = GameObject.Find("CameraGlobal").GetComponent<Camera>();
        }
        if (this.gameObject.name == "btnBirdEye")
        {
            GameObject.Find("CameraBirdEye").GetComponent<Camera>().enabled = true;
			GameObject.Find ("ControlCenter").GetComponent<StartUpManual>().curCam = GameObject.Find("CameraBirdEye").GetComponent<Camera>();
        }
        if (this.gameObject.name == "btnBehind")
        {
            GameObject.Find("CameraBehind").GetComponent<Camera>().enabled = true;
			GameObject.Find ("ControlCenter").GetComponent<StartUpManual>().curCam = GameObject.Find("CameraBehind").GetComponent<Camera>();
        }
        if (this.gameObject.name == "btnFreeForm")
        {
            GameObject.Find("CameraFree").GetComponent<Camera>().enabled = true;
			GameObject.Find ("ControlCenter").GetComponent<StartUpManual>().curCam = GameObject.Find("CameraFree").GetComponent<Camera>();
        }
    }
}
