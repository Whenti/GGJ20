using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] Transform posInitialePlayer;

    [SerializeField] GameObject Myelines;
    [SerializeField] PauseLogic pause_canvas; 
    [SerializeField]
    CameraLogic my_camera;


    //--------------------------------------------------------------------------------
    //------------------------  VICTORY AND GAME OVER---------------------------------
    //--------------------------------------------------------------------------------
    
    public enum GameState { Game, Pause, Injection, NotPlaying, WaitingCamera, GameOver};
    public GameState game_state;

    double life;
    double life_display;
    double life_max = 100;


    //--------------------------------------------------------------------------------
    //------------------------  WAVE GENERATION     ----------------------------------
    //--------------------------------------------------------------------------------

    [SerializeField] GameObject Generators;

    [SerializeField] GameObject CellulesBlanches;

    [SerializeField] WhiteCell celluleBlanchePrefab;

    float timer_wave;
    float duration_wave;
    int current_wave;
    int total_waves=10;


    //--------------------------------------------------------------------------------
    //------------------------  AMMO GENERATION     ----------------------------------
    //--------------------------------------------------------------------------------

    public GameObject Ammunitions;
    public GameObject Particules;

    [SerializeField] Ammunition ammoPrefab;
    [SerializeField] Ammunition megaAmmoPrefab;

    float timer_ammo;
    float duration_ammo = 1.0f;

    //--------------------------------------------------------------------------------
    //------------------------  AMMO VISUALIZATION  ----------------------------------
    //--------------------------------------------------------------------------------
    [SerializeField] List<Image> items;
    [SerializeField] Sprite sprite_ammo;
    [SerializeField] Sprite sprite_mega_ammo;

    //--------------------------------------------------------------------------------
    //------------------------  AMMO VISUALIZATION  ----------------------------------
    //--------------------------------------------------------------------------------
    [SerializeField] Syringe syringe;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize() {
        game_state = GameState.NotPlaying;
        timer_wave = 0;
        duration_wave = 4.0f;
        current_wave = 0;

        life = life_max;
        life_display = life;

        player.Initialize();
        player.transform.position = posInitialePlayer.position;


        foreach (Transform t in Myelines.transform) {
            if (t.tag == "myeline") {
                t.GetComponent<Myeline>().Initialize();
            } else if (t.tag == "neurone") {
                t.GetComponent<Neurone>().Initialize();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (game_state)
        {
            case GameState.WaitingCamera:
                if (my_camera.camera_state == CameraLogic.CameraState.Game)
                {
                    game_state = GameState.Game;
                }
                else if (my_camera.camera_state == CameraLogic.CameraState.Overall)
                {
                    game_state = GameState.NotPlaying;
                }
                break;
            case GameState.Game:
                wavesManagement();
                ammoGeneration();
                healthManagement();
                break;
            case GameState.Injection:
                if (syringe.syringe_mode == Syringe.SyringeMode.Done)
                {
                    syringe.Initialize();
                    game_state = GameState.Game;
                }
                else
                {
                    Debug.Log("injection");
                    syringe.update_injection();
                }
                break;
            case GameState.Pause:
                break;
            case GameState.NotPlaying:
                break;
        }
        itemsManagement();

        if (game_state != GameState.Pause)
            my_camera.update_camera();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (game_state == GameState.Game || game_state == GameState.Injection)
            {
                pause_canvas.Activate(true);
                game_state = GameState.Pause;
            }
            else if(game_state == GameState.Pause)
            {
                pause_canvas.ButtonResumeClicked();
            }
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            play();
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            start_syringe();
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            gameOver();
        }
    }

    public void play() {
        my_camera.setState(CameraLogic.CameraState.ToGame);
        game_state = GameState.WaitingCamera;
    }

    public void quit()
    {
        my_camera.setState(CameraLogic.CameraState.ToOverall);
        game_state = GameState.WaitingCamera;
        syringe.Initialize();
    }

    void wavesManagement() {

        if (current_wave < total_waves) {
            //if we are at the final wave, there is no need to use the timer. Only use it when current_wave < total_waves
            timer_wave += Time.deltaTime;
        }

        if (timer_wave >= duration_wave) {
            //new wave
            current_wave++;
            createWave(current_wave);
            

            timer_wave = 0;
            duration_wave = 10.0f;
        }
    }

    public void waveEnded() {
        //this functions is used when the player finished a wave early,
        //the function increment the timer so the player does not have to wait next wave

        timer_wave = Mathf.Max(timer_wave, duration_wave - 5.0f);
    }

    public void createWave(int number_cells) {
        for (int i = 0; i < number_cells; ++i) {
            createCell();
        }
    }

    public void createCell() {
        //choose a position

        int index = Random.Range(0, Generators.transform.childCount);
        Vector3 pos = Generators.transform.GetChild(index).transform.position;

        //create cell

        WhiteCell c = WhiteCell.Instantiate(celluleBlanchePrefab);
        c.transform.SetParent(CellulesBlanches.transform, false);
        c.transform.position = pos;

    }

    void itemsManagement() {

        for (int i = 0; i < items.Count; ++i) {
            items[i].gameObject.SetActive(false);

        }

        //afficher ammos du player

        for (int i = 0; i < player.ammo.Count; ++i) {

            if (player.ammo[i] == "ammo") {
                items[i].sprite = sprite_ammo;
                items[i].gameObject.SetActive(true);
            } else if (player.ammo[i] == "mega_ammo") {
                items[i].sprite = sprite_mega_ammo;
                items[i].gameObject.SetActive(true);
            }
            
        }
    }

    void ammoGeneration() {
        timer_ammo += Time.deltaTime;

        if (timer_ammo >= duration_ammo) {
            timer_ammo -= duration_ammo;

            if (Random.Range(0, 2) == 0) {
                createAmmo("ammo");
            } else {
                createAmmo("mega_ammo");
            }
        }
    }

    public void createAmmo(string type) {
        Ammunition a = null;
        if (type == "ammo") {
            a = Ammunition.Instantiate(ammoPrefab);
        } else if (type == "mega_ammo") {
            a = Ammunition.Instantiate(megaAmmoPrefab);
        }

        if (a != null) {
            a.transform.SetParent(Ammunitions.transform, false);

            int index = Random.Range(0, Generators.transform.childCount);
            Vector3 pos = Generators.transform.GetChild(index).transform.position;
            a.transform.position = pos;
        }
    }

    public void addHealth(int i) {
        life += i;

        if (life >= life_max) life = life_max;
    }

    void healthManagement() {
        int num_sain =0;
        int num_total = 0;
        foreach (Transform t in Myelines.transform) {
            num_total++;
            if (t.tag=="myeline" && !t.GetComponent<Myeline>().isDestructed()) {
                num_sain++;
            }
        }

        //suivant le pourcentage de myeline absente, la santé descend plus vite

        life -= Mathf.Pow( 1-(float)num_sain/(float)num_total, 3);

        float speed_life_display = 1.0f;
        if (life < life_display - speed_life_display) {
            life_display -= speed_life_display;
        } else if (life > life_display + speed_life_display) {
            life_display += speed_life_display;
        } else {
            life_display = life;
        }


        if (life <= 0) {
            life = 0;
            gameOver();
        }
    }

    public void start_syringe()
    {
        syringe.Prepare();
        this.game_state = GameState.Injection;
    }

    public void gameOver() {
        //afficher canvas game over
        pause_canvas.Activate(false);
        this.game_state = GameState.GameOver;
    }

    public void clean() {
        //ammo
        foreach (Transform t in Ammunitions.transform) {
            Destroy(t.gameObject);
        }

        player.clean();

        //white cells
        foreach (Transform t in CellulesBlanches.transform) {
            Destroy(t.gameObject);
        }
    }
}
