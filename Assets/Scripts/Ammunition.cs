using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{

    [SerializeField] Particule particulePrefab;

    Vector3 direction;

    float speed = 0.1f;

    float timer;
    float duration = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
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
        transform.position += direction.normalized * speed;
    }

    void destructionManagement() {
        timer += Time.deltaTime;

        if (timer >= duration) {
            destruction();
        }
    }

    public void destruction() {

        for (int i = 0; i < 10; ++i) {
            //create particule

            Particule p = Particule.Instantiate(particulePrefab);
            p.transform.position = transform.position;

        }

        Destroy(gameObject);
    }

    
}
