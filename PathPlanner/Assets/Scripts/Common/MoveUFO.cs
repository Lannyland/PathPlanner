using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Common;

public class MoveUFO : MonoBehaviour {
	
	public Transform UAV;
	
	private bool grabUAV = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(grabUAV)
		{
			Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,4.0f));
			// Debug.Log("Camera world space = " + p);								
			UAV.transform.position = p;
		}
	}
	
	// When mouse button is pressed
	void OnMouseDown()
	{
	  	Debug.Log ("UAV Position=" + UAV.transform.position);
		Vector2 UAVScreenPos = Camera.main.WorldToScreenPoint(UAV.transform.position);
		Vector2 Mouse2DPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Debug.Log("UAVScreenPos=" + UAVScreenPos);
		Debug.Log ("Mouse2DPos=" + Mouse2DPos);
		float distance = (UAVScreenPos - Mouse2DPos).magnitude;
		Debug.Log ("distance=" + distance);
		if (distance < 10)
		{
			grabUAV = true;
		}
	}

	// When mouse button is released
	void OnMouseUp()
	{
		grabUAV = false;
		
		// Snap UAV to the closest vertex
		MISCLib.SnapToGrid (ref UAV);

	}
}
