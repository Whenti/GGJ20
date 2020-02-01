using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{

    [SerializeField] Player player;

    [SerializeField] GameObject Myelines;
    [SerializeField] GameObject CellulesBlanches;
    [SerializeField] CelluleBlanche celluleBlanchePrefab;



    //--------------------------------------------------------------------------------
    //------------------------  WAVE GENERATION     ----------------------------------
    //--------------------------------------------------------------------------------

    float timer_wave;
    float duration_wave;
    int current_wave;
    int total_waves=10;

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

        CelluleBlanche c = CelluleBlanche.Instantiate(celluleBlanchePrefab);
        c.transform.SetParent(CellulesBlanches.transform, false);
        c.transform.position = pos;

    }
}
