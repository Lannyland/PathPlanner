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
			Vector3 newMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0f));
			UAV.transform.position += new Vector3(newMousePos.x - curMousePos.x, 0f, newMousePos.z - curMousePos.z);
            curMousePos = newMousePos;
		}
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
		MISCLib.SnapToGrid (ref UAV);
	}
}
