using UnityEngine;
using System.Collections;

public class FlyModeManual : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {        
        FlyManual fm = GameObject.Find("UAV").GetComponent<FlyManual>();        
        UILabel label = GameObject.Find("lblTurnStrafe").GetComponent<UILabel>();
        if (label.text == "Turn")
        {
            label.text = "Strafe";
            fm.flyMode = FlyManual.FlyMode.Turn;
        }
        else
        {
            label.text = "Turn";
            fm.flyMode = FlyManual.FlyMode.Strafe;
            // Rotate UAV to face forward
            GameObject.Find("UAV").transform.rotation = Quaternion.identity;
        }
    }
}
