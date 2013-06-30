using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class StartUpManual : StartUpChores {

	// Use this for initialization
	void Start () {
        base.Start();
		
		int timer = ProjectConstants.intFlightDuration * 60;
		int second = timer % 60;
		int minute = timer / 60;
		GameObject.Find("lblFlightTime").GetComponent<UILabel>().text = minute.ToString() + ":" + second.ToString("00");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
