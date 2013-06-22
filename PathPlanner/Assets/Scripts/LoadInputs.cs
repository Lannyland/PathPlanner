using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class LoadInputs : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
        // Set input fields defualt values
        UILabel diffFileLoad = GameObject.Find("inputDiffFileLoad").GetComponent<UILabel>();
        diffFileLoad.text = ProjectConstants.strDiffFileLoad;
        UILabel distFileLoad = GameObject.Find("inputDistFileLoad").GetComponent<UILabel>();
        distFileLoad.text = ProjectConstants.strDistFileLoad;
        UILabel TerrainImage = GameObject.Find("inputTerrainImage").GetComponent<UILabel>();
        TerrainImage.text = ProjectConstants.strTerrainImage;
        UICheckbox useDiffMap = GameObject.Find("chkUseDiff").GetComponent<UICheckbox>();
        useDiffMap.isChecked = ProjectConstants.boolUseDiffMap;
        UICheckbox useEndPoint = GameObject.Find("chkUseEnd").GetComponent<UICheckbox>();
        useEndPoint.isChecked = ProjectConstants.boolUseEndPoint;
        UISlider flightDuration = GameObject.Find("Slider").GetComponent<UISlider>();
        float steps = Convert.ToSingle(flightDuration.GetComponent<SliderSetSteps>().steps);
        flightDuration.sliderValue = ProjectConstants.intFlightDuration / (steps - 1);
        flightDuration.GetComponent<SliderNoZero>().OnSliderChange(flightDuration.sliderValue);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
