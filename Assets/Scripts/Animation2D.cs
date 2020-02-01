using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation2D {

    //************************************************************************************************************************
    //*************************************          GLOBAL VARIABLES               ******************************************
    //************************************************************************************************************************

    public string name { get; private set; }

    public int N { get { return sprites.Count; } }
    List<Sprite> sprites;

    public float duration { get; private set; }
    public float timer { get; private set; }

    public bool loop { get; private set; }

    public float graine { get; private set; }



    //************************************************************************************************************************
    //*************************************          CONSTRUCTOR FONCTION       **********************************************
    //************************************************************************************************************************

    public Animation2D(string _name, List<Sprite> _sprites, float _duration, bool _loop = true, float _graine = 0) {
        name = _name;
        sprites = _sprites;
        duration = _duration;
        loop = _loop;
        graine = _graine;

        timer = graine;
    }

    //************************************************************************************************************************
    //**************************************          EVOLUTION FONCTION       ***********************************************
    //************************************************************************************************************************
    public void start() {
        timer = graine;
    }

    public bool evolve(float time_factor) {
        timer += Time.deltaTime * time_factor;

        int current = 0;
        if (loop) {
            current = Mathf.CeilToInt(timer / duration * N) % N;
        } else {

            current = Mathf.FloorToInt(Mathf.Min(timer / duration * N, N - 1));
        }

        return (current == N - 1);
    }

    public Sprite currentSprite() {
        if (loop) {
            return sprites[Mathf.CeilToInt(timer / duration * N) % N];
        } else {

            return sprites[Mathf.FloorToInt(Mathf.Min(timer / duration * N, N - 1))];
        }
    }





}
