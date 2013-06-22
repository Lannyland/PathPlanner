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

        public static int intMapWith = 100;
        public static int intMapHeight = 100;
        
        public static string strDiffFileLoad = @"C:\Lanny\MAMI\IPPA\Maps\DiffMaps\TestDiffMap100.csv";
        public static string strDistFileLoad = @"C:\Lanny\MAMI\IPPA\Maps\DistMaps\TestDistMap100.csv";

        // public static string strDiffFileLoad = @"C:\Lanny\MAMI\IPPA\Maps\DiffMaps\Small_HikerPaulDiff.csv";
        // public static string strDistFileLoad = @"C:\Lanny\MAMI\IPPA\Maps\DistMaps\Smoothed_Small_HikerPaulDist.csv";


        // public static string strTerrainImage = @"http://lannyland.com/images/yulu.jpg";
        // public static string strTerrainImage = @"http://lannyland.com/images/LannylandIncHover.png";
        public static string strTerrainImage = @"C:\Lanny\MAMI\IPPA\Maps\DiffMaps\TerrainImage.jpg";

        public static bool boolUseDiffMap = false;      // Whether to use diff map for path planning.
        public static bool boolUseEndPoint = false;     // Whether to allow user to set end point for path planning.
        public static int intFlightDuration = 60;       // Default flight duration in minutes.

        // Also used as a data store
        public static RtwMatrix mOriginalDistMap = null;
        public static RtwMatrix mDistMapCurStepUndo = null;
        public static RtwMatrix mDistMapCurStepWorking = null;
        public static RtwMatrix mDiffMap = null;

        #endregion

		
        #region Editor constants

        public static int diffLevel = 1;	// 1: Easy 2: Medium 3: Difficult
		public static int appendMode = 1;	// 1: Erase 2: Increase 3: Decrease
		public static int navMode = 1;		// 1: Rotate 2: Pan
		public static int editMode = 1;		// 1: Paint 2: Select
		public static int brushSize = 1;	// Paint Brush Size from 1 to 10
		
		public static Mesh mesh = null;

        #endregion
    }
}
