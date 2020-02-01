using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //--------------------------------------------------------------------------------
    //------------------------  MOVEMENTS           ----------------------------------
    //--------------------------------------------------------------------------------

    float speed_max = 10;
    float acceleration_max = 100;

    Vector3 speed;
    Vector3 acceleration;

    bool up, down, left, right;


    //--------------------------------------------------------------------------------
    //------------------------  SHOOT               ----------------------------------
    //--------------------------------------------------------------------------------

    [SerializeField] Ammunition ammoPrefab;
    [SerializeField] Ammunition megaAmmoPrefab;
    int ammo;
    int ammo_max;

    Vector3 aim_direction;
    float distance_aim = 0.4f;

    bool hold_blast;
    float timer_hold;
    float duration_hold = 0.4f;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        inputsManagement();
        movementManagement();
        aimManagement();
        blastManagement();
    }

    public void Initialize() {
        up = false;
        down = false;
        right = false;
        left = false;

        ammo_max = 50;
        ammo = ammo_max;
        timer_hold = 0;
        hold_blast = false;

    }

    void movementManagement() {
        speed = speed*0.95f;
        if (speed.magnitude < 0.00001f) {
            speed = Vector2.zero;
        }

        Vector2 addAccel = new Vector2(0,0);

        if (left) {
            addAccel += new Vector2(-1, 0);
        }
        if (right) {
            addAccel += new Vector2(1, 0);
        }
        if (up) {
            addAccel += new Vector2(0, 1);
        }
        if (down) {
            addAccel += new Vector2(0, -1);
        }

        acceleration = addAccel.normalized * acceleration_max;

        speed += acceleration * Time.deltaTime;

        if (speed.magnitude >= speed_max) {
            speed = speed.normalized * speed_max;
        }

        transform.position += speed * Time.deltaTime;

    }

    void aimManagement() {
        aim_direction = new Vector3(1, 0, 0);

        Vector2 mouseScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 posScreen = transform.position;

        Vector2 diff = (mouseScreen-posScreen);

        aim_direction = diff;
    }

    void shoot() {
        if (ammo > 0) {
            
            ammo--;



            if (timer_hold >= duration_hold) {
                //mega ammo

                Ammunition a = Ammunition.Instantiate(megaAmmoPrefab);
                a.transform.position = transform.position+(aim_direction).normalized*distance_aim;
                a.setDirection(aim_direction);
            } else {
                //regular ammo

                Ammunition a = Ammunition.Instantiate(ammoPrefab);
                a.transform.position = transform.position + (aim_direction).normalized * distance_aim;
                a.setDirection(aim_direction);
            }

            timer_hold = 0;
            hold_blast = false;

        } else {
            //cannot shoot !
        }
    }

    void blastManagement() {
        Debug.Log(" Timer : " + timer_hold);
        if (hold_blast) {
            timer_hold += Time.deltaTime;
        }
    }

    public void addAmmo(int num) {
        ammo += num;
        ammo = Mathf.Max(0, Mathf.Min(ammo_max, ammo));
    }

    void inputsManagement() {
        //KEY DOWN
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            left = true;
        } else {
            left = false;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            right = true;
        } else {
            right = false;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            down = true;
        } else {
            down = false;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            up = true;
        } else {
            up = false;
        }
            
        if (Input.GetMouseButtonDown(0)) {
            hold_blast = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            shoot();
        }
    }
}
