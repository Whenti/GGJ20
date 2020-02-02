using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceElectricity : MonoBehaviour
{

    float timer;
    float duration = 1.0f;

    float limite_give_electricity = 5.0f;

    [SerializeField] GameObject myelines;

    SpriteRenderer sr;

    [SerializeField] Sprite sprite_active;
    [SerializeField] Sprite sprite_electric;

    bool is_electric;
    float timer_electricity;
    float duration_electricity=0.2f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Initialize();
    }

    public void Initialize() {
        is_electric = false;
        timer_electricity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (is_electric) {
            timer_electricity += Time.deltaTime;
            

            if (timer_electricity >= duration_electricity) {
                is_electric = false;
                timer_electricity = 0;
            }

            sr.sprite = sprite_electric;
        } else {
            sr.sprite = sprite_active;
        }


        if (timer >= duration) {
            timer -= duration;

            //launch electricity
            is_electric = true;


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
    }


}
