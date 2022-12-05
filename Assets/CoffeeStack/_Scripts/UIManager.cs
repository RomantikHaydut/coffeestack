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
    private GameObject startText;

    private void Awake()
    {
        DisplayLevelText();
        Cursor.visible = false;
        FinishPanel.SetActive(false);
        GameManager.StartGame += CloseStartText;
    }

    private void DisplayLevelText()
    {
        levelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DisplayScore()
    {
        FinishPanel.SetActive(true);
        int score = FindObjectOfType<ScoreManager>().score;
        scoreText.text = "Score : " + score;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void CloseStartText()
    {
        startText = GameObject.Find("Start_Text");
        startText.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.StartGame -= CloseStartText;
    }
}
