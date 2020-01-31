using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{

    private enum TongueState { Down, Up, MovingDown, MovingUp };
    private TongueState tongue_state;
    private Vector3 initial_pos;
    private Vector3 final_pos;
    private int T;
    private int timer;

    // Start is called before the first frame update
    void Start()
    {
        this.initial_pos = this.transform.position;
        Vector2 size = this.GetComponent<Renderer>().bounds.size;
        this.final_pos = new Vector3(this.initial_pos.x, this.initial_pos.y - size.y, this.initial_pos.z);
        this.T = 30;
        this.Initialize(); 
    }

    void Initialize()
    {
        this.timer = 0;
        this.transform.position = initial_pos;
        this.tongue_state = TongueState.Up;
    }

    public void trigger()
    {
        switch (this.tongue_state)
        {
            case TongueState.MovingUp:
                this.tongue_state = TongueState.MovingDown;
                break;
            case TongueState.MovingDown:
                this.tongue_state = TongueState.MovingUp;
                break;
            case TongueState.Down:
                this.tongue_state = TongueState.MovingUp;
                break;
            case TongueState.Up:
                this.tongue_state = TongueState.MovingDown;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.tongue_state == TongueState.Up || this.tongue_state == TongueState.Down)
            return;
        
        if(this.tongue_state == TongueState.MovingUp){
            this.timer -= 1;
            if(this.timer <= 0)
            {
                this.timer = 0;
                this.tongue_state = TongueState.Up;
            }
        }
        else if(this.tongue_state == TongueState.MovingDown){
            this.timer += 1;
            if(this.timer >= this.T)
            {
                this.timer = this.T;
                this.tongue_state = TongueState.Down;
            }
        }
        float lambda = (float)this.timer / (float)this.T;
        this.transform.position = (1 - lambda) * this.initial_pos + lambda * this.final_pos;
    }
}
