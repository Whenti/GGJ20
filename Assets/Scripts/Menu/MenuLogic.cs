﻿using System.Collections;
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
    GameObject closed_eye_left;
    [SerializeField]
    GameObject closed_eye_right;

    [SerializeField]
    GameObject quit_text;
    [SerializeField]
    GameObject sure_text_right;
    [SerializeField]
    GameObject sure_text_left;
    [SerializeField]
    GameObject credits_text;
    [SerializeField]
    GameObject play_text;
    [SerializeField]
    GameObject tutorial_text;
    [SerializeField]
    GameObject left_paupiere;
    [SerializeField]
    GameObject right_paupiere;

    private enum MenuState { ClosedLeft, ClosedRight, Normal, Quitting, Playing };
    private MenuState menu_state;
    private bool tutorial;

    // Start is called before the first frame update
    void Start()
    {
        button_play.GetComponent<Button>().onClick.AddListener(ButtonPlayClicked);
        button_credit.GetComponent<Button>().onClick.AddListener(ButtonCreditClicked);
        button_quit_right.GetComponent<Button>().onClick.AddListener(ButtonQuitRightClicked);
        button_quit_left.GetComponent<Button>().onClick.AddListener(ButtonQuitLeftClicked);
        tutorial = true;
        setState(MenuState.Normal);
    }

    public void setTutorialFalse()
    {
        tutorial = false;
    }

    void setState(MenuState menu)
    {
        menu_state = menu;
        if(menu == MenuState.Normal)
        {
            quit_text.gameObject.SetActive(true);
            sure_text_right.gameObject.SetActive(false);
            sure_text_left.gameObject.SetActive(false);
            tutorial_text.SetActive(tutorial);
            play_text.SetActive(!tutorial);
            left_paupiere.gameObject.SetActive(false);
            right_paupiere.gameObject.SetActive(false);
            credits_text.gameObject.SetActive(true);
        }
        else if(menu == MenuState.ClosedLeft)
        {
            quit_text.gameObject.SetActive(false);
            sure_text_right.gameObject.SetActive(true);
            sure_text_left.gameObject.SetActive(false);
            left_paupiere.gameObject.SetActive(true);
            right_paupiere.gameObject.SetActive(false);
        }
        else if(menu == MenuState.ClosedRight)
        {
            quit_text.gameObject.SetActive(false);
            sure_text_right.gameObject.SetActive(false);
            sure_text_left.gameObject.SetActive(true);
            left_paupiere.gameObject.SetActive(false);
            right_paupiere.gameObject.SetActive(true);
        }
        else if(menu == MenuState.Quitting)
        {
            iris_left.enterMenu();
            iris_right.enterMenu();
            canvas_menu.enabled = true;
            setState(MenuState.Normal);
        }
        else if(menu == MenuState.Playing)
        {
            iris_left.quitMenu();
            iris_right.quitMenu();
            canvas_menu.enabled = false;
            credits_text.SetActive(false);
            tutorial_text.SetActive(false);
            quit_text.SetActive(false);
            sure_text_left.SetActive(false);
            sure_text_right.SetActive(false);
            play_text.SetActive(false);

            GameObject.Find("MainGame").GetComponent<MainGame>().play(tutorial);
        }
    }

    void ButtonPlayClicked()
    {
        setState(MenuState.Playing);
    }

    public void quitGame()
    {
        setState(MenuState.Quitting);
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
            Application.Quit();
        else if (menu_state == MenuState.Normal)
            setState(MenuState.ClosedRight);
    }

    void ButtonQuitLeftClicked()
    {
        if (menu_state == MenuState.ClosedLeft)
            setState(MenuState.Normal);
        else if (menu_state == MenuState.ClosedRight)
            Application.Quit();
        else if (menu_state == MenuState.Normal)
            setState(MenuState.ClosedLeft);
    }

    private void Quit()
    {
         
    }
}
