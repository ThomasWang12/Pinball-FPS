using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    Game game;

    public static WaveController WC;

    public List<Wave> Waves;

    public Wave CurWave = null;

    public int totalWave;
    public int currentWave = 1;

    public bool LevelCompleted = false;

    void Awake()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<Game>();
    }

    void Start()
    {
        totalWave = Waves.Count;

        if (WC == null)
        {
            WC = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (CurWave != null && CurWave.WaveCompleted)
        {
            game.ui.WaveComplete(currentWave);
            game.sound.Play(Sound.name.Win);
            currentWave++;

            Destroy(Waves[0].gameObject);
            CurWave = null;
            Waves.RemoveAt(0);
        }
        if (Waves.Count == 0)
        {
            LevelCompleted = true;
        }
    }

    public void NextWave()
    {
        if (Waves.Count > 0)
        {
            Waves[0].gameObject.SetActive(true);
            Waves[0].WaveStart();
            CurWave = Waves[0];
        }
    }

}
