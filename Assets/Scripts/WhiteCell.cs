using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCell : MonoBehaviour {

    SpriteRenderer sr;

    //--------------------------------------------------------------------------------
    //------------------------  MOVEMENT            ----------------------------------
    //--------------------------------------------------------------------------------
    private Vector2 speed;
    private float angle_accel;
    private float speed_norm;

    //--------------------------------------------------------------------------------
    //------------------------  STATE               ----------------------------------
    //--------------------------------------------------------------------------------

    bool is_agressive;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();


        speed_norm = 1f;
        angle_accel = 0.0f;
        Initialize();
    }

    private void Initialize()
    {
        float angle = Random.Range(0f, 360.0f);
        this.speed = new Vector2(speed_norm, 0.0f);
        this.speed = this.speed.Rotate(angle);

        is_agressive = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            this.speed = this.speed.Rotate(180.0f);
            this.GetComponent<Rigidbody2D>().velocity = this.speed;
        }
        if (collision.gameObject.tag == "myeline")
        {
            collision.GetComponent<Myeline>().destroy();
        }
        if (collision.gameObject.tag == "mega_ammo") {
            collision.GetComponent<Ammunition>().destroy();
            neutralize();
        }
        if (collision.gameObject.tag == "ammo") {
            collision.GetComponent<Ammunition>().destroy();
            //cannot neutralize, need a mega_ammo
        }
    }

    // Update is called once per frame
    void Update()
    {
        movementManagement();
        spriteManagement();
    }

    void movementManagement() {

        //update `angle_accel` with `angle_var
        int angle_force = 3;
        float lambda = 0.5f;
        float angle_delta = RandomGaussianGenerator.GenerateNormalRandom(0, 10.0f, -angle_force, angle_force);

        angle_accel = lambda * angle_accel + (1 - lambda) * angle_delta;
        this.speed = this.speed.Rotate(angle_accel);
        this.GetComponent<Rigidbody2D>().velocity = this.speed;
    }

    void spriteManagement() {
        if (is_agressive) {
            sr.color = Color.red;
        } else {
            sr.color = Color.white;
        }
    }

    public void neutralize() {
        is_agressive = false;
    }


}
