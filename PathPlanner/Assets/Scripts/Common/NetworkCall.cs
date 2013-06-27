using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using rtwmatrix;

namespace TCPIPTest
{
	public class NetworkCall
	{
		#region Members
		
		// public variables
		public double efficiency;
		public double runTime;
		public Vector2[] path;
		public bool callSuccess = true;
		public string message = "";
		
		// private variables
		RtwMatrix DistMap;
		RtwMatrix DiffMap;
		Vector2 start;
		Vector2 end;
		bool useDiffMap;
		bool useEndPoint;
		bool plentyTime;
		int T;
		
		#endregion
		
		#region Constructor, Destructor

        // Constructor
        public NetworkCall(RtwMatrix _distMap, RtwMatrix _diffMap, 
			Vector2 _start, Vector2 _end, 
			bool _useDiffMap, bool _useEndPoint, bool _plentyTime,
			int _T)
        {
			DistMap = _distMap.Clone();
			DiffMap = _diffMap.Clone();
			start = _start * 10 + new Vector2(50, 50);
            end = _end * 10 + new Vector2(50, 50);
			useDiffMap = _useDiffMap;
			useEndPoint = _useEndPoint;
			plentyTime = _plentyTime;
			T = _T;
			MakeCall();			
        }

        // Destructor
        ~NetworkCall()
        {
            // Cleaning up
            DistMap = null;
            DiffMap = null;
        }

        #endregion
                
        #region functions
		
		// Testing client server call to path planning server IPPA
		void MakeCall()
		{
		    // Generate path planning requests
	        PathPlanningRequest newRequest = new PathPlanningRequest();
			BuildRequest(ref newRequest);
	        
	        // Build Path Request into Object
	        // Object via protocol buffer
	        byte[] outStream = RequestToByteArray(newRequest);
	
	        // Add header to indicate data size
	        byte[] outStreamFinal = ProtoBuffer.TCPHandler.AddDataSizeHeader(outStream);
	        
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
					message = "Server exception occurred. No data received!";
	            }
	            else
	            {
	                // Debug.Log("Efficiency = " + BitConverter.ToDouble(inStream, 0) + "\n");
	                // Debug.Log("Run Time = " + BitConverter.ToDouble(inStream, 8) + "\n");
					efficiency = BitConverter.ToDouble(inStream, 0);
	                runTime = BitConverter.ToDouble(inStream, 8);
	
	                path = new Vector2[(inStream.Length - 16)/8];
	                // Debug.Log("Path length = " + path.Length);
	                for (int i = 16; i < inStream.Length; i = i + 4)
	                {
	                    // Debug.Log(BitConverter.ToInt32(inStream, i).ToString() + " ");
	                    if ((i / 4) % 2 == 1)
	                    {
	                        // Debug.Log(", ");
							path[(i-16)/8].y = BitConverter.ToInt32(inStream, i);							
	                    }
						else
						{
							path[(i-16)/8].x = BitConverter.ToInt32(inStream, i);							
						}
	                }
	            }
	        }
	        catch (System.Net.Sockets.SocketException ex)
	        {
	            if (ex.ErrorCode == 10061)
	            {
	                Debug.Log("Server not available!");
		            callSuccess = false;
					message = "Server not available!";					
	            }
	            else
	            {
	                Debug.Log(ex.Message);
		            callSuccess = false;
					message = ex.Message;					
	            }                
	        }
		}		
		
		// Building the path planning request object and does sanity check
	    private void BuildRequest(ref PathPlanningRequest newRequest)
	    {
	        newRequest.UseDistributionMap = true;
	        newRequest.UseTaskDifficultyMap = useDiffMap;
	        newRequest.UseHierarchy = true;
	        newRequest.UseCoarseToFineSearch = true;
	        newRequest.UseParallelProcessing = true;
	        newRequest.VehicleType = UAVType.Copter;
	        newRequest.DetectionType = DType.FixPercentage;
	        newRequest.DetectionRate = 1;
			newRequest.DistMap = DistMap;
	        newRequest.DiffMap = DiffMap;
	        newRequest.UseEndPoint = useEndPoint;
	        newRequest.T = T;
	        newRequest.pStart.column = Convert.ToInt16(Math.Round(start.x));
	        newRequest.pStart.row = Convert.ToInt16(Math.Round(start.y));
			if(useEndPoint)
			{				
				newRequest.pEnd.column = Convert.ToInt16(Math.Round(end.x));
		        newRequest.pEnd.row = Convert.ToInt16(Math.Round(end.y));
		        if(plentyTime)
				{
					newRequest.AlgToUse = AlgType.EA_E;
				}
				else
				{
					newRequest.AlgToUse = AlgType.LHCGWCONV_E;
				}
			}
			else
			{
				newRequest.pEnd.column = 0;
		        newRequest.pEnd.row = 0;			
		        if(plentyTime)
				{
					newRequest.AlgToUse = AlgType.EA;
				}
				else
				{
					newRequest.AlgToUse = AlgType.LHCGWCONV;
				}
			}			
	        newRequest.DrawPath = false;
	
	        // Find max task-difficulty and compute diff rates only once
            if (useDiffMap)
            {
                newRequest.MaxDifficulty = Convert.ToInt32(DiffMap.MinMaxValue()[1]);
                // Set task-difficulty rates
                double[] DiffRates = new double[newRequest.MaxDifficulty + 1];
                double rate = 1.0 / (newRequest.MaxDifficulty + 1);
                for (int i = 0; i < newRequest.MaxDifficulty + 1; i++)
                {
                    DiffRates[i] = 1 - i * rate;
                }
                newRequest.DiffRates = DiffRates;
            }
	
	        // Remember TopN parameter for TopTwo and TopN algorithms
	        newRequest.TopN = 5;
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
	
		#endregion
	}
}