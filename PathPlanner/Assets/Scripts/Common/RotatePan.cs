using UnityEngine;
using System.Collections;

public class RotatePan : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// When clicked
    void OnClick()
    {
        // First change label back and forth
        UILabel label = GameObject.Find("lblPanRotate").GetComponent<UILabel>(); 
		
        if (label.text == "Pan")
        {
			if(Camera.mainCamera.GetComponent<Orbit>() != null)
			{
	            label.text = "Rotate";
				Assets.Scripts.ProjectConstants.navMode = 2;				
				Camera.mainCamera.GetComponent<Orbit>().gesture = Orbit.Gesture.Pan;
			}
        }
        else
        {
			if(Camera.mainCamera.GetComponent<Orbit>() != null)
			{
				label.text = "Pan";
				Assets.Scripts.ProjectConstants.navMode = 1;
				Camera.mainCamera.GetComponent<Orbit>().gesture = Orbit.Gesture.Rotate;
			}
        }        
    }

}
