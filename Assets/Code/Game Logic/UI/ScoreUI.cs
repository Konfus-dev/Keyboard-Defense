using System.Collections;
using System.Collections.Generic;
using KeyboardDefense.Characters.Enemies;
using KeyboardDefense.Prompts;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    public void OnScoreChanged(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
