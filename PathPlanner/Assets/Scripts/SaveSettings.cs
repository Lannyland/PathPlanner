using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class SaveSettings : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Save settings and return to Menu scene
    void OnClick()
    {
        // Save settings
        UILabel diffFileLoad = GameObject.Find("inputDiffFileLoad").GetComponent<UILabel>();
        ProjectConstants.strDiffFileLoad = diffFileLoad.text;
        UILabel distFileLoad = GameObject.Find("inputDistFileLoad").GetComponent<UILabel>();
        ProjectConstants.strDistFileLoad = distFileLoad.text;
        UILabel TerrainImage = GameObject.Find("inputTerrainImage").GetComponent<UILabel>();
        ProjectConstants.strTerrainImage = TerrainImage.text;
        UICheckbox useDiffMap = GameObject.Find("chkUseDiff").GetComponent<UICheckbox>();
        ProjectConstants.boolUseDiffMap = useDiffMap.isChecked;
        UICheckbox useEndPoint = GameObject.Find("chkUseEnd").GetComponent<UICheckbox>();
        ProjectConstants.boolUseEndPoint = useEndPoint.isChecked;
        UISlider flightDuration = GameObject.Find("Slider").GetComponent<UISlider>();
        int steps = flightDuration.GetComponent<SliderSetSteps>().steps;
        ProjectConstants.intFlightDuration = Convert.ToInt16(flightDuration.sliderValue) * (steps - 1);
        //Debug.Log("strDiffFileLoad = " + ProjectConstants.strDiffFileLoad);
        //Debug.Log("strDistFileLoad = " + ProjectConstants.strDistFileLoad);
        //Debug.Log("strTerrainImage = " + ProjectConstants.strTerrainImage);
        //Debug.Log("boolUseDiffMap = " + ProjectConstants.boolUseDiffMap);
        //Debug.Log("boolUseEndPoint = " + ProjectConstants.boolUseEndPoint);
        //Debug.Log("intFlightDuration = " + ProjectConstants.intFlightDuration);

        // Load Menu scene
        Application.LoadLevel("Menu");
    }
}
