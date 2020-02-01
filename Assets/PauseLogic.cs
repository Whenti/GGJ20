using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseLogic : MonoBehaviour
{
    [SerializeField] Button button_resume;
    [SerializeField] Button button_quit;
    [SerializeField] MainGame main_game;
    [SerializeField] MenuLogic menu_logic;

    MainGame.GameState previous_game_state;

    // Start is called before the first frame update
    void Start()
    {
        button_resume.GetComponent<Button>().onClick.AddListener(ButtonResumeClicked);
        button_quit.GetComponent<Button>().onClick.AddListener(ButtonQuitClicked);
        this.gameObject.SetActive(false);
    }

    public void Activate()
    {
        previous_game_state = main_game.game_state;
        this.gameObject.SetActive(true);
    }

    public void ButtonResumeClicked()
    {
        this.gameObject.SetActive(false);
        main_game.game_state = previous_game_state;
    }

    void ButtonQuitClicked()
    {
        this.gameObject.SetActive(false);
        main_game.quit();
        menu_logic.quitGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
