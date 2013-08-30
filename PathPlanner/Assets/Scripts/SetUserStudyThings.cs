using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;

public class SetUserStudyThings : MonoBehaviour {

	// Use this for initialization
	void Start () {

        // Debug.Log("pageIndex = " + ProjectConstants.pageIndex);
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
		// Debug.Log("strAppDir = " + strAppDir);
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
			// Remember log file name
			ProjectConstants.strLogFileName = MISCLib.GetLogFileName();
			// Write group id in log file
			MISCLib.SaveToLogFile("group|"+ProjectConstants.GroupID);
			
            // Hide Group ID field
            uiGroupID.transform.position = new Vector3(-10000f, -10000f, -10000f);

            #region Screen pairs that will be rearranged based on Group ID selection

            // The following three screens are for real test manual flight
            List<string> instM = new List<string>();
            List<int> durM = new List<int>();
            List<string> nextM = new List<string>();
            List<string> diffM = new List<string>();
            List<string> distM = new List<string>();
			List<string> chatM = new List<string>();

            string strM1 = "Scenario 1 Manual Flight\n\n";
            strM1 = strM1 + "In this scenario, you will use the [9696FB]manual planning mode[FFFFFF] to plan a UAV path for a [9696FB]60 \nminutes[FFFFFF] flight. [9696FB]No task difficulty map is used[FFFFFF] in this exercise.\n\n";
            strM1 = strM1 + "You objective is to score as high as possible within a [9696FB]5-minute[FFFFFF] time window. Once you are satisfied \nwith the path you created, you can end the exercise early by clicking the [9696FB]Next[FFFFFF] button.\n\n";
            strM1 = strM1 + "While your main task is to plan the UAV path, try your best to [9696FB]answer questions[FFFFFF] in the [9696FB]chat \nbox[FFFFFF] when your code name [9696FB]Eagle[FFFFFF] is called.\n\n";
            strM1 = strM1 + "You have [9696FB]5 minutes[FFFFFF] to complete the exercise. Once time is up, please stop and click \n[9696FB]Next[FFFFFF] to continue.";
            instM.Add(strM1);
            durM.Add(0);
            nextM.Add("UserStudyManualFlight");
            diffM.Add("");
            distM.Add("");
			chatM.Add("");
			
            string strM2 = "";
            instM.Add(strM2);
            durM.Add(5);
            nextM.Add("UserStudyCompare");
            diffM.Add("");
            distM.Add(strAppDir + @"\TestDistMap1.csv");
			chatM.Add(strAppDir + @"\ChatBoxManual1.txt");

            AddComparisonScreen(instM, durM, nextM, diffM, distM, chatM, "UserStudy");

            // The following three screens are for real test pattern flight
            List<string> instP = new List<string>();
            List<int> durP = new List<int>();
            List<string> nextP = new List<string>();
            List<string> diffP = new List<string>();
            List<string> distP = new List<string>();
			List<string> chatP = new List<string>();

            string strP1 = "Scenario 1 Pattern Flight\n\n";
            strP1 = strP1 + "In this scenario, you will use the [9696FB]pattern planning mode[FFFFFF] to plan a UAV path for a [9696FB]60 \nminutes[FFFFFF] flight. [9696FB]No task difficulty map is used[FFFFFF] in this exercise.\n\n";
            strP1 = strP1 + "You objective is to score as high as possible within a [9696FB]5-minute[FFFFFF] time window. Once you are satisfied \nwith the path you created, you can end the exercise early by clicking the [9696FB]Next[FFFFFF] button.\n\n";
            strP1 = strP1 + "While your main task is to plan the UAV path, try your best to [9696FB]answer questions[FFFFFF] in the [9696FB]chat \nbox[FFFFFF] when your code name [9696FB]Eagle[FFFFFF] is called.\n\n";
            strP1 = strP1 + "You have [9696FB]5 minutes[FFFFFF] to complete the exercise. Once time is up, please stop and click \n[9696FB]Next[FFFFFF] to continue.";
            instP.Add(strP1);
            durP.Add(0);
            nextP.Add("UserStudyPatternFlight");
            diffP.Add("");
            distP.Add("");
			chatP.Add("");

            string strP2 = "";
            instP.Add(strP2);
            durP.Add(5);
            nextP.Add("UserStudyCompare");
            diffP.Add("");
            distP.Add(strAppDir + @"\TestDistMap1.csv");
			chatP.Add(strAppDir + @"\ChatBoxPattern1.txt");

            AddComparisonScreen(instP, durP, nextP, diffP, distP, chatP, "UserStudy");

            // The following two screens are for real test sliding autonomy flight
            List<string> instS = new List<string>();
            List<int> durS = new List<int>();
            List<string> nextS = new List<string>();
            List<string> diffS = new List<string>();
            List<string> distS = new List<string>();
			List<string> chatS = new List<string>();

            string strS1 = "Scenario 1 Sliding Autonomy Flight\n\n";
            strS1 = strS1 + "In this scenario, you will use the [9696FB]sliding autonomy planning mode[FFFFFF] to plan a UAV path for a [9696FB]60 \nminutes[FFFFFF] flight. [9696FB]No task difficulty map is used[FFFFFF] in this exercise.\n\n";
            strS1 = strS1 + "You objective is to score as high as possible within a [9696FB]5-minute[FFFFFF] time window. Once you are satisfied \nwith the path you created, you can end the exercise early by clicking the [9696FB]Next[FFFFFF] button.\n\n";
            strS1 = strS1 + "While your main task is to plan the UAV path, try your best to [9696FB]answer questions[FFFFFF] in the [9696FB]chat \nbox[FFFFFF] when your code name [9696FB]Eagle[FFFFFF] is called.\n\n";
            strS1 = strS1 + "You have [9696FB]5 minutes[FFFFFF] to complete the exercise. Once time is up, please stop and click \n[9696FB]Next[FFFFFF] to continue.";
            instS.Add(strS1);
            durS.Add(0);
            nextS.Add("UserStudySlidingAutonomy");
            diffS.Add("");
            distS.Add("");
			chatS.Add("");		

            string strS2 = "";
            instS.Add(strS2);
            durS.Add(5);
            nextS.Add("UserStudyCompare");
            diffS.Add("");
            distS.Add(strAppDir + @"\TestDistMap1.csv");
			chatS.Add(strAppDir + @"\ChatBoxSliding1.txt");

            AddComparisonScreen(instS, durS, nextS, diffS, distS, chatS, "UserStudy");

            // The following two screens are for real test manual flight with difficulty map
            List<string> instMD = new List<string>();
            List<int> durMD = new List<int>();
            List<string> nextMD = new List<string>();
            List<string> diffMD = new List<string>();
            List<string> distMD = new List<string>();
			List<string> chatMD = new List<string>();

            string strMD1 = "Scenario 2 Manual Flight\n\n";
            strMD1 = strMD1 + "In this scenario, you will use the [9696FB]manual planning mode[FFFFFF] to plan a UAV path for a [9696FB]60 \nminutes[FFFFFF] flight.\n\n";
            strMD1 = strMD1 + "Note that [9696FB]a task difficulty is used[FFFFFF] in this exercise.\n\n";
            strMD1 = strMD1 + "You objective is to score as high as possible within a [9696FB]5-minute[FFFFFF] time window. Once you are satisfied \nwith the path you created, you can end the exercise early by clicking the [9696FB]Next[FFFFFF] button.\n\n";
            strMD1 = strMD1 + "While your main task is to plan the UAV path, try your best to [9696FB]answer questions[FFFFFF] in the [9696FB]chat \nbox[FFFFFF] when your code name [9696FB]Eagle[FFFFFF] is called.\n\n";
            strMD1 = strMD1 + "You have [9696FB]5 minutes[FFFFFF] to complete the exercise. Once time is up, please stop and click \n[9696FB]Next[FFFFFF] to continue.";
            instMD.Add(strMD1);
            durMD.Add(0);
            nextMD.Add("UserStudyManualFlight");
            diffMD.Add("");
            distMD.Add("");
			chatMD.Add("");
			
            string strMD2 = "";
            instMD.Add(strMD2);
            durMD.Add(5);
            nextMD.Add("UserStudyCompare");
            diffMD.Add(strAppDir + @"\TestDiffMap2.csv");
            distMD.Add(strAppDir + @"\TestDistMap2.csv");
			chatMD.Add(strAppDir + @"\ChatBoxManual2.txt");

            AddComparisonScreen(instMD, durMD, nextMD, diffMD, distMD, chatMD, "UserStudy");

            // The following two screens are for real test pattern flight with difficulty map
            List<string> instPD = new List<string>();
            List<int> durPD = new List<int>();
            List<string> nextPD = new List<string>();
            List<string> diffPD = new List<string>();
            List<string> distPD = new List<string>();
			List<string> chatPD = new List<string>();

            string strPD1 = "Scenario 2 Pattern Flight\n\n";
            strPD1 = strPD1 + "In this scenario, you will use the [9696FB]pattern planning mode[FFFFFF] to plan a UAV path for a [9696FB]60 \nminutes[FFFFFF] flight.\n\n";
            strPD1 = strPD1 + "Note that [9696FB]a task difficulty is used[FFFFFF] in this exercise.\n\n";
            strPD1 = strPD1 + "You objective is to score as high as possible within a [9696FB]5-minute[FFFFFF] time window. Once you are satisfied \nwith the path you created, you can end the exercise early by clicking the [9696FB]Next[FFFFFF] button.\n\n";
            strPD1 = strPD1 + "While your main task is to plan the UAV path, try your best to [9696FB]answer questions[FFFFFF] in the [9696FB]chat \nbox[FFFFFF] when your code name [9696FB]Eagle[FFFFFF] is called.\n\n";
            strPD1 = strPD1 + "You have [9696FB]5 minutes[FFFFFF] to complete the exercise. Once time is up, please stop and click \n[9696FB]Next[FFFFFF] to continue.";
            instPD.Add(strPD1);
            durPD.Add(0);
            nextPD.Add("UserStudyPatternFlight");
            diffPD.Add("");
            distPD.Add("");
			chatPD.Add("");

            string strPD2 = "";
            instPD.Add(strPD2);
            durPD.Add(5);
            nextPD.Add("UserStudyCompare");
            diffPD.Add(strAppDir + @"\TestDiffMap2.csv");
            distPD.Add(strAppDir + @"\TestDistMap2.csv");
			chatPD.Add(strAppDir + @"\ChatBoxPattern2.txt");

            AddComparisonScreen(instPD, durPD, nextPD, diffPD, distPD, chatPD, "UserStudy");

            // The following two screens are for real test sliding autonomy flight with difficulty map
            List<string> instSD = new List<string>();
            List<int> durSD = new List<int>();
            List<string> nextSD = new List<string>();
            List<string> diffSD = new List<string>();
            List<string> distSD = new List<string>();
			List<string> chatSD = new List<string>();

            string strSD1 = "Scenario 2 Sliding Autonomy Flight\n\n";
            strSD1 = strSD1 + "In this scenario, you will use the [9696FB]sliding autonomy planning mode[FFFFFF] to plan a UAV path for a [9696FB]60 \nminutes[FFFFFF] flight.\n\n";
            strSD1 = strSD1 + "Note that [9696FB]a task difficulty is used[FFFFFF] in this exercise.\n\nYou objective is to score as high as possible within a [9696FB]5-minute[FFFFFF] time window. Once you are satisfied \nwith the path you created, you can end the exercise early by clicking the [9696FB]Next[FFFFFF] button.\n\n";
            strSD1 = strSD1 + "You objective is to score as high as possible within a [9696FB]5-minute[FFFFFF] time window. Once you are satisfied \nwith the path you created, you can end the exercise early by clicking the [9696FB]Next[FFFFFF] button.\n\n"; 
            strSD1 = strSD1 + "While your main task is to plan the UAV path, try your best to [9696FB]answer questions[FFFFFF] in the [9696FB]chat \nbox[FFFFFF] when your code name [9696FB]Eagle[FFFFFF] is called.\n\n";
            strSD1 = strSD1 + "You have [9696FB]5 minutes[FFFFFF] to complete the exercise. Once time is up, please stop and click \n[9696FB]Next[FFFFFF] to continue.";
            instSD.Add(strSD1);
            durSD.Add(0);
            nextSD.Add("UserStudySlidingAutonomy");
            diffSD.Add("");
            distSD.Add("");
			chatSD.Add("");		

            string strSD2 = "";
            instSD.Add(strSD2);
            durSD.Add(5);
            nextSD.Add("UserStudyCompare");
            diffSD.Add(strAppDir + @"\TestDiffMap2.csv");
            distSD.Add(strAppDir + @"\TestDistMap2.csv");
			chatSD.Add(strAppDir + @"\ChatBoxSliding2.txt");

            AddComparisonScreen(instSD, durSD, nextSD, diffSD, distSD, chatSD, "UserStudy");

            #endregion

            #region Different sequences based on Group ID

            int i = ProjectConstants.nextScene.Count + 3;
            switch (ProjectConstants.GroupID)
            {
                case 1:  // Scenario 1 first. Manual first.
                    ManualNoDiff(instM, durM, nextM, diffM, distM, chatM);
                    NoCompareScreen(i);
                    PatternNoDiff(instP, durP, nextP, diffP, distP, chatP);
                    SlideNoDiff(instS, durS, nextS, diffS, distS, chatS);

                    ManualWithDiff(instMD, durMD, nextMD, diffMD, distMD, chatMD);
                    PatternWithDiff(instPD, durPD, nextPD, diffPD, distPD, chatPD);
                    SlideWithDiff(instSD, durSD, nextSD, diffSD, distSD, chatSD);

                    break;
                case 2: // Scenario 1 first. Pattern first.
                    PatternNoDiff(instP, durP, nextP, diffP, distP, chatP);
                    NoCompareScreen(i);
                    SlideNoDiff(instS, durS, nextS, diffS, distS, chatS);
                    ManualNoDiff(instM, durM, nextM, diffM, distM, chatM);

                    PatternWithDiff(instPD, durPD, nextPD, diffPD, distPD, chatPD);
                    SlideWithDiff(instSD, durSD, nextSD, diffSD, distSD, chatSD);
                    ManualWithDiff(instMD, durMD, nextMD, diffMD, distMD, chatMD);

                    break;
                case 3: // Scenario 1 first. Sliding first.
                    SlideNoDiff(instS, durS, nextS, diffS, distS, chatS);
                    NoCompareScreen(i);
                    ManualNoDiff(instM, durM, nextM, diffM, distM, chatM);
                    PatternNoDiff(instP, durP, nextP, diffP, distP, chatP);

                    SlideWithDiff(instSD, durSD, nextSD, diffSD, distSD, chatSD);
                    ManualWithDiff(instMD, durMD, nextMD, diffMD, distMD, chatMD);
                    PatternWithDiff(instPD, durPD, nextPD, diffPD, distPD, chatPD);

                    break;
                case 4: // Scenario 2 first. Manual first.
                    ManualWithDiff(instMD, durMD, nextMD, diffMD, distMD, chatMD);
                    NoCompareScreen(i);
                    PatternWithDiff(instPD, durPD, nextPD, diffPD, distPD, chatPD);
                    SlideWithDiff(instSD, durSD, nextSD, diffSD, distSD, chatSD);

                    ManualNoDiff(instM, durM, nextM, diffM, distM, chatM);
                    PatternNoDiff(instP, durP, nextP, diffP, distP, chatP);
                    SlideNoDiff(instS, durS, nextS, diffS, distS, chatS);

                    break;
                case 5: // Scenario 2 first. Pattern first.
                    PatternWithDiff(instPD, durPD, nextPD, diffPD, distPD, chatPD);
                    NoCompareScreen(i);
                    SlideWithDiff(instSD, durSD, nextSD, diffSD, distSD, chatSD);
                    ManualWithDiff(instMD, durMD, nextMD, diffMD, distMD, chatMD);

                    PatternNoDiff(instP, durP, nextP, diffP, distP, chatP);
                    SlideNoDiff(instS, durS, nextS, diffS, distS, chatS);
                    ManualNoDiff(instM, durM, nextM, diffM, distM, chatM);

                    break;
                case 6: // Scenario 2 first. Sliding first.
                    SlideWithDiff(instSD, durSD, nextSD, diffSD, distSD, chatSD);
                    NoCompareScreen(i);
                    ManualWithDiff(instMD, durMD, nextMD, diffMD, distMD, chatMD);
                    PatternWithDiff(instPD, durPD, nextPD, diffPD, distPD, chatPD);

                    SlideNoDiff(instS, durS, nextS, diffS, distS, chatS);
                    ManualNoDiff(instM, durM, nextM, diffM, distM, chatM);
                    PatternNoDiff(instP, durP, nextP, diffP, distP, chatP);

                    break;
                default:

                    break;
            }

            #endregion

            string str29 = "\n\n\n\n\nThank you for doing the user study. Now please [9696FB]take the final survey[FFFFFF].";
            ProjectConstants.instructions.Add(str29);
            ProjectConstants.durations.Add(0);
            ProjectConstants.nextScene.Add("");
            ProjectConstants.diffMaps.Add("");
            ProjectConstants.distMaps.Add("");
			ProjectConstants.chatFiles.Add("");

            return;
        }

        // First screen set Group ID
        string str1 = "\n\n\n\n\nPlease enter a Group ID [9696FB](1 to 6)[FFFFFF] and then click Next to continue.";
        ProjectConstants.instructions.Add(str1);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");
        ProjectConstants.tutorialFiles.Add("");

        // Second screen welcomes user and explains
        // -- Purpose of user study
        // -- Two maps
        // -- HOw UAV vacuums and the goal of the tasks
        // -- Secondary task
        string str2 = "Welcome to the UAV Path Planning user study.\n\n";
        str2 = str2 + "In the following simulated Wilderness Search and Rescue (WiSAR) operations, a person is reported missing in each \nscenario, and your help is needed to plan a path for the Unmanned Aerial Vehicle (UAV) in order to search for the \nmissing person. The UAV is a hexacopter, meaning it can fly to any direction without turning or hover in the air.\n\n";
        str2 = str2 + "On the screen you will see a probability distribution map indicating where are likely places of finding the missing \nperson (hills in red). A task difficulty map might be used to incidate how difficult it is for the UAV to detect \nthe missing person (red for very difficult, green for medium difficulty, and blue for easy).\n\n";
        str2 = str2 + "You can think of the UAV as a vacuum cleaner and as it flies, it vacuums up probability along its way. More \nprobability will be vacuumed up in areas marked easy and few will be vacuumed up in areas marked difficult.\n\n";
        str2 = str2 + "Your objective is to plan a path for the UAV so it can vacuum up as much probability (indicated by your score) as \npossible for a fixed length flight.\n\n";
        str2 = str2 + "You will help plan paths for [9696FB]two WiSAR scenarios[FFFFFF] using [9696FB]three different planning \nmodes[FFFFFF]: [9696FB]manual[FFFFFF], [9696FB]pattern[FFFFFF], and [9696FB]sliding autonomy[FFFFFF].\n\n";
        str2 = str2 + "But first let's practice how to use the three planning modes to plan UAV paths.";
        ProjectConstants.instructions.Add(str2);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");
        ProjectConstants.tutorialFiles.Add("");
        
        // Third screen training manual flight insturctions
        string str3 = "Manual Flight Training\n\n";
        str3 = str3 + "In the next training exercise you will use the [9696FB]manual planning mode[FFFFFF] to plan a UAV path for a [9696FB]60 minutes[FFFFFF] flight.\n\n";
        str3 = str3 + "You can move the UAV around using the arrow keys. There are four camera views to choose from, and WASD keys \ncan be used to rotate/pan the view. Mouse scrollwheel can be used to zoom in/out.\n\n";
        str3 = str3 + "In this exercise [9696FB]no task difficulty map is used[FFFFFF], meaning the UAV can easily vacuum up probability in the entire \nsearch area.\n\n";
        str3 = str3 + "Please follow instructions on screen. You have [9696FB]3 minutes[FFFFFF] to complete the exercise.";
        ProjectConstants.instructions.Add(str3);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudyManualFlightTraining");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");
        ProjectConstants.tutorialFiles.Add("");

        // Fourth screen training manual flight
        string str4 = "";
        ProjectConstants.instructions.Add(str4);
        ProjectConstants.durations.Add(3);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add(strAppDir + @"\TrainingDistMap0.csv");
		ProjectConstants.chatFiles.Add(strAppDir + @"\ChatBoxTraining1.txt");
        ProjectConstants.tutorialFiles.Add(strAppDir + @"\Tutorial1.txt");

        // Fifth screen training pattern flight insturctions
        string str5 = "Pattern Flight Training\n\n";
        str5 = str5 + "In the next training exercise you will use the [9696FB]pattern planning mode[FFFFFF] to plan a UAV path for a [9696FB]60 minutes[FFFFFF] flight. \n\n";
        str5 = str5 + "You can choose from three patterns: [9696FB]lawnmower[FFFFFF], [9696FB]spiral[FFFFFF], and [9696FB]line[FFFFFF]. There are two camera views to choose from, and \nWASD keys can be used to rotate/pan the view. Mouse scrollwheel can be used to zoom in/out.\n\n";
        str5 = str5 + "In this exercise [9696FB]no task difficulty map is used[FFFFFF], meaning the UAV can easily vacuum up probability in the entire \nsearch area.\n\n";
        str5 = str5 + "Please follow instructions on screen. You have [9696FB]3 minutes[FFFFFF] to complete the exercise.";
        ProjectConstants.instructions.Add(str5);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudyPatternFlightTraining");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");
        ProjectConstants.tutorialFiles.Add("");

        // Sixth screen training pattern flight
        string str6 = "";
        ProjectConstants.instructions.Add(str6);
        ProjectConstants.durations.Add(3);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add(strAppDir + @"\TrainingDistMap0.csv");
		ProjectConstants.chatFiles.Add(strAppDir + @"\ChatBoxTraining2.txt");
        ProjectConstants.tutorialFiles.Add(strAppDir + @"\Tutorial2.txt");

        // Seventh screen training sliding autonomy flight insturctions
        string str7 = "Sliding Autonomy Flight Training\n\n";
        str7 = str7 + "In the next training exercise you will use the [9696FB]sliding autonomy planning mode[FFFFFF] to plan a UAV path for a [9696FB]60 minutes[FFFFFF] \nflight.\n\n";
        str7 = str7 + "You can set where you want the path segment to end at, and use two sliders to control how much time you want to \nallocate to the current path segment. WASD keys can be used to rotate/pan the view. Mouse scrollwheel can be \nused to zoom in/out.\n\n";
        str7 = str7 + "In this exercise [9696FB]no task difficulty map is used[FFFFFF], meaning the UAV can easily vacuum up probability in the entire \nsearch area.\n\n";
        str7 = str7 + "Please follow instructions on screen. You have [9696FB]3 minutes[FFFFFF] to complete the exercise.";
        ProjectConstants.instructions.Add(str7);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudySlidingAutonomyTraining");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");
        ProjectConstants.tutorialFiles.Add("");
		
        // Eighth screen training sliding autonomy flight
        string str8 = "";
        ProjectConstants.instructions.Add(str8);
        ProjectConstants.durations.Add(3);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add(strAppDir + @"\TrainingDistMap0.csv");
		ProjectConstants.chatFiles.Add(strAppDir + @"\ChatBoxTraining3.txt");
        ProjectConstants.tutorialFiles.Add(strAppDir + @"\Tutorial3.txt");

        // Nineth screen training pattern flight with difficulty map insturctions
        string str9 = "Pattern Flight With Difficulty Map Training\n\n";
        str9 = str9 + "In the next training exercise you will use the [9696FB]pattern planning mode[FFFFFF] to plan a UAV path for a [9696FB]60 minutes[FFFFFF] flight. \nThis time [9696FB]a task difficulty map is used[FFFFFF]. In areas marked easy (blue) the UAV can easily vacuum up \nprobability; in areas marked difficult (red) the UAV can only vacuum up small amount of probability.\n\n";
        str9 = str9 + "You can choose from three patterns: [9696FB]lawnmower[FFFFFF], [9696FB]spiral[FFFFFF], and [9696FB]line[FFFFFF]. There are two camera views to choose from, \nand WASD keys can be used to rotate/pan the view. Mouse scrollwheel can be used to zoom in/out.\n\n";
        str9 = str9 + "Please follow instructions on screen. You have [9696FB]3 minutes[FFFFFF] to complete the exercise.";
        ProjectConstants.instructions.Add(str9);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudyPatternFlightTraining");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
		ProjectConstants.chatFiles.Add("");
        ProjectConstants.tutorialFiles.Add("");

        // Tenth screen training pattern flight with difficulty map
        string str10 = "";
        ProjectConstants.instructions.Add(str10);
        ProjectConstants.durations.Add(3);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add(strAppDir + @"\TrainingDiffMap0.csv");
        ProjectConstants.distMaps.Add(strAppDir + @"\TrainingDistMap0.csv");
		ProjectConstants.chatFiles.Add(strAppDir + @"\ChatBoxTraining4.txt");
        ProjectConstants.tutorialFiles.Add(strAppDir + @"\Tutorial4.txt");

        // Eleventh screen indicating the real exercises are starting
        string str11 = "Training Session Completed\n\n";
        str11 = str11 + "This is the end of the training exercises. Please also familiarize yourself with the [9696FB]survey[FFFFFF] form on \nthe other computer screen. Once you feel comfortable with the survey form, click [9696FB]Submit[FFFFFF] and then \ncome back to this program.\n\n";
        str11 = str11 + "When you are ready to start the user study, click [9696FB]Next[FFFFFF] to continue.";
        ProjectConstants.instructions.Add(str11);
        ProjectConstants.durations.Add(0);
        ProjectConstants.nextScene.Add("UserStudy");
        ProjectConstants.diffMaps.Add("");
        ProjectConstants.distMaps.Add("");
        ProjectConstants.chatFiles.Add("");
    }

    private static void AddComparisonScreen(List<string> instM, List<int> durM, List<string> nextM, List<string> diffM, List<string> distM, List<string> chatM, string t)
    {
        string str = "Please take the survey on the other computer screen and then answer the question below:\n\n"
			+ "Comparing to the previous task, is this task you just completed more or less difficult than the one before it?";
		instM.Add(str);
        durM.Add(0);
        nextM.Add(t);
        diffM.Add("");
        distM.Add("");
        chatM.Add("");
    }

    private static void SlideWithDiff(List<string> instSD, List<int> durSD, List<string> nextSD, List<string> diffSD, List<string> distSD, List<string> chatSD)
    {
        // Sliding Autonomy with Diff
        ProjectConstants.instructions.AddRange(instSD);
        ProjectConstants.durations.AddRange(durSD);
        ProjectConstants.nextScene.AddRange(nextSD);
        ProjectConstants.diffMaps.AddRange(diffSD);
        ProjectConstants.distMaps.AddRange(distSD);
        ProjectConstants.chatFiles.AddRange(chatSD);
    }

    private static void PatternWithDiff(List<string> instPD, List<int> durPD, List<string> nextPD, List<string> diffPD, List<string> distPD, List<string> chatPD)
    {
        // Pattern with Diff
        ProjectConstants.instructions.AddRange(instPD);
        ProjectConstants.durations.AddRange(durPD);
        ProjectConstants.nextScene.AddRange(nextPD);
        ProjectConstants.diffMaps.AddRange(diffPD);
        ProjectConstants.distMaps.AddRange(distPD);
        ProjectConstants.chatFiles.AddRange(chatPD);
    }

    private static void ManualWithDiff(List<string> instMD, List<int> durMD, List<string> nextMD, List<string> diffMD, List<string> distMD, List<string> chatMD)
    {
        // Manual with Diff
        ProjectConstants.instructions.AddRange(instMD);
        ProjectConstants.durations.AddRange(durMD);
        ProjectConstants.nextScene.AddRange(nextMD);
        ProjectConstants.diffMaps.AddRange(diffMD);
        ProjectConstants.distMaps.AddRange(distMD);
        ProjectConstants.chatFiles.AddRange(chatMD);
    }

    private static void SlideNoDiff(List<string> instS, List<int> durS, List<string> nextS, List<string> diffS, List<string> distS, List<string> chatS)
    {
        // Sliding Autonomy No Diff
        ProjectConstants.instructions.AddRange(instS);
        ProjectConstants.durations.AddRange(durS);
        ProjectConstants.nextScene.AddRange(nextS);
        ProjectConstants.diffMaps.AddRange(diffS);
        ProjectConstants.distMaps.AddRange(distS);
        ProjectConstants.chatFiles.AddRange(chatS);
    }

    private static void PatternNoDiff(List<string> instP, List<int> durP, List<string> nextP, List<string> diffP, List<string> distP, List<string> chatP)
    {
        // Pattern No Diff
        ProjectConstants.instructions.AddRange(instP);
        ProjectConstants.durations.AddRange(durP);
        ProjectConstants.nextScene.AddRange(nextP);
        ProjectConstants.diffMaps.AddRange(diffP);
        ProjectConstants.distMaps.AddRange(distP);
        ProjectConstants.chatFiles.AddRange(chatP);
    }

    private static void ManualNoDiff(List<string> instM, List<int> durM, List<string> nextM, List<string> diffM, List<string> distM, List<string> chatM)
    {
        // Manual No Diff
        ProjectConstants.instructions.AddRange(instM);
        ProjectConstants.durations.AddRange(durM);
        ProjectConstants.nextScene.AddRange(nextM);
        ProjectConstants.diffMaps.AddRange(diffM);
        ProjectConstants.distMaps.AddRange(distM);
        ProjectConstants.chatFiles.AddRange(chatM);
    }

    private static void NoCompareScreen(int i)
    {
        // No need to compare for first one
        ProjectConstants.nextScene[i - 2] = ProjectConstants.nextScene[i - 1];
        ProjectConstants.instructions.RemoveAt(i - 1);
        ProjectConstants.durations.RemoveAt(i - 1);
        ProjectConstants.nextScene.RemoveAt(i - 1);
        ProjectConstants.diffMaps.RemoveAt(i - 1);
        ProjectConstants.distMaps.RemoveAt(i - 1);
        ProjectConstants.chatFiles.RemoveAt(i - 1);
    }

}
