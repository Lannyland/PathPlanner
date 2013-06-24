using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class StartUpSliding : StartUpChores {

	// Use this for initialization
	protected override void Start () {
        base.Start();
		
		if(!Assets.Scripts.ProjectConstants.boolUseEndPoint)
		{
			GameObject.Find("btnSetEnd").GetComponent<UIButton>().enabled = false;
		}
        // The following code should be moved to scene specific start up chore scripts
        //Camera.main.transform.GetComponent<DrawFreeLine>().enabled = false;
        //UISlider slider = GameObject.Find("Slider").GetComponent<UISlider>();
        //slider.sliderValue = 0.1f;
        //slider.GetComponent<BrushSize>().OnSliderChange(slider.sliderValue);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
