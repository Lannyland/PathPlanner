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
        if (ProjectConstants.pageIndex == 0)
        {
            // Save Group ID
            UIInput uiGroupID = GameObject.Find("txtGroupID").GetComponent<UIInput>();
            ProjectConstants.GroupID = Convert.ToInt16(uiGroupID.text);
        }

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
    }

}
