using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using rtwmatrix;
using System;
using System.Text;
using System.IO;
using Assets.Scripts;

namespace Assets.Scripts.Common
{
    class MISCLib
    {
        // Read in a csv file and then store that into matrix
        public static RtwMatrix LoadMap(string FileInName)
        {
            // Read file one line at a time and store to list.
            FileStream file = new FileStream(FileInName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string strLine;
            List<string[]> lstRow = new List<string[]>();
            // Loop through lines
            while (sr.Peek() >= 0)
            {
                strLine = sr.ReadLine().Trim();
                if (strLine.Length > 1)
                {
                    string[] splitData = strLine.Split(',');
                    lstRow.Add(splitData);
                }
            }
            sr.Close();
            file.Close();
            // Create matrix
            RtwMatrix mGrid = new RtwMatrix(lstRow.Count, lstRow[0].Length);
            for (int i = 0; i < mGrid.Rows; i++)
            {
                for (int j = 0; j < mGrid.Columns; j++)
                {
                    mGrid[i, j] = (float)(Convert.ToDouble(lstRow[i][j]));
                }
            }
            return mGrid;
        }
	
		// Save mesh vertices to a csv file
        public static void SaveMap(string FileOutName, RtwMatrix mMap)
        {
            //  Delete file if it already exists
            if (File.Exists(FileOutName))
            {
                File.Delete(FileOutName);
            }

			// Write file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileOutName))
            {
                for (int i = 0; i < mMap.Rows; i++)
                {
                    string strLine = "";
                    for (int j = 0; j < mMap.Columns; j++)
                    {
                        strLine += mMap[i, j] + ",";
                    }
                    strLine = strLine.Substring(0, strLine.Length - 1);
                    file.WriteLine(strLine);
                }
            }
        }
		
        // Scale image values between 0 and 255
        public static void ScaleImageValues(ref RtwMatrix imgin, float scale)
        {
            float[] MinMax = imgin.MinMaxValue();
            float max = MinMax[1];
            if (max != 0)
            {
                for (int y = 0; y < imgin.Rows; y++)
                {
                    for (int x = 0; x < imgin.Columns; x++)
                    {
                        imgin[y, x] = imgin[y, x] / max * scale;
                    }
                }
            }
            else
            {
                for (int y = 0; y < imgin.Rows; y++)
                {
                    for (int x = 0; x < imgin.Columns; x++)
                    {
                        imgin[y, x] = imgin[y, x] / max * scale;
                    }
                }
            }
        }
	
		// Convert RtwMatrix to vertice array
		public static Vector3[] MatrixToArray(RtwMatrix mMap)
		{
			Vector3[] vertices = new Vector3[mMap.Columns * mMap.Rows];
			int index = 0;
			for (int i=0; i<mMap.Rows; i++)
			{
				for (int j=0; j<mMap.Columns; j++)
				{
					vertices[index] = new Vector3(Convert.ToSingle(j)/10-5, mMap[i, j], Convert.ToSingle(i)/10-5);
					index++;
				}
			}
			return vertices;
		}
		
		// Convert vertice array to RtwMatrix
		public static RtwMatrix ArrayToMatrix(Vector3[] vertices)
		{
			// Assuming a square mesh
			int width = Convert.ToInt16(Math.Sqrt(vertices.Length));
			RtwMatrix mMap = new RtwMatrix(width, width);
			int index = 0;
			for (int i=0; i<mMap.Rows; i++)
			{
				for (int j=0; j<mMap.Columns; j++)
				{
					mMap[i,j] = vertices[index].y;
					index++;
				}
			}
			return mMap;			
		}
		
		// Apply Difficulty Color Map
		public static Color[] ApplyDiffColorMap(Vector3[] vs)			
		{
			Color[] colors = new Color [vs.Length];
			int index = 0;
			for(int i=0; i<vs.Length; i++)
			{
				if(vs[index].y == 0f)
				{
					colors[index]=Color.blue;
				}
				else if(vs[index].y == 1f)
				{
					colors[index]=Color.green;
				}
				else if(vs[index].y == 2f)
				{
					colors[index]=Color.red;
				}
				else
				{
						colors[index]=Color.black;
				}
				index++;
			}
			return colors;
		}

        // Apply Difficulty Color Map
        public static Color[] ApplyDistColorMap(Vector3[] vs)
        {
            Color[] colors = new Color[vs.Length];
            int index = 0;
            for (int i = 0; i < vs.Length; i++)
            {
                colors[index] = HeightToDistColor(vs[i].y, 4.0f);
                index++;
            }
            return colors;
        }
		
        // Flip map to draw correctly in Diff Editor
		public static RtwMatrix FlipTopBottom(RtwMatrix map)
		{
			RtwMatrix result = new RtwMatrix(map.Rows, map.Columns);
			for (int i=0; i<map.Rows; i++)
			{
				for (int j=0; j<map.Columns; j++)
				{
					result[i,j] = map[map.Rows - 1 -i, j];
				}
			}
			return result;
		}
		
		// Return color code based on height for Diff Maps
		public static Color HeightToDiffColor(float height)
		{
			int h = (int)Math.Round( height);
			switch (h)
			{
			case 0:
				return Color.blue;
			case 1:
				return Color.green;
			default:
				return Color.red;
			}
		}

        // Return color code based on height for Diff Maps
        public static Color HeightToDistColor(float height, float max)
        {
            float ratio = 0;
            // Deal with all flat at 0
            if (max > 0)
            {
                ratio = 1.0f - height / max;
            }
            HSLColor hslc = new HSLColor(240.0f*ratio, 1f, 0.4f);
            Color c = hslc.ToRGBA();
            return c;
        }

        // Method to see if a point is in a polygon
        // IMPORTANT: Only works with positive numbers.
        public static bool PointInPolygon(Vector2 p, Vector2[] poly)
        {
            Vector2 p1, p2;
            bool inside = false;

            if (poly.Length < 3)
            {
                return inside;
            }

            Vector2 oldPoint = new Vector2(poly[poly.Length - 1].x, poly[poly.Length - 1].y);

            for (int i = 0; i < poly.Length; i++)
            {
                Vector2 newPoint = new Vector2(poly[i].x, poly[i].y);

                if (newPoint.x > oldPoint.x)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.x < p.x) == (p.x <= oldPoint.x) &&
                   (p.y - p1.y) * (p2.x - p1.x) < (p2.y - p1.y) * (p.x - p1.x))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }

        // Method to apply a matrix to vertecies of a mesh
        public static void ApplyMatrixToMesh(RtwMatrix distMapIn, ref Mesh mesh, bool dist)
        {
            Vector3[] vertices = MISCLib.MatrixToArray(distMapIn);
            mesh.vertices = vertices;
            if(dist)
			{
			mesh.colors = MISCLib.ApplyDistColorMap(vertices);
			}
			else
			{
				mesh.colors = MISCLib.ApplyDiffColorMap(vertices);
			}
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
		
		// Method to snap object to grid
		public static void SnapToGrid (ref Transform t)
		{
			float x = Mathf.Round(t.transform.position.x * 10)/10;
			float z = Mathf.Round (t.transform.position.z * 10)/10;		
			t.transform.position = new Vector3(x, 4.0f, z);
		}
		
        // Calculating Manhattan Distance
        public static float ManhattanDistance(float x1, float y1, float x2, float y2)
        {
            float distance = Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
            return distance;
        }

        // Calculating Manhattan Distance
        public static float ManhattanDistance(Vector3 v1, Vector3 v2)
        {
            v1.x = Convert.ToSingle(Math.Round(v1.x * 10f) / 10f);
            v1.z = Convert.ToSingle(Math.Round(v1.z * 10f) / 10f);
            v2.x = Convert.ToSingle(Math.Round(v2.x * 10f) / 10f);
            v2.z = Convert.ToSingle(Math.Round(v2.z * 10f) / 10f);
            float distance = Math.Abs(v1.x - v2.x) + Math.Abs(v1.y - v2.y) + Math.Abs(v1.z - v2.z);
            return distance;
        }
		
		// Load chat text files
		public static void LoadChatFile(string FileInName, ref List<string> lstChatLines, ref List<int> lstTimeStamps)
		{
            // Read file one line at a time and store to list.
            FileStream file = new FileStream(FileInName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string strLine;
            // Loop through lines
            while (sr.Peek() >= 0)
            {
                strLine = sr.ReadLine().Trim();
				if (strLine.Length > 1)
                {
                    string timeStamp = strLine.Substring(0, 5);
					int time = ConvertTimeStampToInt(timeStamp);
					string text = strLine.Substring(5, strLine.Length-5);
					lstTimeStamps.Add(time);
                    lstChatLines.Add(text);
                }
            }
            sr.Close();
            file.Close();			
		}

        // Load tutorial text files
        public static void LoadTutorialFile(string FileInName, ref List<string> lstTutorialLines, ref List<int> lstTimeStamps, ref List<string> lstLocations)
        {
            // Read file one line at a time and store to list.
            FileStream file = new FileStream(FileInName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string strLine;
            // Loop through lines
            while (sr.Peek() >= 0)
            {
                strLine = sr.ReadLine().Trim();
                if (strLine.Length > 1)
                {
                    string[] segments = strLine.Split('|');
                    string timeStamp = segments[0];
                    int time = ConvertTimeStampToInt(timeStamp);
                    string text = segments[1];
                    string location = segments[2];
                    lstTimeStamps.Add(time);
                    lstTutorialLines.Add(text);
                    lstLocations.Add(location);
                }
            }
            sr.Close();
            file.Close();
        }

		
		// Convert time stamp to seconds
		public static int ConvertTimeStampToInt(string timeStamp)
		{
			string[] line = timeStamp.Split(':');
			int minute = Convert.ToInt16(line[0]);
			int second = Convert.ToInt16(line[1]);
			return minute * 60 + second;
		}
		
		// Generate log file name
		public static string GetLogFileName()
		{
			string fileName = "";
			fileName = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
			fileName += ".txt";
			return fileName;
		}
		
		// Save things to log file
		public static void SaveToLogFile(string strToWrite)			
		{
			// Find app folder
			string TargetDirectory = Application.dataPath + @"\..\" + "UserStudyLogs";
			
			// Check if log folder already exists. Create folder if it doesn't exist
            if (!Directory.Exists(TargetDirectory))
            {
                Directory.CreateDirectory(TargetDirectory);
            }

			// Check if file already exists
			string TargetFile = TargetDirectory + "\\" + ProjectConstants.strLogFileName;
			
			// Create file if it doesn't exists
            StreamWriter file = new StreamWriter(TargetFile, true);
			file.WriteLine(strToWrite);
			file.Close();
		}		

		// Save things to log file
		public static void SaveToLogFile(List<string> lstToWrite)			
		{
			// Find app folder
			string TargetDirectory = Application.dataPath + @"\..\" + "UserStudyLogs";
			
			// Check if log folder already exists. Create folder if it doesn't exist
            if (!Directory.Exists(TargetDirectory))
            {
                Directory.CreateDirectory(TargetDirectory);
            }

			// Check if file already exists
			string TargetFile = TargetDirectory + "\\" + ProjectConstants.strLogFileName;
			
			// Create file if it doesn't exists
            StreamWriter file = new StreamWriter(TargetFile, true);
			for(int i=0; i<lstToWrite.Count; i++)
			{
				file.WriteLine(lstToWrite[i]);
			}			
			file.Close();
		}
		
		// Start over logs
		public static void StartOverLogs()
		{
			ProjectConstants.startOverCount++;
            if (ProjectConstants.boolFlyPath != true)
            {
                ProjectConstants.timeLeft = GameObject.Find("lblTime").GetComponent<UILabel>().text;
                ProjectConstants.score = GameObject.Find("lblScore").GetComponent<UILabel>().text;
            }
            Debug.Log("timeLeft = " + ProjectConstants.timeLeft + " score = " + ProjectConstants.score);
			ProjectConstants.startOverLog += "startover|";
			ProjectConstants.startOverLog += "timeleft|";
			ProjectConstants.startOverLog += ProjectConstants.timeLeft;
			ProjectConstants.startOverLog += "|score|";
			ProjectConstants.startOverLog += ProjectConstants.score;
			ProjectConstants.startOverLog += "\n";
		}
	}
}