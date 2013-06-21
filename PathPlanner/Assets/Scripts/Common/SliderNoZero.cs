using UnityEngine;
using System;
using System.Collections;

public class SliderNoZero : MonoBehaviour {

    public UILabel label;
    public int max = 60;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // When value changes
    public void OnSliderChange(float value)
    {
        // Not allow brush size to be 0
        UISlider slide = this.gameObject.GetComponent<UISlider>();
        if (slide.sliderValue == 0)
        {
            float firstValue = 1.0f / (slide.numberOfSteps - 1);
            slide.sliderValue = firstValue;
            value = firstValue;
        }

        // Change display text
        label.text = (value * max).ToString();
    }
}
