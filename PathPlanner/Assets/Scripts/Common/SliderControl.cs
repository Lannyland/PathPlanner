using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Common;

public class SliderControl : MonoBehaviour {

    private UISlider slider;
    private int resolution;

	// Use this for initialization
	void Start () {

        slider = this.gameObject.GetComponent<UISlider>();

	    // Set initial values
        if (this.gameObject.name == "SliderR")
        {
            // Set initial number of steps to 11 or (durationLeft+1) if less than 10 and value to 1
            Debug.Log(this.gameObject.name + " Start() set initial number of steps and value.");
            slider.sliderValue = 1; // This will call SliderR OnSliderChange()
        }
        //if(this.gameObject.name == "SliderD")
        //{
        //    // Set initial number of steps dynamically and value to 1
        //    Debug.Log(this.gameObject.name + " Start() set initial number of steps dynamically and value to 1.");
        //    this.gameObject.GetComponent<UISlider>().numberOfSteps = ProjectConstants.intFlightDuration / 10 + 1;
        //    this.gameObject.GetComponent<UISlider>().sliderValue = 1 / (ProjectConstants.intFlightDuration / 10);
        //}
	}

	// Update is called once per frame
	void Update () {
	
	}

	// When value changes
	public void OnSliderChange(float value)		
	{
		if(slider != null)
		{			
		}
		else
		{
			return;
		}
		
        Debug.Log(this.gameObject.name + " OnSliderChange called.");

		// Not allow slider value to be 0		
        if (value == 0)
        {
            float v = 1.0f / (slider.numberOfSteps - 1);
			slider.sliderValue = v;
			return;
        }
		
		// Deal with different sliders differently
		if (this.gameObject.name == "SliderD")
		{
            // Change display text
            int duration = ComputeDuration(slider);
			ChangeLabelText ("lblDValue", duration.ToString());

			// This section calls PlanPath to draw lines and show vacuum
			if(ProjectConstants.readyToPlanPath)
			{
                Debug.Log("ProjectConstants.readyToPlanPath = " + ProjectConstants.readyToPlanPath + " and let's start path planning");
				Camera.main.GetComponent<PlanPath>().PlanMultiplePaths();
			}
		}
		else if (this.gameObject.name == "SliderR")
		{
            // Sets the max label, value label, number of steps automatically
            SetSliderR(value);
			
    		// Change the duration slider's settings
            UISlider sliderD = GameObject.Find("SliderD").GetComponent<UISlider>();
            ChangeLabelText("lblDMax", ProjectConstants.durationLeft.ToString());
            sliderD.numberOfSteps = Convert.ToInt16(Math.Ceiling(ProjectConstants.durationLeft * 1.0f / resolution) + 1);

			// Move sliderD to lowest possible
            // Only enable possible values that allow the UAV to reach the end point
            if (ProjectConstants.boolUseEndPoint && ProjectConstants.endPointCounter > 0)
            {
                // Find last endpoint and UAV
                GameObject UAV = GameObject.Find("UAV");
                GameObject curEndPoint = GameObject.Find("EndPoint" + ProjectConstants.endPointCounter);
                int duration = ComputeDuration(sliderD);
                while (MISCLib.ManhattanDistance(curEndPoint.transform.position, UAV.transform.position) * 10 > duration * 30)
                {
                    duration += ProjectConstants.resolution;
                }
                if(slider.numberOfSteps != 1)
				{									
					sliderD.sliderValue = Mathf.Clamp01(duration / ProjectConstants.resolution * (1f / (slider.numberOfSteps - 1))); // This calls OnSliderChange() for SliderD.
				}
            }
            else
            {
				sliderD.sliderValue = Mathf.Clamp01(1f / (slider.numberOfSteps - 1)); // This calls OnSliderChange() for SliderD.
//                float newV = 1 / (sliderD.numberOfSteps - 1);
//                float oldV = sliderD.sliderValue;
//                Debug.Log("Setting sliderD value to same. And numberOfSteps = " + sliderD.numberOfSteps);
//                sliderD.sliderValue = newV;
//                if (Math.Abs(oldV - newV)<0.0001)
//                {
//                   sliderD.GetComponent<SliderControl>().OnSliderChange(newV);
//                }
            }
		}				
	}

    // Sets up the max value label, the current selected value label for SliderR.
    private void SetSliderR(float value)
    {
        // Change display text
        int steps = ResolutionSteps();
        slider.numberOfSteps = steps;
        resolution = steps - 1;
        int curResolution = Convert.ToInt16(Math.Round(value * resolution));
        ChangeLabelText("lblRMax", resolution.ToString());
        ChangeLabelText("lblRValue", curResolution.ToString());
        ProjectConstants.resolution = curResolution;
		resolution = curResolution;
    }

    // Compute resolution steps for resolution slider
    private static int ResolutionSteps()
    {
        int steps = 0;
        if (ProjectConstants.durationLeft >= 10)
        {
            steps = 11;
        }
        else
        {
            steps = ProjectConstants.durationLeft + 1;
        }
        return steps;
    }
	
    // Automatically compute the right duration based on sliderD selection.
    private int ComputeDuration(UISlider sliderD)
    {
        float stepValue = 1.0f / (sliderD.numberOfSteps - 1);
        int duration = Convert.ToInt16(sliderD.sliderValue / stepValue) * ProjectConstants.resolution;
        if (duration > ProjectConstants.durationLeft)
        {
            duration = ProjectConstants.durationLeft;
        }
        return duration;
    }
	
    // Method to change given label text to given text.
	private void ChangeLabelText (string labelName, string text)
	{
		UILabel label = GameObject.Find(labelName).GetComponent<UILabel>();
		label.text = text;
	}
	
    // When mouse is clicked on slider
	void OnMouseDown()
	{
		// Tell workThread to stop
		if (this.gameObject.name == "SliderR")
		{
			ProjectConstants.stopPathPlanFactory = true;
		}
	}
	
    // When mouse button is released on slider
	void OnMouseUp()
	{
		// Tell workThread to stop
		if (this.gameObject.name == "SliderR")
		{
			ProjectConstants.stopPathPlanFactory = false;
		}
	}
}
