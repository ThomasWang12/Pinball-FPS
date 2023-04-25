using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBottom : MonoBehaviour
{
    Game game;

    void Awake()
    {
        game = GameObject.FindWithTag("GameManager").GetComponent<Game>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            game.DeductHealth();
            game.sound.Play(Sound.name.LoseHealth);
            game.PlayerRespawn();
        }

        if (collision.gameObject.tag == "Enemy")
        {
            game.DeductHealth();
            game.sound.Play(Sound.name.EnemyScore);
            collision.gameObject.GetComponent<Enemy>().Reset();
        }
    }
}
