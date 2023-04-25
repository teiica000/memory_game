using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultSceneController : MonoBehaviour
{
    [SerializeField] private Text _timeTxt;
    [SerializeField] private Text _scoreTxt;
    [SerializeField] private Text _numOfTriesTxt;
    [SerializeField] private Text _comboTxt;

    private int seconds;
    private int minutes;


    private void Start()
    {
        if(GameManagerSingleton.Instance != null)
        {
            seconds = GameManagerSingleton.Instance._seconds;
            minutes = GameManagerSingleton.Instance._minutes;
            if(seconds > 0)
            {
                seconds--;
            }
            else
            {
                minutes--;
                seconds += 59;
            }

            _timeTxt.text = minutes.ToString() + ":" + seconds.ToString();
            _scoreTxt.text = "Score:" + GameManagerSingleton.Instance._score.ToString();
            _numOfTriesTxt.text = "Number of tries: " + GameManagerSingleton.Instance._numOfTries.ToString();
            _comboTxt.text = "Combos: " + GameManagerSingleton.Instance._combos.ToString();
        }
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("GamePlayScene");
    }
}
