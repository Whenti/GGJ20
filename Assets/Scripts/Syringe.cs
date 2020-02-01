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
    [SerializeField]
    GameObject handle;
    private Vector3 handle_initial_pos;

    // Start is called before the first frame update
    void Start()
    {
        timer = 1;
        neck_pos = neck.gameObject.transform.position;
        outside_pos = this.gameObject.transform.position;
        MAGNITUDE = my_camera.getHeight() / 3.0f;
        TIME_TO_NECK = 50;
        TIME_ON_NECK = 150;
        handle_initial_pos = handle.gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < 0) { return; }
        timer += 1;
        if(timer < TIME_TO_NECK)
        {
            float lambda = (float)this.timer / (float)this.TIME_TO_NECK;

            this.transform.position = lambda * neck_pos + (1 - lambda) * outside_pos;
            this.handle.transform.localPosition = handle_initial_pos;
            this.setAmplitude(lambda);
        }
        else if(timer < TIME_TO_NECK + TIME_ON_NECK)
        {
            int local_timer = timer - TIME_TO_NECK;
            float lambda = (float)local_timer / (float)this.TIME_ON_NECK;
            this.transform.position = neck_pos;
            float h = handle.GetComponent<Renderer>().bounds.size.y * handle.transform.localScale.y / handle.transform.lossyScale.y;           
            this.handle.transform.localPosition = handle_initial_pos + new Vector3(0f, - lambda * h, 0f); 
        }
        else if(timer < TIME_TO_NECK * 2 + TIME_ON_NECK)
        {
            int local_timer = timer - TIME_TO_NECK - TIME_ON_NECK;
            local_timer = TIME_TO_NECK - local_timer;

            float lambda = (float)local_timer / (float)this.TIME_TO_NECK;

            this.transform.position = lambda * neck_pos + (1 - lambda) * outside_pos;
            this.setAmplitude(lambda);
        }
    }

    void setAmplitude(float lambda)
    {
        float magnitude = MAGNITUDE * lambda * (1 - lambda) * 4.0f;
        this.transform.position += new Vector3(0.0f, magnitude, 0.0f);
    }
}
