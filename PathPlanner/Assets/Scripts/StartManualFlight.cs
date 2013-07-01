using UnityEngine;
using System.Collections;

public class StartManualFlight : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		// Make UAV movable
        UILabel label = GameObject.Find("lblStartPause").GetComponent<UILabel>();
        if (label.text == "Start")
        {
            GameObject.Find("UAV").GetComponent<FlyManual>().fly = true;
            label.text = "Pause";
        }
        else
        {
            GameObject.Find("UAV").GetComponent<FlyManual>().fly = false;
            label.text = "Start";
        }
	}
}
