using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;


public class UserStudyLoadNext : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // When button is clicked load a scene
    void OnClick()
    {
        // Save Group ID on first screen
        if (ProjectConstants.pageIndex == 0)
        {
            UIInput uiGroupID = GameObject.Find("txtGroupID").GetComponent<UIInput>();
            Debug.Log("uiGroupID.text = " + uiGroupID.text);
            ProjectConstants.GroupID = Convert.ToInt16(uiGroupID.text);
        }

        // Set what scene to load (or quit application) when Next button is clicked
        string nextScene = ProjectConstants.nextScene[ProjectConstants.pageIndex];
        if (nextScene == "")
        {
            Application.Quit();
        }
        else
        {
            Application.LoadLevel(nextScene);
        }

        // Increase page index counter
        ProjectConstants.pageIndex++;

        // Set dist and diff maps before related scenes are loaded
        List<int> indexes = new List<int>();
        // These are scene indexes of real path planning scenes
        indexes.Add(3);
        indexes.Add(5);
        indexes.Add(7);
        indexes.Add(9);
        indexes.Add(11);
        indexes.Add(14);
        indexes.Add(17);
        indexes.Add(20);
        indexes.Add(23);
        indexes.Add(26);
        if (indexes.Contains(ProjectConstants.pageIndex))
        {
            ProjectConstants.strDistFileLoad = ProjectConstants.distMaps[ProjectConstants.pageIndex];
            ProjectConstants.strDiffFileLoad = ProjectConstants.diffMaps[ProjectConstants.pageIndex];
            if (ProjectConstants.diffMaps[ProjectConstants.pageIndex] == "")
            {
                ProjectConstants.boolUseDiffMap = false;
            }
            else
            {
                ProjectConstants.boolUseDiffMap = true;
            }

        }
    }

}
