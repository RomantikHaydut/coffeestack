using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action StartGame;

    public bool isGameStarted;

    private void Awake()
    {
        isGameStarted = false;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
