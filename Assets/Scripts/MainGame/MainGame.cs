using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{

    [SerializeField] Player player;

    [SerializeField] GameObject Myelines;

   

    //--------------------------------------------------------------------------------
    //------------------------  WAVE GENERATION     ----------------------------------
    //--------------------------------------------------------------------------------

    [SerializeField] GameObject CellulesBlanches;

    [SerializeField] WhiteCell celluleBlanchePrefab;

    float timer_wave;
    float duration_wave;
    int current_wave;
    int total_waves=10;


    //--------------------------------------------------------------------------------
    //------------------------  AMMO GENERATION     ----------------------------------
    //--------------------------------------------------------------------------------

    [SerializeField] GameObject Ammunitions;

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

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize() {
        timer_wave = 0;
        duration_wave = 10.0f;
        current_wave = 0;

        player.Initialize();

        foreach (Transform t in Myelines.transform) {
            t.GetComponent<Myeline>().Initialize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        wavesManagement();
        itemsManagement();
        ammoGeneration();
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

        Vector3 pos = new Vector3(0, 0, 0);

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

            Debug.Log(i + " : " + player.ammo[i]);

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

            if (Random.Range(0, 1) == 0) {
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
            a.transform.position = new Vector3(0, 0, 0);
        }
    }
}
