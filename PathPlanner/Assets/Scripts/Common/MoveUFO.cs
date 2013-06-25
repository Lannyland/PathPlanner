using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Common;

public class MoveUFO : MonoBehaviour {
	
	public Transform UAV;
    public bool movable = true;

	private bool grabUAV = false;
    private Vector3 curMousePos;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(grabUAV)
		{

            Debug.Log("UAV position = " + UAV.transform.position);
            Debug.Log("distance is = " + (UAV.transform.position.x + UAV.transform.position.z));

            Vector3 newMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0f));
            Vector3 UAVPosOffset = new Vector3(newMousePos.x - curMousePos.x, 0f, newMousePos.z - curMousePos.z);
            // If end point is used
            if (ProjectConstants.endPointCounter > 0)
            {
                if (ValidPoint(UAVPosOffset))
                {
                    UAV.transform.position += UAVPosOffset;
                    curMousePos = newMousePos;
                }
                else
                {
                    return;
                }
            }
            else
            {
                UAV.transform.position += UAVPosOffset;
                curMousePos = newMousePos;
            }
		}
	}

    // Check if current move position is within UAV flight duration range 
    bool ValidPoint(Vector3 offset)
    {
        GameObject go1 = this.gameObject;
        GameObject go2 = null;
        if (go1.name == "UAV")
        {
            go2 = GameObject.Find("EndPoint" + ProjectConstants.endPointCounter);
        }
        else
        {
            go2 = GameObject.Find("UAV");
        }

        Vector3 future = go1.transform.position + offset;

        float distance = MISCLib.ManhattanDistance(future, go2.transform.position);
        if(Convert.ToInt16(Math.Round(distance*10)) > ProjectConstants.durationLeft*30)
        {
            return false;
        }
        if (future.x < -5f || future.x > 4.9f || future.z < -5f || future.z > 4.9f ) 
        { 
            return false;
        }
        return true;
    }
	
	// When mouse button is pressed
	void OnMouseDown()
	{
        if (!movable)
        {
            return;
        }

	    RaycastHit hit;
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.name == this.gameObject.name)
            {
                Debug.Log("name=" + hit.collider.name);
                Debug.Log("point=" + hit.point);
                Debug.Log("pos=" + hit.transform.position);
                grabUAV = true;
            }
            
            // remember cur mouse position
            curMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        }        
	}

	// When mouse button is released
	void OnMouseUp()
	{
		grabUAV = false;
		
		// Snap UAV to the closest vertex
		// MISCLib.SnapToGrid (ref UAV);
	}
}
