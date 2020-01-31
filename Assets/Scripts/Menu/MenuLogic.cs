using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    [SerializeField]
    Button button_play;
    [SerializeField]
    Button button_credit;
    [SerializeField]
    Button button_quit;
    [SerializeField]
    Tongue tongue;
    
    // Start is called before the first frame update
    void Start()
    {
        button_play.GetComponent<Button>().onClick.AddListener(ButtonPlayClicked);
        button_credit.GetComponent<Button>().onClick.AddListener(ButtonCreditClicked);
        button_quit.GetComponent<Button>().onClick.AddListener(ButtonQuitClicked);
    }

    void ButtonPlayClicked()
    {
        Debug.Log("You have clicked the button!");
    }

    void ButtonCreditClicked()
    {
        tongue.trigger();
    }
    
    void ButtonQuitClicked()
    {
        Debug.Log("You have clicked the button!");
    }
}
