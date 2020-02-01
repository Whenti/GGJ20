using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    MainGame mainGame;

    //--------------------------------------------------------------------------------
    //------------------------  ANIMATIONS          ----------------------------------
    //--------------------------------------------------------------------------------

    [SerializeField] SpriteRenderer sr;
    [SerializeField] List<Sprite> sprites_idle;
    [SerializeField] List<Sprite> sprites_shoot;

    bool is_shooting;

    Animation2D anim_idle;
    Animation2D anim_shoot;

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

    public List<string> ammo;
    int ammo_max;

    Vector3 aim_direction;
    float distance_aim = 0.4f;

    bool hold_blast;
    float timer_hold;
    float duration_hold = 0.4f;


    // Start is called before the first frame update
    void Start()
    {
        mainGame = GameObject.Find("MainGame").GetComponent<MainGame>();

        anim_idle = new Animation2D("Idle", sprites_idle, 2.0f, true);
        anim_shoot = new Animation2D("Shoot", sprites_shoot, 0.14f, false);

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (mainGame.on_game) {
            inputsManagement();
            movementManagement();
            aimManagement();
            blastManagement();
        }
        animationManagement();
    }

    public void Initialize() {
        up = false;
        down = false;
        right = false;
        left = false;

        ammo_max = 9;
        ammo = new List<string>();
        ammo.Clear();
        timer_hold = 0;
        hold_blast = false;

        is_shooting = false;

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
        if (ammo.Count > 0) {
            
            //if (timer_hold >= duration_hold) {
            if(ammo[ammo.Count-1]=="mega_ammo"){
                //mega ammo

                Ammunition a = Ammunition.Instantiate(megaAmmoPrefab);
                a.transform.SetParent(mainGame.Ammunitions.transform, false);
                a.transform.position = transform.position+(aim_direction).normalized*distance_aim;
                a.setDirection(aim_direction);
                a.setShot(true);
            } else if(ammo[ammo.Count - 1] == "ammo") {
                //regular ammo

                Ammunition a = Ammunition.Instantiate(ammoPrefab);
                a.transform.SetParent(mainGame.Ammunitions.transform, false);
                a.transform.position = transform.position + (aim_direction).normalized * distance_aim;
                a.setDirection(aim_direction);
                a.setShot(true);
            }

            is_shooting = true;
            anim_shoot.start();

            ammo.RemoveAt(ammo.Count - 1);

            timer_hold = 0;
            hold_blast = false;

        } else {
            //cannot shoot ! no ammo
        }
    }

    void blastManagement() {
        if (hold_blast) {
            timer_hold += Time.deltaTime;
        }
    }

    public void addAmmo(string type) {
        if (ammo.Count < ammo_max) {
            ammo.Add(type);
        }
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

    void animationManagement() {
        if (is_shooting) {
            if (anim_shoot.evolve(1.0f)) {
                is_shooting = false;
            }
            sr.sprite = anim_shoot.currentSprite();
        } else {
            anim_idle.evolve(1.0f);
            sr.sprite = anim_idle.currentSprite();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (ammo.Count<ammo_max && collision.gameObject.tag == "mega_ammo" && !collision.GetComponent<Ammunition>().isShot()) {
            collision.GetComponent<Ammunition>().destroy();
            addAmmo("mega_ammo");
        }
        if (ammo.Count < ammo_max && collision.gameObject.tag == "ammo" && !collision.GetComponent<Ammunition>().isShot()) {
            collision.GetComponent<Ammunition>().destroy();
            addAmmo("ammo");
        }
    }
}
