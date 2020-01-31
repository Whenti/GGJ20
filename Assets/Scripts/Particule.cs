using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particule : MonoBehaviour
{

    SpriteRenderer sr;

    [SerializeField] Sprite ammo_sprite;
    [SerializeField] float ammo_scale;

    float timer;
    float duration=1.0f;

    Vector3 direction;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        float alpha = Random.Range(0.0f, 2 * Mathf.PI);
        direction = new Vector2(Mathf.Cos(alpha), Mathf.Sin(alpha));

        speed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        opacityManagement();
        movementManagement();
    }

    void opacityManagement() {
        timer += Time.deltaTime;

        Color c = sr.color;
        sr.color = new Color(c.r, c.g, c.b, 1-timer / duration);

        if (timer >= duration) {
            Destroy(gameObject);
        }
    }

    void movementManagement() {
        speed = speed * 0.94f;
        transform.position = direction.normalized * speed;
    }
}
