using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager
{
    public static UIManager Instance { get { return _instance; } }
    private static UIManager _instance = null;

    [SerializeField]
    Text WaveText = null;

    public void setTextWave(int waveNumber)
    {
        
    }

    private void Awake()

    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
