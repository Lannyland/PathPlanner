using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using rtwmatrix;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TCPIPTest;

public class StartUpChores : MonoBehaviour {
	
	String strText = "Hi!\n";	
	
	// Use this for initialization
	void Start () {
			
	}
		
	// Update is called once per frame
	void Update () {

	}
	
	// Prepare GUI components
	void OnGUI () {
		// Go to full screen mode
		// Screen.fullScreen = true;;
		
		GUI.Label (new Rect(10, 10, 300, 100), strText);								
		
		// Make a background box
		// GUI.Box(new Rect((1920-300)/2,(1080-1000)/2,500,700), "Loader Menu");
		// GUI.Box(new Rect(10,10,1910,1070), "Loader Menu");

/*		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect((1920-300)/2+50,(1080-1000)/2+50,400,200), "Manual Control Path Planning")) {
			Application.LoadLevel("ManualFlight");
		}

*/				// Make the second button.
		if(GUI.Button(new Rect((1920-300)/2+50,(1080-1000)/2+260,400,200), "Flight Patterns Path Planning")) {
			
			// Test Client Server Call here.
			TestCall();			
		}
/*
		// Make the third button.
		if(GUI.Button(new Rect((1920-300)/2+50,(1080-1000)/2+260+210,400,200), "Sliding Autonomy Path Planning")) {
			strText = "Hello World!";
		}
			
*/	 

	}	
	
	// Testing client server call to path planning server IPPA
	void TestCall()
	{
	    // Generate path planning requests
        PathPlanningRequest newRequest = new PathPlanningRequest();
        if (!BuildRequest(newRequest))
        {
            return;
        }
        
        // Build Path Request into Object
        // Object via protocol buffer
        byte[] outStream = RequestToByteArray(newRequest);

        // Add header to indicate data size
        byte[] outStreamFinal = outStreamFinal = ProtoBuffer.TCPHandler.AddDataSizeHeader(outStream);
        
        // Send request
        try
        {
            TcpClient clientSocket = new TcpClient();
            clientSocket.Connect("127.0.0.1", 8888);

            NetworkStream serverStream = clientSocket.GetStream();

            // Send data over socket connection
            serverStream.Write(outStreamFinal, 0, outStreamFinal.Length);
            serverStream.Flush();

            // Get server response
            byte[] inStream = ProtoBuffer.TCPHandler.RecieveData(clientSocket, serverStream);
            
            // Convert byte array to things we want
            if (inStream.Length == 0)
            {
               Debug.Log("Server exception occurred. No data received!");
            }
            else
            {
                Debug.Log("Efficiency = " + BitConverter.ToDouble(inStream, 0) + "\n");
                Debug.Log("Run Time = " + BitConverter.ToDouble(inStream, 8) + "\n");
				strText += "Efficiency = " + BitConverter.ToDouble(inStream, 0) + "\n";
                strText += "Run Time = " + BitConverter.ToDouble(inStream, 8) + "\n";

                int[] curP = new int[inStream.Length - 16];
                Debug.Log("Path = ");
                for (int i = 16; i < inStream.Length; i = i + 4)
                {
                    Debug.Log(BitConverter.ToInt32(inStream, i).ToString() + " ");
                    if ((i / 4) % 2 == 1)
                    {
                        Debug.Log(", ");
                    }
                }
            }
        }
        catch (System.Net.Sockets.SocketException ex)
        {
            if (ex.ErrorCode == 10061)
            {
                Debug.Log("Server not available!");
            }
            else
            {
                Debug.Log(ex.Message);
            }                
        }
	}	
	
    // Building the path planning request object and does sanity check
    private bool BuildRequest(PathPlanningRequest newRequest)
    {
        bool goodRequest = true;

        newRequest.UseDistributionMap = true;
        newRequest.UseTaskDifficultyMap = false;
        newRequest.UseHierarchy = true;
        newRequest.UseCoarseToFineSearch = true;
        newRequest.UseParallelProcessing = true;
        newRequest.VehicleType = UAVType.Copter;
        newRequest.DetectionType = DType.FixPercentage;
        newRequest.DetectionRate = 1;
		RtwMatrix m1 = new RtwMatrix(100, 100);
		RtwMatrix m2 = new RtwMatrix(100, 100);
		m1 = ReadInMap(@"C:\Lanny\MAMI\IPPA\Maps\DistMaps\Smoothed_Small_HikerPaulDist.csv");
		m2 = ReadInMap(@"C:\Lanny\MAMI\IPPA\Maps\DiffMaps\Small_HikerPaulDiff.csv");
		ScaleImageValues(ref m1);
		
		newRequest.DistMap = m1;
        newRequest.DiffMap = m2;
        newRequest.UseEndPoint = false;
        newRequest.T = 300;
        newRequest.pStart.column = 0;
        newRequest.pStart.row = 0;
        newRequest.pEnd.column = 0;
        newRequest.pEnd.row = 0;
        newRequest.AlgToUse = AlgType.LHCGWCONV;
        newRequest.DrawPath = false;

        // Find max task-difficulty and compute diff rates only once
        newRequest.MaxDifficulty = 0;
        // Set task-difficulty rates
        double[] DiffRates = new double[newRequest.MaxDifficulty + 1];
        double rate = 1.0 / (newRequest.MaxDifficulty + 1);
        for (int i = 0; i < newRequest.MaxDifficulty + 1; i++)
        {
            DiffRates[i] = 1;
        }
        newRequest.DiffRates = DiffRates;

        // Remember TopN parameter for TopTwo and TopN algorithms
        newRequest.TopN = 3;

        return goodRequest;
    }

    // Construct byte array for Server Queue Item
    private byte[] RequestToByteArray(PathPlanningRequest newRequest)
    {
        byte[] bytes;

        ProtoBuffer.PathPlanningRequest.Builder newPBRequest = ProtoBuffer.PathPlanningRequest.CreateBuilder();
        newPBRequest.SetUseDistributionMap(newRequest.UseDistributionMap)
                  .SetUseTaskDifficultyMap(newRequest.UseTaskDifficultyMap)
                  .SetUseHierarchy(newRequest.UseHierarchy)
                  .SetUseCoarseToFineSearch(newRequest.UseCoarseToFineSearch)
                  .SetUseParallelProcessing(newRequest.UseParallelProcessing)
                  .SetVehicleType((ProtoBuffer.PathPlanningRequest.Types.UAVType)newRequest.VehicleType)
                  .SetDetectionType((ProtoBuffer.PathPlanningRequest.Types.DType)newRequest.DetectionType)
                  .SetDetectionRate(newRequest.DetectionRate)
                  .SetUseEndPoint(newRequest.UseEndPoint)
                  .SetT(newRequest.T)
                  .SetPStart(DistPointToPBDistPoint(newRequest.pStart))
                  .SetPEnd(DistPointToPBDistPoint(newRequest.pEnd))
                  .SetAlgToUse((ProtoBuffer.PathPlanningRequest.Types.AlgType)newRequest.AlgToUse)
                  .SetBatchRun(newRequest.BatchRun)
                  .SetRunTimes(newRequest.RunTimes)
                  .SetMaxDifficulty(newRequest.MaxDifficulty)
                  .SetDrawPath(newRequest.DrawPath)
                  .SetD(newRequest.d)
                  .SetTopNCount(newRequest.TopN);
        // Have to deal with DiffRates array separately
        if (newRequest.DiffRates != null)
        {
            for (int i = 0; i < newRequest.DiffRates.Length; i++)
            {
                newPBRequest.AddDiffRate(newRequest.DiffRates[i]);
            }
        }
        //Here do the matrix thing
        //// Debug Code
        //// Test with small matrix
        //// int d = 40;
        //int d = 100;
        //int v = 1;
        //RtwMatrix testM = new RtwMatrix(d, d);
        //for (int i = 0; i < d; i++)
        //{
        //    for (int j = 0; j < d; j++)
        //    {
        //        testM[i, j] = v;
        //    }
        //}
        //newPBRequest.SetDistMap(RtwMatrixToPBMatrix(testM));
        if (newRequest.DistMap != null)
        {
            newPBRequest.SetDistMap(RtwMatrixToPBMatrix(newRequest.DistMap));
        }
        if (newRequest.DiffMap != null)
        {
            newPBRequest.SetDiffMap(RtwMatrixToPBMatrix(newRequest.DiffMap));
        }
        ProtoBuffer.PathPlanningRequest Request = newPBRequest.Build();
        newRequest = null;
        // Finally the ServerQueueItem
        ProtoBuffer.ServerQueueItem.Builder newServerQueueItem = new ProtoBuffer.ServerQueueItem.Builder();
        newServerQueueItem.SetCallerIP("127.0.0.1")
                          .SetCurRequest(Request);
        ProtoBuffer.ServerQueueItem ServerQueueItem = newServerQueueItem.Build();
        newServerQueueItem = null;
        bytes = ServerQueueItem.ToByteArray();

        return bytes;
    }

    // Method to convert DistPoint to Protocal Buffer version of DistPoint
    private static ProtoBuffer.PathPlanningRequest.Types.DistPoint DistPointToPBDistPoint(DistPoint p)
    {
        ProtoBuffer.PathPlanningRequest.Types.DistPoint.Builder newPBuilder = ProtoBuffer.PathPlanningRequest.Types.DistPoint.CreateBuilder();
        newPBuilder.SetRow(p.row)
                .SetColumn(p.column);
        ProtoBuffer.PathPlanningRequest.Types.DistPoint newP = newPBuilder.Build();
        newPBuilder = null;
        return newP;
    }

    // Method to convert RtwMatrix to Protocal Buffer version of Matrix
    private static ProtoBuffer.PathPlanningRequest.Types.Matrix RtwMatrixToPBMatrix(RtwMatrix m)
    {
        ProtoBuffer.PathPlanningRequest.Types.Matrix.Builder newMBuilder = ProtoBuffer.PathPlanningRequest.Types.Matrix.CreateBuilder();
        for (int i = 0; i < m.Rows; i++)
        {
            ProtoBuffer.PathPlanningRequest.Types.MatrixRow.Builder rowBuilder = ProtoBuffer.PathPlanningRequest.Types.MatrixRow.CreateBuilder();
            for (int j = 0; j < m.Columns; j++)
            {
                rowBuilder.AddCell(m[i, j]);
            }
            ProtoBuffer.PathPlanningRequest.Types.MatrixRow newRow = rowBuilder.Build();
            rowBuilder = null;
            newMBuilder.AddRow(newRow);
        }
        ProtoBuffer.PathPlanningRequest.Types.Matrix newM = newMBuilder.Build();
        newMBuilder = null;
        return newM;
    }

        // Read in a csv file and then store that into matrix
        public static RtwMatrix ReadInMap(string FileInName)
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
	
        // Scale image values between 0 and 255
        public static void ScaleImageValues(ref RtwMatrix imgin)
        {
            float[] MinMax = imgin.MinMaxValue();
            float max = MinMax[1];
            if (max != 0)
            {
                for (int y = 0; y < imgin.Rows; y++)
                {
                    for (int x = 0; x < imgin.Columns; x++)
                    {
                        imgin[y, x] = imgin[y, x] / max * 255;
                    }
                }
            }
            else
            {
                for (int y = 0; y < imgin.Rows; y++)
                {
                    for (int x = 0; x < imgin.Columns; x++)
                    {
                        imgin[y, x] = imgin[y, x] / max * 255;
                    }
                }
            }
        }

}
