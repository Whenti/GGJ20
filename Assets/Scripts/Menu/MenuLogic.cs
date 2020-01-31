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
    [SerializeField]
    Canvas canvas_menu;
    [SerializeField]
    Iris iris_left;
    [SerializeField]
    Iris iris_right;
    
    // Start is called before the first frame update
    void Start()
    {
        button_play.GetComponent<Button>().onClick.AddListener(ButtonPlayClicked);
        button_credit.GetComponent<Button>().onClick.AddListener(ButtonCreditClicked);
        button_quit.GetComponent<Button>().onClick.AddListener(ButtonQuitClicked);
    }

    void ButtonPlayClicked()
    {
        startGame();
    }

    void startGame()
    {
        iris_left.quitMenu();
        iris_right.quitMenu();
        canvas_menu.enabled = false;
    }

    void endGame()
    {
        iris_left.enterMenu();
        iris_right.enterMenu();
        canvas_menu.enabled = true;
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
