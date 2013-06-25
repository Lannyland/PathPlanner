using UnityEngine;
using System.Collections;

public class ShowHideDiffMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // When clicked
    void OnClick()
    {
        // First change label back and forth
		// Then change material on plane to show or hide terrain image		
        UILabel label = GameObject.Find("lblShowDifficulty").GetComponent<UILabel>();
        MeshFilter mf = GameObject.Find("Plane").GetComponent<MeshFilter>();
        MeshFilter mf2 = GameObject.Find("PlaneDiff").GetComponent<MeshFilter>();	

        if (label.text == "Show Difficulty")
        {
            label.text = "Hide Difficulty";
            // Remember camera location and rotation
        }
        else
        {
            label.text = "Show Difficulty";
        }
        mf.renderer.enabled = !mf.renderer.enabled;
        mf2.renderer.enabled = !mf2.renderer.enabled;

    }
}
