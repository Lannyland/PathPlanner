using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class StartUpSliding : StartUpChores {

	// Use this for initialization
	protected override void Start () {
        base.Start();
		// No end point at all, so disable that button.
		if(!Assets.Scripts.ProjectConstants.boolAnyEndPoint)
		{
			GameObject.Find("btnSetEnd").GetComponent<UIButton>().enabled = false;
		}

		// Only allow fly path when path is completed.
		UIButton fly = GameObject.Find("btnFly").GetComponent<UIButton>();
		fly.GetComponent<FlyPath>().enabled = false;		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
