using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject FinishPanel;
    [SerializeField] Text scoreText;
    [SerializeField] Text levelText;

    private void Awake()
    {
        DisplayLevelText();
        Cursor.visible = false;
        FinishPanel.SetActive(false);
    }

    private void DisplayLevelText()
    {
        levelText.text = "Level " + SceneManager.GetActiveScene().buildIndex;
    }

    public void DisplayScore()
    {
        FinishPanel.SetActive(true);
        int score = FindObjectOfType<ScoreManager>().score;
        scoreText.text = "Score : " + score;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
