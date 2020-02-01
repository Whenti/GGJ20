using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    private enum CameraState { Overall, ToOverall, Game, ToGame };
    private CameraState camera_state;
    
    private int timer;
    private int T_transition;
    private float overall_size;
    private float game_size;
    
    // Start is called before the first frame update
    void Start()
    {
        T_transition = 20;
        overall_size = this.GetComponent<Camera>().orthographicSize;
        game_size = 1;
    }

    private void Initialize()
    {
        this.timer = 0;
        this.camera_state = CameraState.Overall;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.camera_state == CameraState.Game)
        {

        }
        else if (this.camera_state == CameraState.Overall)
        {

        }
        
        else if(this.camera_state == CameraState.ToOverall){
            this.timer -= 1;
            if(this.timer <= 0)
            {
                this.timer = 0;
                this.camera_state = CameraState.Overall;
            }
        }
        else if(this.camera_state == CameraState.ToGame){
            this.timer += 1;
            if(this.timer >= this.T_transition)
            {
                this.timer = this.T_transition;
                this.camera_state = CameraState.Game;
            }
        }
        float lambda = (float)this.timer / (float)this.T_transition;
        this.GetComponent<Camera>().orthographicSize = lambda * game_size + (1 - lambda) * overall_size;
    }

    public void trigger()
    {
        Debug.Log("coucouuuuu");
        switch (this.camera_state)
        {
            case CameraState.Overall:
                this.camera_state = CameraState.ToGame;
                break;
            case CameraState.Game:
                this.camera_state = CameraState.ToOverall;
                break;
            case CameraState.ToGame:
                this.camera_state = CameraState.ToOverall;
                break;
            case CameraState.ToOverall:
                this.camera_state = CameraState.ToGame;
                break;
        }
    }
}
