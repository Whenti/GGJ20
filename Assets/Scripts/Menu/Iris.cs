using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iris : MonoBehaviour
{
    private enum FollowState { Mouse, None};
    [SerializeField]
    GameObject Eye;
    [SerializeField]
    GameObject Player;

    private Vector2 eye_size;
    private Vector2 iris_size;
    private Vector3 computed_pos;
    private FollowState follow_state;
    private Vector2 target_pos;

    // Start is called before the first frame update
    void Start()
    {
        eye_size = Eye.GetComponent<Renderer>().bounds.size;
        iris_size = this.GetComponent<Renderer>().bounds.size;
        Initialize();
    }

    void Initialize()
    {
        target_pos = new Vector2(0, 0);
        follow_state = FollowState.Mouse; 
    }

    public void quitMenu()
    {
        follow_state = FollowState.None;
        this.transform.position = Eye.transform.position;
        this.transform.position -= new Vector3(0f, 0f, 0.1f);
    }

    public void enterMenu()
    {
        follow_state = FollowState.Mouse;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.follow_state == FollowState.Mouse)
        {
            Vector3 eye_pos = Eye.transform.position;
            target_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 eye_to_mouse = new Vector2(target_pos.x - eye_pos.x, target_pos.y - eye_pos.y);
            float R = eye_size.x / 2.0f - iris_size.x / 2.0f;
            float l = 0.05f;
            float lambda = R - R * Mathf.Exp(-l * eye_to_mouse.magnitude / (this.transform.lossyScale.x));
            eye_to_mouse.Normalize();
            computed_pos = new Vector3(eye_pos.x + eye_to_mouse.x * lambda, eye_pos.y + eye_to_mouse.y * lambda, eye_pos.z - 0.1f);
            this.transform.position = computed_pos;
        }
    }
}
