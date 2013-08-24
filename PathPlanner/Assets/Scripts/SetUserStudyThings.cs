using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class SetUserStudyThings : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Set everything ready for User Study
        SetUserStudyParameters();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetUserStudyParameters()
    {
        // Only do this once
        if (ProjectConstants.pageIndex > 0)
        {
            return;
        }
        string str1 = "Please enter a Group ID (1 to 6) and then click Next to continue.";
        string str2 = "Welcome to the UAV Path Planning user study.\n\n";
        str2 = str2 + "In the following simulated Wilderness Search and Rescue operations, a person is "
        + "reported missing in each scenario, and your help is needed to plan a path for the Unmanned "
        + "Aerial Vehicle (UAV) in order to search for the missing person. The UAV is a hexacopter, "
        + "meaning it can fly to any direction without turning or hover in the air.\n";
        str2 = str2 + "

        ProjectConstants.instructions.Add(str1);
        ProjectConstants.pageIndex++;
    }
}
