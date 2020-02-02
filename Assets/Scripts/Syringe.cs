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
    [SerializeField]
    GameObject left_iris;
    [SerializeField]
    GameObject right_iris;
    [SerializeField]
    GameObject liquid;

    Vector3 left_iris_scale;
    Vector3 right_iris_scale;
    
    public enum SyringeMode {None, Waiting, Moving, Done};

    bool already_gave_life;
    
    public SyringeMode syringe_mode;
    // Start is called before the first frame update
    void Start()
    {
        neck_pos = neck.gameObject.transform.position;
        outside_pos = this.gameObject.transform.position;
        MAGNITUDE = my_camera.getHeight() / 3.0f;
        TIME_TO_NECK = 50;
        TIME_ON_NECK = 50;
        handle_initial_pos = handle.transform.localPosition;
        left_iris_scale = left_iris.transform.localScale;
        right_iris_scale = right_iris.transform.localScale;
        this.Initialize();
    }

    public void Initialize()
    {
        timer = 0;
        syringe_mode = SyringeMode.None;
        left_iris.transform.localScale = left_iris_scale;
        right_iris.transform.localScale = right_iris_scale;
        this.transform.position = outside_pos;
        handle.transform.position = handle_initial_pos;
        already_gave_life = false;
    }

    public void Prepare()
    {
        syringe_mode = SyringeMode.Waiting;
        my_camera.setState(CameraLogic.CameraState.ToOverall) ;
    }
        
    // Update is called once per frame
    public void update_injection()
    {
        if (syringe_mode == SyringeMode.None || syringe_mode == SyringeMode.Done)
            return;
        if (syringe_mode == SyringeMode.Waiting)
        {
            if (my_camera.camera_state == CameraLogic.CameraState.Overall)
            {
                syringe_mode = SyringeMode.Moving;
            }
            else if(my_camera.camera_state == CameraLogic.CameraState.Game)
            {
                syringe_mode = SyringeMode.Done;
            }
            return;
        }
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
            if(timer == TIME_TO_NECK)
            {
                GetComponent<AudioSource>().Play();
            }
            int local_timer = timer - TIME_TO_NECK;
            float lambda = (float)local_timer / (float)this.TIME_ON_NECK;
            this.transform.position = neck_pos;
            float h = handle.GetComponent<Renderer>().bounds.size.y * handle.transform.localScale.y / handle.transform.lossyScale.y * 0.5f;           
            this.handle.transform.localPosition = handle_initial_pos + new Vector3(0f, - lambda * h, 0f);
            left_iris.transform.localScale = left_iris_scale + lambda * 0.8f * left_iris_scale;
            right_iris.transform.localScale = right_iris_scale + lambda * 0.8f * right_iris_scale;
            liquid.transform.localScale = new Vector3(1.0f, 1-lambda, 1.0f);
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -40.0f);

            if (!already_gave_life) {
                already_gave_life = true;
                GameObject.Find("MainGame").GetComponent<MainGame>().addHealth(50);
            }
        }
        else if(timer < TIME_TO_NECK * 2 + TIME_ON_NECK)
        {
            int local_timer = timer - TIME_TO_NECK - TIME_ON_NECK;
            local_timer = TIME_TO_NECK - local_timer;

            float lambda = (float)local_timer / (float)this.TIME_TO_NECK;

            this.transform.position = lambda * neck_pos + (1 - lambda) * outside_pos;
            this.setAmplitude(lambda);
        }
        else
        {
            syringe_mode = SyringeMode.Waiting;
            my_camera.setState(CameraLogic.CameraState.ToGame);
        }
    }

    void setAmplitude(float lambda)
    {
        float magnitude = MAGNITUDE * lambda * (1 - lambda) * 4.0f;
        this.transform.position += new Vector3(0.0f, magnitude, 0.0f);
    }
}
