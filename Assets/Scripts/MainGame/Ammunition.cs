using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{

    MainGame mainGame;
    [SerializeField] Particule particulePrefab;

    bool shot=false;

    Vector3 direction;
    float speed = 0.1f;

    float seed;

    float timer;
    float duration = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainGame = GameObject.Find("MainGame").GetComponent<MainGame>();
        timer = 0;
        seed = Random.value * 1000;
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
    
}
