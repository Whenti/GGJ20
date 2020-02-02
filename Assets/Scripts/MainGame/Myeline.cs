using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myeline : MonoBehaviour
{

    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject myelines;

    //--------------------------------------------------------------------------------
    //------------------------  STATE               ----------------------------------
    //--------------------------------------------------------------------------------

    [SerializeField] bool is_incassable;
    public enum State { active, electric, destructed, invisible};
    State state;

    [SerializeField] Sprite sprite_active;
    [SerializeField] Sprite sprite_electric;
    [SerializeField] Sprite sprite_destructed;

    float timer_invisible;//this timer represents the fact that if the myeline is "destructed" more than a certain amount of time, then it becomes invisible
    float duration_invisible=5.0f;


    //--------------------------------------------------------------------------------
    //------------------------  ELECTRICITY           --------------------------------
    //--------------------------------------------------------------------------------
    
    bool can_give_electric;

    float timer_electricity;
    float duration_electricity = 0.2f;
    float limite_give_electricity = 2.0f;
    float limite_give_electricity_neurone = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();

        Initialize();
    }

    public void Initialize() {
        state = State.active;
        can_give_electric = true;
        timer_electricity = 0;
        timer_invisible = 0;
    }

    // Update is called once per frame
    void Update()
    {
        electricityManagement();
        //invisibleManagement();
    }

    public void repair() {
        if (isDestructed()) {
            state = State.active;
            timer_invisible = 0;
        } else {
            //cannot repair, need a mega blast
        }
    }

    public void mega_repair() {
        if (isDestructed() || isInvisible()) {
            state = State.active;
            timer_invisible = 0;
        }
    }

    void electricityManagement() {
        if (isElectric()) {
            timer_electricity += Time.deltaTime;

            if (can_give_electric && timer_electricity > duration_electricity / 3.0f) {
                giveElectricity();
                can_give_electric = false;
            }

            if (timer_electricity >= duration_electricity) {
                state = State.active;
                can_give_electric = true;
                timer_electricity = 0;
            }

            sr.sprite = sprite_electric;
            sr.gameObject.SetActive(true);
        } else if (state == State.destructed) {
            sr.sprite = sprite_destructed;
            sr.gameObject.SetActive(true);
        } else if (state == State.invisible) {
            sr.gameObject.SetActive(false);
        } else if (state == State.active) {
            sr.sprite = sprite_active;
            sr.gameObject.SetActive(true);
        }
    }

    public void receiveElectricity() {
        if (isActive()) {
            state = State.electric;
        }
        

    }

    public void giveElectricity() {
        foreach (Transform t in myelines.transform) {
            if (t.tag == "myeline") {
                if (t.gameObject != gameObject && t.GetComponent<Myeline>().isActive() && (t.position - transform.position).magnitude <= limite_give_electricity) {
                    t.GetComponent<Myeline>().receiveElectricity();
                }
            } else if (t.tag == "neurone") {
                if (t.gameObject != gameObject && t.GetComponent<Neurone>().isActive() && (t.position - transform.position).magnitude <= limite_give_electricity_neurone) {
                    t.GetComponent<Neurone>().receiveElectricity();
                }
            }
        }
    }

    void invisibleManagement() {
        if (isDestructed()) {
            timer_invisible += Time.deltaTime;

            if (timer_invisible >= duration_invisible) {
                state = State.invisible;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (isDestructed()) {
            if (collision.gameObject.tag == "ammo" && collision.GetComponent<Ammunition>().isShot()) {
                collision.GetComponent<Ammunition>().destroy();

                repair();


            }/*else if (collision.gameObject.tag == "mega_ammo" && collision.GetComponent<Ammunition>().isShot()) {
            //collision.GetComponent<Ammunition>().destroy();

            //mega_repair();
            
            
        }*/
        }
    }

    public void destroy() {
        state = State.destructed;
    }

    public bool isActive() { return state == State.active; }
    public bool isElectric() { return state == State.electric; }
    public bool isDestructed() { return state == State.destructed; }
    public bool isInvisible() { return state == State.invisible; }
}
