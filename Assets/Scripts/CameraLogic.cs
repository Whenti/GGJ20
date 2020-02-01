using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    public enum CameraState { Overall, ToOverall, Game, ToGame };
    public CameraState camera_state;
    
    private int timer;
    private int T_transition;
    private float overall_size;
    private Vector3 overall_pos;
    private float game_size;

    [SerializeField]
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        T_transition = 80;
        overall_size = this.GetComponent<Camera>().orthographicSize;
        overall_pos = this.gameObject.transform.position;
        game_size = 5f;
    }

    private void Initialize()
    {
        this.timer = 0;
        this.camera_state = CameraState.Overall;
    }

    // Update is called once per frame
    public void update_camera()
    {
        if (this.camera_state == CameraState.Game)
        {
            transform.position = player.transform.position;
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
        Vector3 player_pos = player.transform.position;
        this.gameObject.transform.position = lambda * new Vector3(player_pos.x, player_pos.y, overall_pos.z) + (1 - lambda) * overall_pos;
    }

    public float getOverallSize()
    {
        return this.overall_size;
    }

    public float getHeight()
    {
        return 2f * this.GetComponent<Camera>().orthographicSize * this.GetComponent<Camera>().aspect;
    }

    public void setState(CameraState state)
    {
        this.camera_state = state;
    }

    public void trigger()
    {
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
