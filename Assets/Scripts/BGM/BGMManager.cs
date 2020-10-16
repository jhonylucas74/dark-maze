using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : Singleton<BGMManager>
{   
    AudioSource _LightThemeAudio;
    AudioSource _GameThemeAudio;
    public GameObject LightTheme;
    public GameObject GameTheme;
    protected override void Awake()
    {
        base.Awake();
        Events.OnLightOn += OnLightOn;
        Events.OnLightOff += OnLightOff;
    }

    void OnDestroy () {
        Events.OnLightOn -= OnLightOn;
        Events.OnLightOff -= OnLightOff;
    }

    void Start ()
    {
        _LightThemeAudio = LightTheme.GetComponent<AudioSource>();
        _GameThemeAudio = GameTheme.GetComponent<AudioSource>();
    }

    void OnLightOn () {
        _LightThemeAudio.volume = 0.3f;
        _GameThemeAudio.volume = 0;
    }

    void OnLightOff () {
        _LightThemeAudio.volume = 0;
        _GameThemeAudio.volume = 0.3f;
    }
}
