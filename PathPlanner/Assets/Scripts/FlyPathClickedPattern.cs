using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;
using rtwmatrix;

public class FlyPathClickedPattern : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnClick()
	{
		// Save things for user study
		ProjectConstants.timeLeft = GameObject.Find("lblTime").GetComponent<UILabel>().text;
		// Debug.Log ("timeleft=" + ProjectConstants.timeLeft);		
		ProjectConstants.score = GameObject.Find("lblScore").GetComponent<UILabel>().text;
		// Debug.Log("score=" + ProjectConstants.score);

		GameObject.Find("btnStartOver").GetComponent<UIButton>().GetComponent<StartOverPattern>().OnClick();
        GameObject.Find("btnFly").GetComponent<UIButton>().isEnabled = true;
        GameObject.Find("btnStart").GetComponent<UIButton>().isEnabled = false;

		// Save things for user study
		ProjectConstants.boolFlyPath = true;
		// Debug.Log("boolFlyPath=" + ProjectConstants.boolFlyPath);


		//// Fly plane following path and then vacuum up as the plane goes and also up the score
        GameObject.Find("UAV").GetComponent<FlyPathPattern>().path = GameObject.Find("UAV").GetComponent<FlyPattern>().path;
        GameObject.Find("UAV").transform.position = new Vector3(ProjectConstants.originalStart.x, 4.0f, ProjectConstants.originalStart.y);
        Debug.Log("originalStart = " + ProjectConstants.originalStart);
        Debug.Log("UAVPos after fly path is clicked = " + GameObject.Find("UAV").transform.position);
        GameObject.Find("UAV").GetComponent<FlyPathPattern>().fly = true;
    }
}
