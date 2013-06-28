using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;

public class Flypath : MonoBehaviour {
	
	public bool fly = false;
	public Transform UAV;
	public float speed = 50;
	public Vector2[] path;
	public float maxDiff = 0f;
		
	private int currentWayPoint;
	private Mesh diffMesh;			
	private Mesh distMesh;			
	private Vector3[] diffVertices;
	private Vector3[] distVertices;
	private Color[] distColors;		
	
	
	// Use this for initialization
	void Start () {
		currentWayPoint = 1;	
		diffMesh = GameObject.Find("PlaneDiff").GetComponent<MeshFilter>().mesh;			
		distMesh = GameObject.Find("Plane").GetComponent<MeshFilter>().mesh;			
		diffVertices = diffMesh.vertices;
		distVertices = distMesh.vertices;
		distColors = distMesh.colors;				
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate() {
		if(currentWayPoint < path.Length)
		{
			// Keep checking those waypoints
			Vector3 target = new Vector3(path[currentWayPoint].x/10f-5f, 4.0f, path[currentWayPoint].y/10f-5f);
			Vector3 moveDirection = target - UAV.position;
			
			if(moveDirection.magnitude<0.01)
			{
				// Debug.Log("Waypoint " + currentWayPoint + " checked.");
				// Change the vertex height of the waypoint to simulate vacuum effect
				int index = Convert.ToInt16(path[currentWayPoint].y * ProjectConstants.intMapWidth + path[currentWayPoint].x);
				float v = PointVacuum (index);
					
				// Give yourself some game points!
				IncreasingScoreEffect effect = Camera.main.GetComponent<IncreasingScoreEffect>();
				effect.style = IncreasingScoreEffect.EffectStyle.MileStone;
				effect.curScore += v;
				
				// Ready for next waypoint
				currentWayPoint++;
			}
			UAV.Translate(moveDirection*speed*Time.deltaTime);
		}		
	}
	
	float PointVacuum(int i)
	{
		if(maxDiff == 0f)
		{
			diffVertices[i].y = 1;
		}
		else
		{
	        diffVertices[i].y = (maxDiff + 1 - diffVertices[i].y) * (1.0f / (maxDiff + 1));
		}
		
		float v = distVertices[i].y * diffVertices[i].y;
		distVertices[i].y -= v;
		
		distColors[i] = MISCLib.HeightToDistColor(distVertices[i].y, 4f);
		
		distMesh.vertices = distVertices;
		distMesh.colors = distColors;
		distMesh.RecalculateNormals();
		distMesh.RecalculateBounds();
		return v;
	}
}
