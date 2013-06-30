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
		GameObject.Find("UAV").GetComponent<FlyManual>().fly = true;
	}
}
