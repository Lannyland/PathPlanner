using UnityEngine;
using System.Collections;

public class SliderSetSteps : MonoBehaviour {
    public int steps = 61;

	// Use this for initialization
	void Start () {
        UISlider slide = this.gameObject.GetComponent<UISlider>();
        slide.numberOfSteps = steps;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
