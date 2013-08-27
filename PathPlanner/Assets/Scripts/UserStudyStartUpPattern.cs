using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class UserStudyStartUpPattern : UserStudyStartUpChores
{

    // Use this for initialization
    void Start()
    {
        base.Start();

        int timer = ProjectConstants.durations[ProjectConstants.pageIndex] * 60;
        int second = timer % 60;
        int minute = timer / 60;
        GameObject.Find("lblFlightTime").GetComponent<UILabel>().text = minute.ToString() + ":" + second.ToString("00");

        GameObject.Find("CameraGlobal").GetComponent<Camera>().enabled = false;
        GameObject.Find("CameraFree").GetComponent<Camera>().enabled = true;
        GameObject.Find("CameraBirdEye").GetComponent<Camera>().enabled = false;
        GameObject.Find("CameraBehind").GetComponent<Camera>().enabled = false;

        curCam = GameObject.Find("CameraFree").GetComponent<Camera>();

        GameObject.Find("btnFly").GetComponent<UIButton>().isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
