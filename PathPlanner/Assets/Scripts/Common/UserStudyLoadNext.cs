using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;

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
		
		// Save things to log file
		List<int> needToSave = new List<int>();
		needToSave.Add (12);
		needToSave.Add (14);
		needToSave.Add (17);
		needToSave.Add (20);
		needToSave.Add (23);
		needToSave.Add (26);
		if(needToSave.Contains(ProjectConstants.pageIndex))
		{
            SaveEverythingToLog();
        }
		else
		{
			ResetLogVariables();
		}
        // Save comparison
        if (needToSave.Contains(ProjectConstants.pageIndex - 1) && ProjectConstants.pageIndex != 13)
        {
            if (GameObject.Find("chkLess").GetComponent<UICheckbox>().isChecked)
            {
                ProjectConstants.comparisons.Add("Less");
            }
            else if (GameObject.Find("chkSame").GetComponent<UICheckbox>().isChecked)
            {
                ProjectConstants.comparisons.Add("Same");
            }
            else if (GameObject.Find("chkMore").GetComponent<UICheckbox>().isChecked)
            {
                ProjectConstants.comparisons.Add("More");
            }
        }
        if (ProjectConstants.pageIndex == 27)
        {
            MISCLib.SaveToLogFile("comparisons");
            MISCLib.SaveToLogFile(ProjectConstants.comparisons);
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
        indexes.Add(12);
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
	
	private void SaveEverythingToLog()
	{
		// If user never clicked Fly Path, or if user clicked Fly Path and then clicked Start Over
		if(!ProjectConstants.boolFlyPath)
		{
			// Debug.Log ("Inside SaveEverythingToLog method.");
			// Debug.Log("boolFlyPath=" + ProjectConstants.boolFlyPath);			
			ProjectConstants.timeLeft = GameObject.Find("lblTime").GetComponent<UILabel>().text;
			// Debug.Log ("timeleft=" + ProjectConstants.timeLeft);
			ProjectConstants.score = GameObject.Find("lblScore").GetComponent<UILabel>().text;
			// Debug.Log("score=" + ProjectConstants.score);
		}
		// If user clicked Fly Path and not Start Over, then use stored numbers directly		
		string [] temp = ProjectConstants.instructions[ProjectConstants.pageIndex-1].Split('\n');
		string name = temp[0];
		ProjectConstants.logs.Add("name|"+name);
		ProjectConstants.logs.Add("timeleft|"+ProjectConstants.timeLeft);
		ProjectConstants.logs.Add("score|"+ProjectConstants.score);
		ProjectConstants.logs.Add("mouseclicks|"+ProjectConstants.mouseclicks);
		MISCLib.SaveToLogFile(ProjectConstants.logs);
		// Store path
		string path = "";
		int pathLength = 0;
		if(name == "Scenario 1 Manual Flight" || name == "Scenario 2 Manual Flight")
		{
            FlyManual fm = GameObject.Find("UAV").GetComponent<FlyManual>();
            for (int i = 0; i < fm.path.Length; i++)
            {
                if (i <= ProjectConstants.curWayPoint)
                {
                    path += fm.path[i].x + " " + fm.path[i].y + ",";
                    pathLength++;
                }
                else
                {
                    break;
                }
            }
		}		
		else if (name =="Scenario 1 Pattern Flight" || name =="Scenario 2 Pattern Flight")
		{
            if (ProjectConstants.boolFlyPath)
            {
                pathLength = 1801;
                FlyPathPattern fpp = GameObject.Find("UAV").GetComponent<FlyPathPattern>();
                for (int i = 0; i < fpp.path.Count; i++)
                {
                    path += fpp.path[i].x + " " + fpp.path[i].y + ",";
                }
                pathLength = fpp.path.Count;
            }
            else
            {
                FlyPattern fp = GameObject.Find("UAV").GetComponent<FlyPattern>();
                for (int i = 0; i < fp.path.Count; i++)
                {
                    path += fp.path[i].x + " " + fp.path[i].y + ",";
                }
                pathLength = fp.path.Count;
            }
		}
		else if (name == "Scenario 1 Sliding Autonomy Flight" || name == "Scenario 2 Sliding Autonomy Flight")
		{
            if (ProjectConstants.boolFlyPath)
            {
                pathLength = 1801;
                FlyPath fpp = GameObject.Find("UAV").GetComponent<FlyPath>();
                for (int i = 0; i < fpp.path.Length; i++)
                {
                    path += fpp.path[i].x + " " + fpp.path[i].y + ",";
                }
                pathLength = fpp.path.Length;
            }
            else
            {
                Vector2[] SAPath = new Vector2[ProjectConstants.intFlightDuration * 30 + 1];
                int index = 0;
                for (int i = 0; i < ProjectConstants.AllPathSegments.Count; i++)
                {
                    Array.Copy(ProjectConstants.AllPathSegments[i], 0, SAPath, index, ProjectConstants.AllPathSegments[i].Length);
                    index += ProjectConstants.AllPathSegments[i].Length;
                    index--;
                }
                for (int i = 0; i < SAPath.Length; i++)
                {
                    if (i <= index + 1)
                    {
                        path += SAPath[i].x + " " + SAPath[i].y + ",";
                        pathLength++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
		}
		if(pathLength < 1801)
		{
			MISCLib.SaveToLogFile("pathcomplete|No");
		}
		else
		{
			MISCLib.SaveToLogFile("pathcomplete|Yes");			
		}
		MISCLib.SaveToLogFile("path|"+path);		
		MISCLib.SaveToLogFile(ProjectConstants.replies);

		ResetLogVariables();
	}
	
	private void ResetLogVariables()
	{
		// Debug.Log("Reseting save log variables...");
		ProjectConstants.logs.Clear();
		ProjectConstants.timeLeft = "";
		ProjectConstants.score = "";
		ProjectConstants.mouseclicks = 0;
		ProjectConstants.boolFlyPath = false;
		ProjectConstants.curWayPoint = 0;
		ProjectConstants.replies.Clear();

        // Also reset sliding autonomy stuff
		ProjectConstants.navMode = 1;		
		ProjectConstants.resolution = 10;	
		ProjectConstants.durationLeft = 60;
        ProjectConstants.duration = 10;	

        ProjectConstants.readyToPlanPath = false;		
		ProjectConstants.boolUseEndPoint = false;     	
        ProjectConstants.boolPlenty = false;          	
        ProjectConstants.lastPathApproved = true;		
		ProjectConstants.endPointCounter = 0;			
        ProjectConstants.originalStart = new Vector2(); 
        ProjectConstants.curStart = new Vector2();      
        ProjectConstants.curEnd = new Vector2();        
		ProjectConstants.stopPathPlanFactory = false;	
        ProjectConstants.AllPathSegments = new List<Vector2[]>();
	}
}
