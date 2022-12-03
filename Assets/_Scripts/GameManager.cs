using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action StartGame;

    public bool isGameStarted;

    public bool isLevelFinished;

    private void Awake()
    {
        isGameStarted = false;
        isLevelFinished = false;
        if (Instance == null)
        {
            Instance = this;
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGameStarted)
        {
            StartGame?.Invoke();
            isGameStarted = true;
        }
    }
}
