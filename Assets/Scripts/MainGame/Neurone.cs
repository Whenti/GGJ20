using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neurone : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] GameObject myelines;

    public enum State { active, electric };
    State state;

    [SerializeField] Sprite sprite_active;
    [SerializeField] Sprite sprite_electric;

    //--------------------------------------------------------------------------------
    //------------------------  ELECTRICITY           --------------------------------
    //--------------------------------------------------------------------------------

    bool can_give_electric;

    float timer_electricity;
    float duration_electricity = 0.2f;
    float limite_give_electricity = 4.0f;



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
        }else if (state == State.active) {
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
                if (t.gameObject != gameObject && t.GetComponent<Neurone>().isActive() && (t.position - transform.position).magnitude <= limite_give_electricity) {
                    t.GetComponent<Neurone>().receiveElectricity();
                }
            }
        }
    }


    public bool isActive() { return state == State.active; }
    public bool isElectric() { return state == State.electric; }
}
