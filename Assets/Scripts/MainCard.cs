using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class MainCard : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private GameObject cardBack;

    private int _id;

    public int Id
    {
        get { return _id; }
    }

    public GameObject CardBack
    {
        get { return cardBack; }
    }

    public void Card_OnClick()
    {
        if(cardBack.activeSelf && sceneController.canReveal)
        {
            cardBack.SetActive(false);
            sceneController.RevealCard(this);
        }
    }

    public void UnReveal()
    {
        cardBack.SetActive(true);
    }

    public void ChangeImage(int id, Sprite image)
    {
        _id = id;
        GetComponent<UnityEngine.UI.Image>().sprite = image;
    }
}
