using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingText : MonoBehaviour
{
    private Vector2 initial_scale;
    private Vector3 initial_pos;
    private float power;
    private int timer;
    private int T;

    // Start is called before the first frame update
    void Start()
    {
        initial_scale = this.gameObject.transform.localScale;
        initial_pos = this.gameObject.transform.position;
        T = 100;
        power = 0.1f;
        Initialize();
    }

    void Initialize()
    {
        timer = Random.Range(0, T);
    }

    // Update is called once per frame
    void Update()
    {
        ++timer;
        if (timer >= T)
        {
            timer = 0;
        }
        float lambda = Mathf.PI * 2 * (float)timer / (float)T;
        this.gameObject.transform.localScale = new Vector2(initial_scale.x * (1 + power * Mathf.Sin(lambda)), initial_scale.y * (1 - power * Mathf.Cos(lambda)));
    }
}
