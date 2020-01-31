using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iris : MonoBehaviour
{
    [SerializeField]
    GameObject Eye;

    private Vector2 eye_size;
    private Vector2 iris_size;
    private Vector3 computed_pos;

    // Start is called before the first frame update
    void Start()
    {
        eye_size = Eye.GetComponent<Renderer>().bounds.size;
        iris_size = this.GetComponent<Renderer>().bounds.size;
        Initialize();
    }

    void Initialize()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 eye_pos = Eye.transform.position;
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 eye_to_mouse = new Vector2(mouse_pos.x - eye_pos.x, mouse_pos.y - eye_pos.y);
        float R = 0.1f;
        float l = 10f;
        float lambda = eye_size.x / 2.0f - iris_size.x;
        lambda = R - R * Mathf.Exp(-l * lambda); 
        computed_pos = new Vector3(eye_pos.x + eye_to_mouse.x * lambda, eye_pos.y + eye_to_mouse.y * lambda, eye_pos.z-0.1f);
        this.transform.position = computed_pos;
    }
}
