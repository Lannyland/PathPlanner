using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class SetUserStudyThings : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Debug.Log("pageIndex = " + ProjectConstants.pageIndex);
        // Set everything ready for User Study
        SetUserStudyParameters();
        UILabel label = GameObject.Find("GUIText").GetComponent<UILabel>();
        label.text = ProjectConstants.instructions[ProjectConstants.pageIndex];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetUserStudyParameters()
    {
		string strAppDir = Application.dataPath + @"\..\" + "UserStudyData";
		Debug.Log("strAppDir = " + strAppDir);
        UIInput uiGroupID = GameObject.Find("txtGroupID").GetComponent<UIInput>();

        // Do this once for screen 1 and once for screen 2
        if (ProjectConstants.pageIndex > 1)
        {
            // Hide Group ID field
            uiGroupID.transform.position = new Vector3(-10000f, -10000f, -10000f);
            return;
        }
        else if(ProjectConstants.pageIndex == 1)
        {
            // Hide Group ID field
            uiGroupID.transform.position = new Vector3(-10000f, -10000f, -10000f);

            #region screen pairs that will be rearranged based on Group ID selection

            // The following two screens are for real test manual flight
            List<string> instM = new List<string>();
            List<int> durM = new List<int>();
            List<string> nextM = new List<string>();
            List<string> diffM = new List<string>();
            List<string> distM = new List<string>();
			List<string> chatM = new List<string>();

            string strM1 = "Screnario 1 Manual Flight";
            instM.Add(strM1);
            durM.Add(0);
            nextM.Add("UserStudyManualFlight");
            diffM.Add("");
            distM.Add("");
			chatM.Add("");
			
            string strM2 = "";
            instM.Add(strM2);
            durM.Add(5);
            nextM.Add("UserStudy");
            diffM.Add("");
            distM.Add(strAppDir + @"\TestDistMap1.csv");
			chatM.Add(strAppDir + @"\ChatBoxManual1.txt");

            // The following two screens are for real test pattern flight
            List<string> instP = new List<string>();
            List<int> durP = new List<int>();
            List<string> nextP = new List<string>();
            List<string> diffP = new List<string>();
            List<string> distP = new List<string>();
			List<string> chatP = new List<string>();

            string strP1 = "Screnario 1 Pattern Flight";
            instP.Add(strP1);
            durP.Add(0);
            nextP.Add("UserStudyPatternFlight");
            diffP.Add("");
            distP.Add("");
			chatP.Add("");

            string strP2 = "";
            instP.Add(strP2);
            durP.Add(5);
            nextP.Add("UserStudy");
            diffP.Add("");
            distP.Add(strAppDir + @"\TestDistMap1.csv");
			chatP.Add(strAppDir + @"\ChatBoxPattern1.txt");

            // The following two screens are for real test sliding autonomy flight
            List<string> instS = new List<string>();
            List<int> durS = new List<int>();
            List<string> nextS = new List<string>();
            List<string> diffS = new List<string>();
            List<string> distS = new List<string>();
			List<string> chatS = new List<string>();

            string strS1 = "Screnario 1 Sliding Autonomy Flight";
            instS.Add(strS1);
            durS.Add(0);
            nextS.Add("UserStudySlidingAutonomy");
            diffS.Add("");
            distS.Add("");
			chatS.Add("");		

            string strS2 = "";
            instS.Add(strS2);
            durS.Add(5);
            nextS.Add("UserStudy");
            diffS.Add("");
            distS.Add(strAppDir + @"\TestDistMap1.csv");
			chatS.Add(strAppDir + @"\ChatBoxSliding1.txt");

            // The following two screens are for real test manual flight with difficulty map
            List<string> instMD = new List<string>();
            List<int> durMD = new List<int>();
            List<string> nextMD = new List<string>();
            List<string> diffMD = new List<string>();
            List<string> distMD = new List<string>();
			List<string> chatMD = new List<string>();

            string strMD1 = "Screnario 2 Manual Flight";
            instMD.Add(strMD1);
            durMD.Add(0);
            nextMD.Add("UserStudyManualFlight");
            diffMD.Add("");
            distMD.Add("");
			chatMD.Add("");
			
            string strMD2 = "";
            instMD.Add(strMD2);
            durMD.Add(5);
            nextMD.Add("UserStudy");
            diffMD.Add(strAppDir + @"\TestDiffMap2.csv");
            distMD.Add(strAppDir + @"\TestDistMap2.csv");
			chatMD.Add(strAppDir + @"\ChatBoxManual2.txt");

            // The following two screens are for real test pattern flight with difficulty map
            List<string> instPD = new List<string>();
            List<int> durPD = new List<int>();
            List<string> nextPD = new List<string>();
            List<string> diffPD = new List<string>();
            List<string> distPD = new List<string>();
			List<string> chatPD = new List<string>();

            string strPD1 = "Screnario 2 Pattern Flight";
            instPD.Add(strPD1);
            durPD.Add(0);
            nextPD.Add("UserStudyPatternFlight");
            diffPD.Add("");
            distPD.Add("");
			chatPD.Add("");

            string strPD2 = "";
            instPD.Add(strPD2);
            durPD.Add(5);
            nextPD.Add("UserStudy");
            diffPD.Add(strAppDir + @"\TestDiffMap2.csv");
            distPD.Add(strAppDir + @"\TestDistMap2.csv");
			chatPD.Add(strAppDir + @"\ChatBoxPattern2.txt");

            // The following two screens are for real test sliding autonomy flight with difficulty map
            List<string> instSD = new List<string>();
            List<int> durSD = new List<int>();
            List<string> nextSD = new List<string>();
            List<string> diffSD = new List<string>();
            List<string> distSD = new List<string>();
			List<string> chatSD = new List<string>();

            string strSD1 = "Screnario 2 Sliding Autonomy Flight";
            instSD.Add(strSD1);
            durSD.Add(0);
            nextSD.Add("UserStudySlidingAutonomy");
            diffSD.Add("");
            distSD.Add("");
			chatSD.Add("");		

            string strSD2 = "";
            instSD.Add(strSD2);
            durSD.Add(5);
            nextSD.Add("UserStudy");
            diffSD.Add(strAppDir + @"\TestDiffMap2.csv");
            distSD.Add(strAppDir + @"\TestDistMap2.csv");
			chatSD.Add(strAppDir + @"\ChatBoxSliding2.txt");

            #endregion

            switch (ProjectConstants.GroupID)
            {
                case 1:
                    // Manual No Diff
                    ProjectConstants.instructions.AddRange(instM);
                    ProjectConstants.durations.AddRange(durM);
                    ProjectConstants.nextScene.AddRange(nextM);
                    ProjectConstants.diffMaps.AddRange(diffM);
                    ProjectConstants.distMaps.AddRange(distM);
					ProjectConstants.chatFiles.AddRange(chatM);

                    // Pattern No Diff
                    ProjectConstants.instructions.AddRange(instP);
                    ProjectConstants.durations.AddRange(durP);
                    ProjectConstants.nextScene.AddRange(nextP);
                    ProjectConstants.diffMaps.AddRange(diffP);
                    ProjectConstants.distMaps.AddRange(distP);
					ProjectConstants.chatFiles.AddRange(chatP);

                    // Sliding Autonomy No Diff
                    ProjectConstants.instructions.AddRange(instS);
                    ProjectConstants.durations.AddRange(durS);
                    ProjectConstants.nextScene.AddRange(nextS);
                    ProjectConstants.diffMaps.AddRange(diffS);
                    ProjectConstants.distMaps.AddRange(distS);
					ProjectConstants.chatFiles.AddRange(chatS);

                    // Manual with Diff
                    ProjectConstants.instructions.AddRange(instMD);
                    ProjectConstants.durations.AddRange(durMD);
                    ProjectConstants.nextScene.AddRange(nextMD);
                    ProjectConstants.diffMaps.AddRange(diffMD);
                    ProjectConstants.distMaps.AddRange(distMD);
					ProjectConstants.chatFiles.AddRange(chatMD);

                    // Pattern with Diff
                    ProjectConstants.instructions.AddRange(instPD);
                    ProjectConstants.durations.AddRange(durPD);
                    ProjectConstants.nextScene.AddRange(nextPD);
                    ProjectConstants.diffMaps.AddRange(diffPD);
                    ProjectConstants.distMaps.AddRange(distPD);
					ProjectConstants.chatFiles.AddRange(chatPD);

                    // Sliding Autonomy with Diff
                    ProjectConstants.instructions.AddRange(instSD);
                    ProjectConstants.durations.AddRange(durSD);
                    ProjectConstants.nextScene.AddRange(nextSD);
                    ProjectConstants.diffMaps.AddRange(diffSD);
                    ProjectConstants.distMaps.AddRange(distSD);
					ProjectConstants.chatFiles.AddRange(chatSD);

                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                default:

                    break;
            }

            string str17 = "Thank you for doing the user study. Now please take the survey.";
            ProjectConstants.instructions.Add(str17);
            ProjectConstants.durations.Add(0);
            ProjectConstants.nextScene.Add("");
            ProjectConstants.diffMaps.Add("");
            ProjectConstants.distMaps.Add("");
			ProjectConstants.chatFiles.Add("");

            return;
        }

        // First screen set Group ID
        string str1 = "Please enter a Group ID (1 to 6) and then click Next to continue.";
        ProjectConstants.instructions.Add(str1);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");

        // Second screen welcomes user and explains
        // -- Purpose of user study
        // -- Two maps
        // -- HOw UAV vacuums and the goal of the tasks
        // -- Secondary task
        string str2 = "Welcome to the UAV Path Planning user study.\n\n";
        str2 = str2 + "In the following simulated Wilderness Search and Rescue operations, a person is "
        + "reported missing in each scenario, and your help is needed to plan a path for the Unmanned "
        + "Aerial Vehicle (UAV) in order to search for the missing person. The UAV is a hexacopter, "
        + "meaning it can fly to any direction without turning or hover in the air.\n";
        str2 = str2 + "";
        ProjectConstants.instructions.Add(str2);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");
        
        // Third screen training manual flight insturctions
        string str3 = "Manual Flight Training";
        ProjectConstants.instructions.Add(str3);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudyManualFlight");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");

        // Fourth screen training manual flight
        string str4 = "";
        ProjectConstants.instructions.Add(str4);
        ProjectConstants.durations.Add(3);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add(strAppDir + @"\TrainingDistMap0.csv");
		ProjectConstants.chatFiles.Add(strAppDir + @"\ChatBoxTraining1.txt");

        // Fifth screen training pattern flight insturctions
        string str5 = "Pattern Flight Training";
        ProjectConstants.instructions.Add(str5);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudyPatternFlight");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");

        // Sixth screen training pattern flight
        string str6 = "";
        ProjectConstants.instructions.Add(str6);
        ProjectConstants.durations.Add(3);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add(strAppDir + @"\TrainingDistMap0.csv");
		ProjectConstants.chatFiles.Add(strAppDir + @"\ChatBoxTraining2.txt");

        // Seventh screen training sliding autonomy flight insturctions
        string str7 = "Sliding Autonomy Flight Training";
        ProjectConstants.instructions.Add(str7);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudySlidingAutonomy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");
		
        // Eighth screen training sliding autonomy flight
        string str8 = "";
        ProjectConstants.instructions.Add(str8);
        ProjectConstants.durations.Add(3);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add(strAppDir + @"\TrainingDistMap0.csv");
		ProjectConstants.chatFiles.Add(strAppDir + @"\ChatBoxTraining3.txt");

        // Nineth screen training pattern flight with difficulty map insturctions
        string str9 = "Pattern Flight With Difficulty Map Training";
        ProjectConstants.instructions.Add(str9);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudyPatternFlight");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");

        // Tenth screen training pattern flight with difficulty map
        string str10 = "";
        ProjectConstants.instructions.Add(str10);
        ProjectConstants.durations.Add(3);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add(strAppDir + @"\TrainingDiffMap0.csv");
        ProjectConstants.distMaps.Add(strAppDir + @"\TrainingDistMap0.csv");
		ProjectConstants.chatFiles.Add(strAppDir + @"\ChatBoxTraining4.txt");
    }
}
