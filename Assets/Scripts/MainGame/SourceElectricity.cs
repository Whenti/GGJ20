using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceElectricity : MonoBehaviour
{

    float timer;
    float duration = 1.0f;

    float limite_give_electricity = 5.0f;

    [SerializeField] GameObject myelines;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;



        if (timer >= duration) {
            timer -= duration;

            //launch electricity


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
