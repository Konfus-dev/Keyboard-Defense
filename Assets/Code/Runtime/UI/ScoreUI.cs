using System.Collections;
using System.Collections.Generic;
using KeyboardDefense.Logic.Score;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    public void OnScoreChanged()
    {
        scoreText.text = $"Score: {ScoreManager.Instance.Score}";
    }
}
