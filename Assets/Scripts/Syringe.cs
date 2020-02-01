using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour
{
    private int timer;
    private int TIME_TO_NECK;
    private int TIME_ON_NECK;
    private float MAGNITUDE;
    private Vector3 neck_pos;
    private Vector3 outside_pos;
    [SerializeField]
    GameObject neck;
    [SerializeField]
    CameraLogic my_camera;

    // Start is called before the first frame update
    void Start()
    {
        timer = -1;
        neck_pos = neck.gameObject.transform.position;
        outside_pos = new Vector3(neck_pos.x + my_camera.getOverallSize(), neck_pos.y, neck_pos.z);
        MAGNITUDE = my_camera.getOverallSize() / 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < 0) { return; } 
        else if(timer < TIME_TO_NECK)
        {
            int t = timer;
            float lambda = (float)this.timer / (float)this.TIME_ON_NECK;
            //this.transform.position = 
        }
    }
}
