using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCell : MonoBehaviour {

    SpriteRenderer sr;


    //--------------------------------------------------------------------------------
    //------------------------  ANIMATION            ----------------------------------
    //--------------------------------------------------------------------------------

    List<Sprite> sprites_idle_neutral;
    List<Sprite> sprites_idle_agressive;

    Animation2D anim_idle_neutral;
    Animation2D anim_idle_agressive;


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

        //load sprites
        sprites_idle_neutral = new List<Sprite>();
        sprites_idle_agressive = new List<Sprite>();

        for (int i = 0; i < 50; ++i) {
            string txt = i + "";
            if (txt.Length == 1) txt = "0000" + txt;
            else if (txt.Length == 2) txt = "000" + txt;
            sprites_idle_neutral.Add(Resources.Load<Sprite>("Sprites/ennemies/lymphoBlanc/lymphoBlanc_" + txt));
        }

        for (int i = 0; i < 49; ++i) {
            string txt = i + "";
            if (txt.Length == 1) txt = "0000" + txt;
            else if (txt.Length == 2) txt = "000" + txt;
            sprites_idle_agressive.Add(Resources.Load<Sprite>("Sprites/ennemies/lymphoRouge/lymphoRouge_" + txt ));
        }

        anim_idle_neutral = new Animation2D("Idle Neutral", sprites_idle_neutral, 1.0f, true);
        anim_idle_agressive = new Animation2D("Idle Agressive", sprites_idle_agressive, 1.0f, true);


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


    // Update is called once per frame
    void Update() {
        movementManagement();
        animationsManagement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            this.speed = this.speed.Rotate(180.0f);
            this.GetComponent<Rigidbody2D>().velocity = this.speed;
        }
        if (collision.gameObject.tag == "myeline" && is_agressive)
        {
            collision.GetComponent<Myeline>().destroy();
        }
        if (collision.gameObject.tag == "mega_ammo" && collision.GetComponent<Ammunition>().isShot()) {
            collision.GetComponent<Ammunition>().destroy();
            neutralize();
        }
        if (collision.gameObject.tag == "ammo" && collision.GetComponent<Ammunition>().isShot()) {
            collision.GetComponent<Ammunition>().destroy();
            //cannot neutralize, need a mega_ammo
        }
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
    

    public void neutralize() {
        is_agressive = false;
    }

    void animationsManagement() {
        if (is_agressive) {
            anim_idle_agressive.evolve(1.0f);
            sr.sprite = anim_idle_agressive.currentSprite();
        } else {
            anim_idle_neutral.evolve(1.0f);
            sr.sprite = anim_idle_neutral.currentSprite();
        }
    }


}
