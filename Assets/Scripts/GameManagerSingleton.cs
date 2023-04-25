using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingleton : MonoBehaviour
{
    public static GameManagerSingleton Instance;

    public int _seconds;
    public int _minutes;
    public int _score;
    public int _numOfTries;
    public int _combos;

    public void SetValues(int pSeconds, int pMinutes, int pScore, int pNumOfTries, int pCombos)
    {
        this._seconds = pSeconds;
        this._minutes = pMinutes;
        this._score = pScore;
        this._numOfTries = pNumOfTries;
        this._combos = pCombos;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }
}
