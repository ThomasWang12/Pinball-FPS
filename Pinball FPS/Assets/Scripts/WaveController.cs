using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController WC;

    public List<Wave> Waves;

    public Wave CurWave = null;

    public bool LevelCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (WC == null)
        {
            WC = this;
        }
        else
        {
            Destroy(gameObject);
        }
            

    }

    // Update is called once per frame
    void Update()
    {
        if (CurWave != null && CurWave.WaveCompleted)
        {
            Destroy(Waves[0].gameObject);
            CurWave = null;
            Waves.RemoveAt(0);
        }
        if (CurWave == null)//TODO:check is start or not, if start then NextWave
        {
            NextWave();
        }
        if (Waves.Count == 0)
        {
            LevelCompleted = true;//TODO: End this level
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
