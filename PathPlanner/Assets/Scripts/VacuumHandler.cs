using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Common;
using rtwmatrix;

public class VacuumHandler {
	
	private Vector2[] path;	
	private Vector3[] distVertices;
	private Vector3[] diffVertices;
	private Color[] colors;
	private float firstVacuum;
	private float CDF;
	
	// Contructor
	public VacuumHandler(Vector2[] _path, Vector3[] _distMapVertices, Vector3[] _diffMapVertices)
	{
		path = _path;
		distVertices = _distMapVertices;
		diffVertices = _diffMapVertices;
		CDF = 0.0f;
		Vacuum();
	}
	
	// Destructor
	~VacuumHandler()
	{
		path = null;
		distVertices = null;
		diffVertices = null;
	}
	
	// Method to create a 3D line.
	public void Vacuum()
	{
		// Basically fly the UAV following the path and vacuum based on distmap and diffmap
		
		// Get diff map max
		float maxDiff = 0f;
		for(int i=0; i<diffVertices.Length; i++)
		{
			if(maxDiff < diffVertices[i].y)
			{
				maxDiff = diffVertices[i].y;
			}
		}

		if(maxDiff < 0.001f)
		{
			// No Diff map is used
			maxDiff = 0f;
		}

		// Just use the Vector3 array to set probability of detection	
		for(int i=0; i<diffVertices.Length; i++)
		{
			if(maxDiff == 0f)
			{
				diffVertices[i].y = 1;
			}
			else
			{
                diffVertices[i].y = (maxDiff + 1 - diffVertices[i].y) * (1.0f / (maxDiff + 1));
			}
		}
		
		// Now fly path and change vertices heights
		for(int i=0; i<path.Length; i++)
		{
			int index = Convert.ToInt16(path[i].y * ProjectConstants.intMapWidth + path[i].x);
			if(i==0)
			{
				firstVacuum = distVertices[index].y * diffVertices[index].y;
			}
			CDF += distVertices[index].y * diffVertices[index].y;
			distVertices[index].y -= distVertices[index].y * diffVertices[index].y;
		}
		
		// Set color for these vertices
		colors = MISCLib.ApplyDistColorMap(distVertices);
	}	
	
	public Vector3[] GetVertices()
	{
		return distVertices;
	}
	
	public Color[] GetColors()
	{
		return colors;
	}
	
	public float GetFirstVacuum()
	{
		return firstVacuum;
	}
	
	public float GetCDF()
	{
		return CDF;
	}
}