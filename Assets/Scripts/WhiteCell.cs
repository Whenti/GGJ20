using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCell : MonoBehaviour
{
    private Vector2 speed;
    private float angle_accel;
    private float speed_norm;
    // Start is called before the first frame update
    void Start()
    {
        speed_norm = 1f;
        angle_accel = 0.0f;
        Initialize();
    }

    private void Initialize()
    {
        float angle = Random.Range(0f, 360.0f);
        this.speed = new Vector2(speed_norm, 0.0f);
        this.speed = this.speed.Rotate(angle);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("oh oui");
        if (collision.gameObject.tag == "obstacle")
        {
            this.speed = this.speed.Rotate(180.0f);
            this.GetComponent<Rigidbody2D>().velocity = this.speed;
        }
        if (collision.gameObject.tag == "myeline")
        {
            //collision.GetComponent<Myeline>().destroy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //update `angle_accel` with `angle_var
        int angle_force = 3;
        float lambda = 0.5f;
        float angle_delta = RandomGaussianGenerator.GenerateNormalRandom(0, 10.0f, -angle_force, angle_force);

        angle_accel = lambda * angle_accel + (1 - lambda) * angle_delta;
        this.speed = this.speed.Rotate(angle_accel);
        this.GetComponent<Rigidbody2D>().velocity = this.speed;
    }
}
