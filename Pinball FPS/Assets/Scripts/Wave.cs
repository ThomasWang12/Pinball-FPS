using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public List<GameObject> Enemies; // 裝這一波的敌人

    public bool WaveCompleted;

    public float appareSpeed;

    void Update()
    {
        if (transform.childCount == 0)
        {
            WaveCompleted = true;
        }
    }

    public void WaveStart()
    {
        StartCoroutine(GenerateNextEnemy());
    }

    public IEnumerator GenerateNextEnemy()
    {

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].SetActive(true);
            yield return new WaitForSeconds(appareSpeed);
        }

    }
}
