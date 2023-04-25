using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 2;
    public const int gridColumns = 4;
    public const float offsetX = 300f;
    public const float offsetY = 500f;

    [SerializeField] private MainCard mainCard;

    [SerializeField] private Sprite[] images;
    
    [SerializeField] private Canvas canvas;
    
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _soundOnBtn;
    [SerializeField] private Button _soundOffBtn;
    [SerializeField] private Button _efxOnBtn;
    [SerializeField] private Button _efxOffBtn;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _comboTxt;
    [SerializeField] private Text _timer;
    [SerializeField] private Text _numOfTriesTxt;

    private int _score = 0;
    private int _combo = 0;
    private int _numOfTries = 0;
    private int comboTemp = 0;

    private int _minutes = 0;
    private int _seconds = 0;
    private string _secondsString;
    private string _minutesString;

    private bool _countTime = true;

    private MainCard _firstRevealedCard;
    private MainCard _secondRevealedCard;
    private AudioSource _music;
    private AudioSource _efx;

    public bool canReveal
    {
        get { return _secondRevealedCard == null; }
    }

    private void Start()
    {
        SetCardPositions();

        StartCoroutine(StartTimer());

        _efx = _efxOnBtn.GetComponent<AudioSource>();

        _soundOffBtn.gameObject.SetActive(false);
        _efxOffBtn.gameObject.SetActive(false);
    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (_countTime)
            {
                _seconds++;
            }
            
            if (_seconds < 60 && _minutes == 0)
            {
                if(_seconds < 10)
                {
                    _secondsString = "0" + _seconds.ToString();
                    _timer.text = ":" + _secondsString;
                }
                else
                {
                    _timer.text = ":" + _seconds.ToString();
                }
            }
            else if(_seconds < 60 && _minutes > 0)
            {
                if (_seconds < 10)
                {
                    _secondsString = "0" + _seconds.ToString();
                    _timer.text = _minutes.ToString() + ":" + _secondsString;
                }
                else
                {
                    _timer.text = _minutes.ToString() + ":" + _seconds.ToString();
                }
            }
            else
            {
                _seconds = 0;
                _minutes++;
                _timer.text = _minutes.ToString() + ":" + _seconds.ToString();
            }
        }
    }
    private void SetCardPositions()
    {
        Vector3 startPosition = mainCard.transform.position;

        int[] cardNumbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        cardNumbers = ShuffleMainCards(cardNumbers);

        MainCard card;

        for(int i=0; i<gridColumns; i++)
        {
            for(int j=0; j<gridRows; j++)
            {
                if(i==0 && j == 0)
                {
                    card = mainCard;
                }
                else
                {
                    card = Instantiate(mainCard);
                }

                int index = j * gridColumns + i;
                int id = cardNumbers[index];
                card.ChangeImage(id, images[id]);

                float positionX = startPosition.x + (i*offsetX);
                float positionY = startPosition.y + (j*offsetY);
                card.transform.SetParent(canvas.transform);
                card.transform.position = new Vector3(positionX, positionY, startPosition.z);
            }
        }
    }

    private int[] ShuffleMainCards(int[] cardNumbers)
    {
        int[] shuffledCardsNumbers = cardNumbers.Clone() as int[];

        for (int i=0; i<shuffledCardsNumbers.Length; i++)
        {
            int temp = shuffledCardsNumbers[i];
            int r = UnityEngine.Random.Range(0, shuffledCardsNumbers.Length);
            shuffledCardsNumbers[i] = shuffledCardsNumbers[r];
            shuffledCardsNumbers[r] = temp;
        }

        return shuffledCardsNumbers;
    }

    public void RevealCard(MainCard card)
    {
        if(_efx.mute == false)
        {
            _efx.Play();
        }

        if(_firstRevealedCard == null)
        {
            _firstRevealedCard = card;
        }
        else
        {
            _secondRevealedCard = card;

            StartCoroutine(CheckCardsSimularity());
        }
    }

    public IEnumerator CheckCardsSimularity()
    {
        yield return new WaitForSeconds(1);
        if(_firstRevealedCard.Id == _secondRevealedCard.Id)
        {
            _score++;
            comboTemp++;
            if(comboTemp >= 2)
            {
                _combo += 1;
                _comboTxt.text = "Combos: " + _combo.ToString();
            }
            _scoreText.text = "Score: " + _score.ToString();
            _firstRevealedCard.gameObject.GetComponent<Button>().enabled = false;
            _secondRevealedCard.gameObject.GetComponent<Button>().enabled = false;
            
        }
        else
        {
            _firstRevealedCard.CardBack.SetActive(true);
            _secondRevealedCard.CardBack.SetActive(true);
            comboTemp = 0;
        }

        _numOfTries++;
        _numOfTriesTxt.text = "Number of tries: " + _numOfTries.ToString();

        _firstRevealedCard = null;
        _secondRevealedCard = null;

        if (_score == 4)
        {
            _countTime = false;

            yield return new WaitForSeconds(1);

            GameManagerSingleton.Instance.SetValues(_seconds, _minutes, _score, _numOfTries, _combo);

            SceneManager.LoadScene("ResultScene");
        }
    }

    public void SoundOff()
    {
        if (_soundOnBtn.gameObject.activeSelf)
        {
            _music = GetComponent<AudioSource>();
            _music.mute = true;
            _soundOffBtn.gameObject.SetActive(true);
            _soundOnBtn.gameObject.SetActive(false);
        }
    }

    public void SoundOn()
    {
        if (_soundOffBtn.gameObject.activeSelf)
        {
            _music = GetComponent<AudioSource>();
            _music.mute = false;
            _soundOnBtn.gameObject.SetActive(true);
            _soundOffBtn.gameObject.SetActive(false);
        }
    }

    public void EfxOff()
    {
        if (_efxOnBtn.gameObject.activeSelf)
        {
            _efx.mute = true;
            _efxOffBtn.gameObject.SetActive(true);
            _efxOnBtn.gameObject.SetActive(false);
        }
    }

    public void EfxOn()
    {
        if (_efxOffBtn.gameObject.activeSelf)
        {
            _efx.mute = false;
            _efxOnBtn.gameObject.SetActive(true);
            _efxOffBtn.gameObject.SetActive(false);
        }
    }
}
