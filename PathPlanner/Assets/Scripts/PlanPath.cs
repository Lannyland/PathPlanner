using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using TCPIPTest;

public class PlanPath : MonoBehaviour {

    public ArrayList arrPaths = new ArrayList();

	// Use this for initialization
	void Start () {
	    // Fill ArrayList with empty things
        for (int i = 0; i < ProjectConstants.durationLeft; i++)
        {
            arrPaths.Add(null);
        }
	}

    // Update is called once per frame
    void Update()
    {
	
	}

    // The most important part of the tool: actually plan paths
    public void PlanMultiplePaths()
    {
        // Set things up first
        int resolution = Convert.ToInt16(GameObject.Find("lblRValue").GetComponent<UILabel>().text);
        int duration = Convert.ToInt16(GameObject.Find("lblDValue").GetComponent<UILabel>().text);

        // First plan the selected duration path
        // (each time step is 2 seconds, so divide by 60 and times 2)
        if (arrPaths[duration - 1] != null) 
        {
        }
        else
        {
            Debug.Log("Doing current path planning");
            NetworkCall call = new NetworkCall(
                ProjectConstants.mDistMapCurStepWorking,
                ProjectConstants.mDiffMap,
                ProjectConstants.curStart,
                ProjectConstants.curEnd,
                ProjectConstants.boolUseDiffMap,
                ProjectConstants.boolUseEndPoint,
                ProjectConstants.boolPlenty,
                duration * 30);
            arrPaths[duration - 1] = call.path;
            Debug.Log("Path returned length = " + call.path.Length);
        }


        // While the user is not doing anything, just keep planning
        for (int i = resolution; i < ProjectConstants.durationLeft; i += resolution)
        {
            Debug.Log("Doing additional path planning");
            // If a path already exists, then no need to plan again
            if (arrPaths[i - 1] != null)
            { 
            }
            else
            {
                NetworkCall call = new NetworkCall(
                    ProjectConstants.mDistMapCurStepWorking,
                    ProjectConstants.mDiffMap,
                    ProjectConstants.curStart,
                    ProjectConstants.curEnd,
                    ProjectConstants.boolUseDiffMap,
                    ProjectConstants.boolUseEndPoint,
                    ProjectConstants.boolPlenty,
                    i * 30);
                arrPaths[i - 1] = call.path;
                Debug.Log("Path returned length = " + call.path.Length);
            }
        }
    }
}
