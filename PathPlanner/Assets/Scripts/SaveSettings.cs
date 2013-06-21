using UnityEngine;
using System;
using System.Collections;

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
        Assets.Scripts.ProjectConstants.strDiffFileLoad = diffFileLoad.text;
        UILabel distFileLoad = GameObject.Find("inputDistFileLoad").GetComponent<UILabel>();
        Assets.Scripts.ProjectConstants.strDistFileLoad = distFileLoad.text;
        UILabel TerrainImage = GameObject.Find("inputTerrainImage").GetComponent<UILabel>();
        Assets.Scripts.ProjectConstants.strTerrainImage = TerrainImage.text;
        UICheckbox useDiffMap = GameObject.Find("chkUseDiff").GetComponent<UICheckbox>();
        Assets.Scripts.ProjectConstants.boolUseDiffMap = useDiffMap.isChecked;
        UICheckbox useEndPoint = GameObject.Find("chkUseEnd").GetComponent<UICheckbox>();
        Assets.Scripts.ProjectConstants.boolUseEndPoint = useEndPoint.isChecked;
        UISlider flightDuration = GameObject.Find("Slider").GetComponent<UISlider>();
        int steps = flightDuration.GetComponent<SliderSetSteps>().steps;
        Assets.Scripts.ProjectConstants.intFlightDuration = Convert.ToInt16(flightDuration.sliderValue) * (steps - 1);

        //Debug.Log("strDiffFileLoad = " + Assets.Scripts.ProjectConstants.strDiffFileLoad);
        //Debug.Log("strDistFileLoad = " + Assets.Scripts.ProjectConstants.strDistFileLoad);
        //Debug.Log("strTerrainImage = " + Assets.Scripts.ProjectConstants.strTerrainImage);
        //Debug.Log("boolUseDiffMap = " + Assets.Scripts.ProjectConstants.boolUseDiffMap);
        //Debug.Log("boolUseEndPoint = " + Assets.Scripts.ProjectConstants.boolUseEndPoint);
        //Debug.Log("intFlightDuration = " + Assets.Scripts.ProjectConstants.intFlightDuration);

        // Load Menu scene
        Application.LoadLevel("Menu");
    }
}
