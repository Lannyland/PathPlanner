using UnityEngine;
using System.Collections;

public class PatternModes : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		if (this.gameObject.name == "btnLawnmower")
        {
            GameObject.Find("UAV").GetComponent<FlyPattern>().flyMode = FlyPattern.PatternMode.Lawnmower;
		}
		if (this.gameObject.name == "btnSpiral")
        {
            GameObject.Find("UAV").GetComponent<FlyPattern>().flyMode = FlyPattern.PatternMode.Spiral;
		}
		if (this.gameObject.name == "btnLine")
        {
            GameObject.Find("UAV").GetComponent<FlyPattern>().flyMode = FlyPattern.PatternMode.Line;
		}		
	}
}
