using UnityEngine;
using System.Collections;

public class SetAppendMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		// Based on who is clicked, set diff level accordingly
		// Also set projector material for brush color
	    GameObject go = GameObject.Find("MaterialHolder");
		Projector projector = GameObject.Find("BrushProjector").GetComponent<Projector>();
        PaintSurface ps = Camera.main.GetComponent<PaintSurface>();

		switch (this.gameObject.name)
		{
		case "chkErase":
			Assets.Scripts.ProjectConstants.appendMode = 1;
			projector.material = go.GetComponent<MaterialCatelog>().catelog[0];
            ps.editMode = PaintSurface.EditMode.Erase;
            ps.fallOff = PaintSurface.FallOff.Disc;
			break;
		case "chkIncrease":
			Assets.Scripts.ProjectConstants.appendMode = 2;
			projector.material = go.GetComponent<MaterialCatelog>().catelog[1];
            ps.editMode = PaintSurface.EditMode.Raise;
            ps.fallOff = PaintSurface.FallOff.Gauss;
			break;
		case "chkDecrease":
			Assets.Scripts.ProjectConstants.appendMode = 3;
			projector.material = go.GetComponent<MaterialCatelog>().catelog[1];
            ps.editMode = PaintSurface.EditMode.Lower;
            ps.fallOff = PaintSurface.FallOff.Gauss;
			break;
		}
	}
}
