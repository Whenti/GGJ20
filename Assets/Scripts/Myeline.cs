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

    public enum State { active, electric, destructed, invisible};
    State state;


    //--------------------------------------------------------------------------------
    //------------------------  ELECTRICITY           ----------------------------------
    //--------------------------------------------------------------------------------
    
    bool can_give_electric;

    float timer_electricity;
    float duration_electricity = 0.1f;
    float limite_give_electricity = 2.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        electricityManagement();
    }

    public void repair() {
        if (isDestructed()) {
            state = State.active;
        } else {
            //cannot repair, need a mega blast
        }
    }

    public void mega_repair() {
        if (isDestructed() || isInvisible()) {
            state = State.active;
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

            sr.color = Color.red;
            sr.gameObject.SetActive(true);
        } else if (state == State.destructed) {
            sr.color = Color.red;
            sr.gameObject.SetActive(true);
        } else if (state == State.invisible) {
            sr.color = Color.grey;
            sr.gameObject.SetActive(false);
        } else if (state == State.active) {
            sr.color = Color.white;
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
            if ( t.gameObject != gameObject && !t.GetComponent<Myeline>().isElectric() && (t.position - transform.position).magnitude <= limite_give_electricity) {
                t.GetComponent<Myeline>().receiveElectricity();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.tag == "ammo") {
            collision.GetComponent<Ammunition>().destruction();

            repair();


        }else if (collision.gameObject.tag == "mega_ammo") {
            collision.GetComponent<Ammunition>().destruction();

            mega_repair();
            
            
        }
    }

    public bool isActive() { return state == State.active; }
    public bool isElectric() { return state == State.electric; }
    public bool isDestructed() { return state == State.destructed; }
    public bool isInvisible() { return state == State.invisible; }
}
