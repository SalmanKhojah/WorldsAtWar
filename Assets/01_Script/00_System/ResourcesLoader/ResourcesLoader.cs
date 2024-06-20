using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourcesLoader : MonoBehaviour
{
    public static ResourcesLoader Instance;

    [SerializeField]
    private Sprite[] _exposionSpritesArray;
    [SerializeField]
    private AudioClip[] _exposionSoundsArray;

    public void Initialize()
    {
        Instance = this;
    }

    public void LoadSprite()
    {
        _exposionSpritesArray = Resources.LoadAll<Sprite>("Sprites");

        _exposionSoundsArray = Resources.LoadAll<AudioClip>("Audio");
    }

    public Sprite[] GetExposionSpritesArray()
    {
        return _exposionSpritesArray;
    }

    public AudioClip GetAudioClip(string name)
    {
        AudioClip audioClip = null;

        foreach (AudioClip clip in _exposionSoundsArray)
        {
            if (clip.name.Contains(name))
            {
                audioClip = clip;
                break;
            }
        }

        return audioClip;
    }
}
