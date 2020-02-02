using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseLogic : MonoBehaviour
{
    [SerializeField] Illuminated button_resume;
    [SerializeField] Illuminated button_quit;
    [SerializeField] Illuminated button_retry;
    [SerializeField] Illuminated button_end_tuto;
    [SerializeField] GameObject game_over;
    [SerializeField] MainGame main_game;
    [SerializeField] MenuLogic menu_logic;
    [SerializeField] GameObject you_won;

    MainGame.GameState previous_game_state;

    // Start is called before the first frame update
    void Start()
    {
        button_resume.GetComponent<Button>().onClick.AddListener(ButtonResumeClicked);
        button_quit.GetComponent<Button>().onClick.AddListener(ButtonQuitClicked);
        button_retry.GetComponent<Button>().onClick.AddListener(ButtonQuitClicked);
        button_end_tuto.GetComponent<Button>().onClick.AddListener(ButtonQuitClicked);
        this.gameObject.SetActive(false);
    }

    public void Activate(bool b, bool win = false, bool tutorial = false)
    {
        previous_game_state = main_game.game_state;
        button_resume.MySetActive(b);
        button_quit.MySetActive(b);
        button_retry.MySetActive(!b && !tutorial);
        you_won.gameObject.SetActive(!b && win);
        game_over.gameObject.SetActive(!b && !win);
        button_end_tuto.gameObject.SetActive(!b && tutorial);
        this.gameObject.SetActive(true);
    }

    public void ButtonResumeClicked()
    {
        if (button_resume.MyIsActive())
        {
            this.gameObject.SetActive(false);
            main_game.game_state = previous_game_state;
        }
    }

    void ButtonQuitClicked()
    {
        this.gameObject.SetActive(false);
        main_game.quit();
        menu_logic.setTutorialFalse();
        menu_logic.quitGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
