using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{

    MainGame mainGame;
    [SerializeField] Particule particulePrefab;

    bool shot=false;

    public bool is_tuto { get; set; }

    Vector3 direction;
    float speed = 0.1f;
    Vector2 speedVector;
    private float angle_accel;
    private float speed_norm;

    float seed;

    float timer;
    float duration = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainGame = GameObject.Find("MainGame").GetComponent<MainGame>();
        timer = 0;
        seed = Random.value * 1000;


        speed_norm = 1f;
        angle_accel = 0.0f;
        float angle = Random.Range(0f, 360.0f);
        this.speedVector = new Vector2(speed_norm, 0.0f);
        this.speedVector = this.speedVector.Rotate(angle);
    }

    // Update is called once per frame
    void Update()
    {
        movementManagement();
        destructionManagement();
    }

    public void setDirection(Vector3 dir) {
        direction = dir;
    }

    void movementManagement() {

        if (shot) {
            transform.position += direction.normalized * speed;
        } else {
            float freq_small = 3.0f;
            float radius_small = 0.001f;

            float freq_big = 1.0f;
            float radius_big = 0.01f;

            transform.position += new Vector3(Mathf.Cos(timer*freq_small*1.30289f+seed),
                                               Mathf.Sin(timer* freq_small * 1.02308f+seed),
                                               0)*radius_small+
                                new Vector3(    Mathf.Cos(timer*freq_big*1.4329f+seed),
                                                Mathf.Sin(timer*freq_big*1.0389f+seed),
                                                0)*radius_big;


            //update `angle_accel` with `angle_var
            int angle_force = 3;
            float lambda = 0.5f;
            float angle_delta = RandomGaussianGenerator.GenerateNormalRandom(0, 10.0f, -angle_force, angle_force);

            angle_accel = lambda * angle_accel + (1 - lambda) * angle_delta;
            this.speedVector = this.speedVector.Rotate(angle_accel);
            this.GetComponent<Rigidbody2D>().velocity = this.speedVector;
        }
    }

    void destructionManagement() {
        if (shot) {
            timer += Time.deltaTime;

            if (timer >= duration) {
                destroy();
            }
        } else {
            timer += Time.deltaTime;
        }
    }

    public void setShot(bool b) {
        shot = b;
        timer = 0;
    }

    public void destroy() {

        if (shot) {
            for (int i = 0; i < 5; ++i) {
                //create particule

                Particule p = Particule.Instantiate(particulePrefab);
                p.transform.SetParent(mainGame.Particules.transform, false);
                p.transform.position = transform.position;
                p.transform.rotation = Quaternion.Euler(0, 0, Random.value * 10);

            }
        }

        Destroy(gameObject);
    }

    public bool isShot() {
        return shot;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "obstacle") {
            this.speedVector = speedVector.Rotate(180.0f);
            this.GetComponent<Rigidbody2D>().velocity = speedVector;
        }
    }
}
