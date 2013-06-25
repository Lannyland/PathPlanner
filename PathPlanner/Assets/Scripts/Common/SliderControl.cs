using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;

public class SliderControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// When value changes
	public void OnSliderChange(float value)
	{
		// Not allow slider value to be 0		
        UISlider slider = this.gameObject.GetComponent<UISlider>();
        if (value == 0)
        {
            float v = 1.0f / (slider.numberOfSteps + 1);
			slider.sliderValue = v;
			return;
        }
		
		// Deal with different sliders differently
		if (this.gameObject.name == "SliderD")
		{
			// Only enable possible values that allow the UAV to reach the end point

            
            // Change display text
			int number = Convert.ToInt16(Math.Round (value / (1.0f / (slider.numberOfSteps-1))));
			int v = ChangeLabelText ("lblDValue", number, ProjectConstants.resolution);
			if (number * ProjectConstants.resolution > ProjectConstants.durationLeft)
			{
				v = ChangeLabelText ("lblDValue", ProjectConstants.durationLeft, 1);
			}			

			// Remember current duration
			Assets.Scripts.ProjectConstants.duration = v;

			// This section calls PlanPath to draw lines and show vacuum
			if(ProjectConstants.readyToPlanPath)
			{
				Camera.main.GetComponent<PlanPath>().PlanMultiplePaths();
			}
		}
		else if (this.gameObject.name == "SliderR")
		{
			// Change display text
			int v = ChangeLabelText ("lblRValue", value, 10);
			
			// Remember current resolution
			Assets.Scripts.ProjectConstants.resolution = v;
			
			// Change the duration slider's settings
			UISlider sliderD = GameObject.Find("SliderD").GetComponent<UISlider>();
			sliderD.numberOfSteps = Convert.ToInt16(Math.Ceiling(ProjectConstants.durationLeft * 1.0f / v)+1);

			// Move slider to 1
			sliderD.sliderValue = 1.0f / (sliderD.numberOfSteps - 1);
			sliderD.GetComponent<SliderControl>().OnSliderChange(sliderD.sliderValue);
		}				
	}
	
	private int ChangeLabelText (string labelName, float value, int scale)
	{
		UISlider sliderD = this.gameObject.GetComponent<UISlider>();		
		UILabel label = GameObject.Find(labelName).GetComponent<UILabel>();
		int v = Convert.ToInt16(Math.Round(value*scale));
		label.text = v.ToString();
		return v;
	}
	
	void OnMouseDown()
	{
		// Tell workThread to stop
		if (this.gameObject.name == "SliderR")
		{
			ProjectConstants.stopPathPlanFactory = true;
		}
	}
	
	void OnMouseUp()
	{
		// Tell workThread to stop
		if (this.gameObject.name == "SliderR")
		{
			ProjectConstants.stopPathPlanFactory = false;
		}
	}
}
