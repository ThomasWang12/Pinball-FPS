using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sound
{
    public enum name
    {
        Flipper, FlipperHit, SlowMoEnter, SlowMoExit, Shoot, Win, Lose
    }

    public static string AudioEnumToName(name name)
    {
        return name switch
        {
            name.Flipper => "Flipper",
            name.FlipperHit => "Flipper Hit",
            name.SlowMoEnter => "Slow Mo Enter",
            name.SlowMoExit => "Slow Mo Exit",
            name.Shoot => "Shoot",
            name.Win => "Win",
            name.Lose => "Lose",
            _ => ""
        };
    }
}

public class Audio : MonoBehaviour
{
    [Tooltip("Find the AudioSource component in its children and store them in this list.")]
    public List<AudioSource> audioList;

    void Start()
    {
        List<GameObject> audioObjects = new List<GameObject>();
        Methods.GetChildRecursive(gameObject, audioObjects, "");

        for (int i = 0; i < audioObjects.Count; i++)
        {
            AudioSource audioSource = audioObjects[i].GetComponent<AudioSource>();
            if (audioSource != null) audioList.Add(audioSource);
        }
    }

    public void Play(Sound.name name)
    {
        string audioName = Sound.AudioEnumToName(name);
        int index = AudioNameToIndex(audioName);
        audioList[index].PlayOneShot(audioList[index].clip);
    }

    public void Stop(Sound.name name)
    {
        string audioName = Sound.AudioEnumToName(name);
        int index = AudioNameToIndex(audioName);
        audioList[index].Stop();
    }

    int AudioNameToIndex(string name)
    {
        for (int i = 0; i < audioList.Count; i++)
        {
            if (audioList[i].gameObject.name == name)
                return i; // The index
        }
        return -1; // It is not in the list
    }
}