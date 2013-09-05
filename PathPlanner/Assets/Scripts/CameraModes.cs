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

		UIButton pr = GameObject.Find("btnRotate").GetComponent<UIButton>();
		
		// Only enable one camera.
        if (this.gameObject.name == "btnGlobal")
        {
            GameObject.Find("CameraGlobal").GetComponent<Camera>().enabled = true;
			SetCurCam("CameraGlobal");
			pr.isEnabled = false;			
        }
        if (this.gameObject.name == "btnBirdEye")
        {
            GameObject.Find("CameraBirdEye").GetComponent<Camera>().enabled = true;
			SetCurCam("CameraBirdEye");
			pr.isEnabled = false;			
        }
        if (this.gameObject.name == "btnBehind")
        {
            GameObject.Find("CameraBehind").GetComponent<Camera>().enabled = true;
			SetCurCam("CameraBehind");
			pr.isEnabled = false;			
        }
        if (this.gameObject.name == "btnFreeForm")
        {
            GameObject.Find("CameraFree").GetComponent<Camera>().enabled = true;
			SetCurCam("CameraFree");
			pr.isEnabled = true;			
        }
    }
	
	void SetCurCam(string name)
	{
		if(GameObject.Find ("ControlCenter").GetComponent<StartUpManual>()!=null)
		{
			GameObject.Find ("ControlCenter").GetComponent<StartUpManual>().curCam = GameObject.Find(name).GetComponent<Camera>();			
		}
		else
		{
			GameObject.Find ("ControlCenter").GetComponent<StartUpPattern>().curCam = GameObject.Find(name).GetComponent<Camera>();	
		}	
	}
}
