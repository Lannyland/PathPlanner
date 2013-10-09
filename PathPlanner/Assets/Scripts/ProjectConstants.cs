using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using rtwmatrix;

namespace Assets.Scripts
{
    class ProjectConstants
    {
        #region Global constants

        public static int intMapWidth = 100;
        public static int intMapHeight = 100;
        
        // public static string strDiffFileLoad = @"C:\Lanny\MAMI\IPPA\Maps\DiffMaps\TestDiffMap100_012.csv";
        // public static string strDistFileLoad = @"C:\Lanny\MAMI\IPPA\Maps\DistMaps\TestDistMap100.csv";

        public static string strDiffFileLoad = @"C:\Lanny\MAMI\PathPlanner\PathPlanner\UserStudyData\TestDiffMap1.csv";
        public static string strDistFileLoad = @"C:\Lanny\MAMI\PathPlanner\PathPlanner\UserStudyData\TestDistMap1.csv";

        // public static string strDiffFileLoad = @"http://lannyland.com/Unity/TestDistMap100.csv";
        // public static string strDistFileLoad = @"http://lannyland.com/Unity/TestDiffMap100_012.csv";

        // public static string strDiffFileLoad = @"C:\Lanny\MAMI\IPPA\Maps\DiffMaps\Small_HikerPaulDiff.csv";
        // public static string strDistFileLoad = @"C:\Lanny\MAMI\IPPA\Maps\DistMaps\Smoothed_Small_HikerPaulDist.csv";


        // public static string strTerrainImage = @"http://lannyland.com/images/yulu.jpg";
        // public static string strTerrainImage = @"http://lannyland.com/images/LannylandIncHover.png";
        public static string strTerrainImage = @"C:\Lanny\MAMI\IPPA\Maps\DiffMaps\TerrainImage.jpg";

        public static bool boolUseDiffMap = true;      // Whether to use diff map for path planning.
		public static bool boolAnyEndPoint = true;
        public static int intFlightDuration = 60;       // Default flight duration in minutes.

        // Max wait time (*10)
        public static int MaxWaitTime = 600000;            // 15000 milisecond, so 15 seconds.

        // Also used as a data store
        public static RtwMatrix mOriginalDistMap = null;
        public static RtwMatrix mDistMapCurStepUndo = null;
        public static Vector3[] curVertices = null;
        public static RtwMatrix mDiffMap = new RtwMatrix(intMapHeight, intMapWidth);

        #endregion

		#region Manual Scene constants
		
		public static int editMode = 1;		// 1: Fly 2: Draw		
		public static int cameraMode = 4; 	// 1: Global 2: Behind 3: Bird's Eye 4: Free Form
        
		#endregion
			
		#region Pattern Scene constants
		
		#endregion

		#region SlidingAutonomy Scene constants

		public static int navMode = 1;		// 1: Rotate 2: Pan
		public static int resolution = 10;	// Resolution from 1 to 10
		public static int durationLeft = 60;// How much time is left at the current path segment
		public static int duration = 10;	// Current duration selected

        public static bool readyToPlanPath = false;				// Don't plan path even slider is dragged until plan path button is clicked.
		public static bool boolUseEndPoint = false;     			// Whether to allow user to set end point for path planning.
        public static bool boolPlenty = false;          		// With plenty of time use EA for path planning
        public static bool lastPathApproved = true;				// If last path is approved, user can set new end point
		public static int endPointCounter = 0;					// Remember how many end points have been created
        public static Vector2 originalStart = new Vector2();    // The UAV start position
        public static Vector2 curStart = new Vector2();         // Current start point
        public static Vector2 curEnd = new Vector2();           // Current end point
		public static bool stopPathPlanFactory = false;			// Once set to true, the worker thread quits.
		public static List<Vector2[]> AllPathSegments = new List<Vector2[]>();

        #endregion
		

		#region Old Editor Scene parameters

		// public static string strMapFileLoad = @"C:\Lanny\MAMI\IPPA\Maps\DistMaps\Smoothed_Small_HikerPaulDist.csv";
        public static string strMapFileLoad = @"C:\Lanny\MAMI\IPPA\Maps\DistMaps\TestDistMap100.csv";
        public static string strMapFileSave = @"C:\Lanny\MAMI\IPPA\Maps\DistMaps\NewDistMap.csv";
        // public static string strTerrainImage = @"http://lannyland.com/images/yulu.jpg";
        // public static string strTerrainImage = @"http://lannyland.com/images/LannylandIncHover.png";

        public static int diffLevel = 1;	// 1: Easy 2: Medium 3: Difficult
		public static int appendMode = 1;	// 1: Erase 2: Increase 3: Decrease
		public static int brushSize = 1;	// Paint Brush Size from 1 to 10
		
		public static Mesh mesh = null;

        #endregion

        #region User Study Parameters

        public static int GroupID = 0;
        public static int[,] groups = new int[60, 6];
        public static int pageIndex = 0;
        public static List<string> instructions = new List<string>();
        public static List<int> durations = new List<int>();
        public static List<string> nextScene = new List<string>();
        public static List<string> diffMaps = new List<string>();
        public static List<string> distMaps = new List<string>();
		public static List<string> chatFiles = new List<string>();
        public static List<string> tutorialFiles = new List<string>();
        public static bool UAVMovable = false;

        #endregion

		#region Saving things to log
		public static string strLogFileName = "";
		// year-month-date-hour-minute-second.txt (24hour)
		public static List<string> logs = new List<string>();
		// Things to save in logs for each exercise:
		// - group|1
		// - name|Scenario 1 Manual Flight
		// - timeleft|28
		// - score|2640
		// - mouseclicks|264
		// - complete|yes
		// - path|bla...bla...bla...
		// The following are temporary variables
		public static string timeLeft = "";
		public static string score = "";
		public static int mouseclicks = 0;
		public static int curWayPoint = 0;
		public static bool boolFlyPath = false;		
		
		// Record each time when user clicks start over button
		public static int startOverCount = 0;
		public static string startOverLog = "";

		public static List<string> replies = new List<string>();
		// User replies in chat box for each exercise:
		// - timestamp|reply
		public static List<string> comparisons = new List<string>();
		// User feedback for all exercieses:
		// - more
		// - more
		// - less
		// - more
		// - more
		


		#endregion
    }
}
