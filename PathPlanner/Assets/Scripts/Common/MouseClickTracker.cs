using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class MouseClickTracker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            ProjectConstants.mouseclicks++;
            // Debug.Log("Mouse click detected.");
        }
	}
}
