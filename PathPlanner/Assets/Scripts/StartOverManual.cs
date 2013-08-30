using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Common;
using rtwmatrix;

public class StartOverManual : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// When button is clicked
	void OnClick()
	{
        GameObject UAV = GameObject.Find("UAV");
        Transform transform = UAV.transform;
        FlyManual fm = UAV.GetComponent<FlyManual>();

		// Move UAV back to center and make it movable
        transform.position = new Vector3(0f, 4f, 0f);
        transform.rotation = Quaternion.identity;
        UAV.GetComponent<MoveUFO>().movable = ProjectConstants.UAVMovable;
				
		// Reload distMap to mesh
		RtwMatrix distMapIn = ProjectConstants.mOriginalDistMap.Clone();
		Mesh mesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;
        MISCLib.ApplyMatrixToMesh(distMapIn, ref mesh, true);
        fm.distVertices = mesh.vertices;
        fm.distColors = mesh.colors;

        // Reset timer
        fm.timer = ProjectConstants.intFlightDuration * 60;
		fm.curWaypoint = 1;

		// Set everything back to default values
		ProjectConstants.mDistMapCurStepUndo = ProjectConstants.mOriginalDistMap.Clone();
        Vector3[] copy = new Vector3[mesh.vertices.Length];
        Array.Copy(mesh.vertices, copy, copy.Length);
        ProjectConstants.curVertices = copy;
		
		// Enable those buttons
        GameObject.Find("btnStart").GetComponent<UIButton>().isEnabled = true;
        fm.fly = false;

        // reset score
        GameObject.Find("ControlCenter").GetComponent<IncreasingScoreEffect>().curScore = 0f;
        GameObject.Find("ControlCenter").GetComponent<IncreasingScoreEffect>().initialScore = 0f;
        GameObject.Find("lblScore").GetComponent<UILabel>().text = "0";

        // Reset pause button
        UILabel label = GameObject.Find("lblStartPause").GetComponent<UILabel>();
        if (label.text == "Pause")
        {
            label.text = "Start";
        }

        // Reset timer label
        int second = fm.timer % 60;
        int minute = fm.timer / 60;
        GameObject.Find("lblFlightTime").GetComponent<UILabel>().text = minute.ToString() + ":" + second.ToString("00");
	}
}
