using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{   
    AudioSource _audio;
    void Awake()
    {
      DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        _audio = GetComponent<AudioSource>();
    }
}
