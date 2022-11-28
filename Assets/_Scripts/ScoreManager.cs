using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public void AddScore(int coffeeGrade,bool isCoffeeHeat,bool isCoffeeCreamed)
    {
        score += coffeeGrade * (isCoffeeHeat ? 2 : 1) * (isCoffeeCreamed ? 2 : 1);
    }
}
