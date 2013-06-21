using UnityEngine;
using System;
using System.Collections;

public class LoadInputs : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
        // Set input fields defualt values
        UILabel diffFileLoad = GameObject.Find("inputDiffFileLoad").GetComponent<UILabel>();
        diffFileLoad.text = Assets.Scripts.ProjectConstants.strDiffFileLoad;
        UILabel distFileLoad = GameObject.Find("inputDistFileLoad").GetComponent<UILabel>();
        distFileLoad.text = Assets.Scripts.ProjectConstants.strDistFileLoad;
        UILabel TerrainImage = GameObject.Find("inputTerrainImage").GetComponent<UILabel>();
        TerrainImage.text = Assets.Scripts.ProjectConstants.strTerrainImage;
        UICheckbox useDiffMap = GameObject.Find("chkUseDiff").GetComponent<UICheckbox>();
        useDiffMap.isChecked = Assets.Scripts.ProjectConstants.boolUseDiffMap;
        UICheckbox useEndPoint = GameObject.Find("chkUseEnd").GetComponent<UICheckbox>();
        useEndPoint.isChecked = Assets.Scripts.ProjectConstants.boolUseEndPoint;
        UISlider flightDuration = GameObject.Find("Slider").GetComponent<UISlider>();
        float steps = Convert.ToSingle(flightDuration.GetComponent<SliderSetSteps>().steps);
        flightDuration.sliderValue = Assets.Scripts.ProjectConstants.intFlightDuration / (steps - 1);
        flightDuration.GetComponent<SliderNoZero>().OnSliderChange(flightDuration.sliderValue);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
