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
    Button button_quit_left;
    [SerializeField]
    Button button_quit_right;
    [SerializeField]
    Tongue tongue;
    [SerializeField]
    Canvas canvas_menu;
    [SerializeField]
    Iris iris_left;
    [SerializeField]
    Iris iris_right;
    [SerializeField]
    CameraLogic my_camera;
    [SerializeField]
    GameObject closed_eye_left;
    [SerializeField]
    GameObject closed_eye_right;

    private enum MenuState { ClosedLeft, ClosedRight, Normal, Quitting };
    private MenuState menu_state;

    // Start is called before the first frame update
    void Start()
    {
        button_play.GetComponent<Button>().onClick.AddListener(ButtonPlayClicked);
        button_credit.GetComponent<Button>().onClick.AddListener(ButtonCreditClicked);
        button_quit_right.GetComponent<Button>().onClick.AddListener(ButtonQuitRightClicked);
        button_quit_left.GetComponent<Button>().onClick.AddListener(ButtonQuitLeftClicked);
        setState(MenuState.Normal);
    }

    void enableButton(Button button, bool b)
    {
        button.enabled = b;
        button.gameObject.SetActive(b);
    }

    void setState(MenuState menu)
    {
        Debug.Log(menu);
        menu_state = menu;
        if(menu == MenuState.Normal)
        {
        }
        else if(menu == MenuState.ClosedLeft)
        {
        }
        else if(menu == MenuState.ClosedRight)
        {
        }
        else if(menu == MenuState.Quitting)
        {
            this.canvas_menu.enabled = false;
        }
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
        my_camera.trigger();
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
    
    void ButtonQuitRightClicked()
    {
        if (menu_state == MenuState.ClosedRight)
            setState(MenuState.Normal);
        else if (menu_state == MenuState.ClosedLeft)
            setState(MenuState.Quitting);
        else if (menu_state == MenuState.Normal)
            setState(MenuState.ClosedRight);
    }

    void ButtonQuitLeftClicked()
    {
        if (menu_state == MenuState.ClosedLeft)
            setState(MenuState.Normal);
        else if (menu_state == MenuState.ClosedRight)
            setState(MenuState.Quitting);
        else if (menu_state == MenuState.Normal)
            setState(MenuState.ClosedLeft);
    }

    private void Quit()
    {
         
    }
}
