using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUiMainManager : MonoBehaviour
{
    private UiTextObject _texts;
    private GameObject _gameOverText;

    public void Initialize()
    {
        _texts = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<UiTextObject>();

        _texts.Initialize();

        _gameOverText = transform.GetChild(1).gameObject;

        _gameOverText.SetActive(false);
        //_texts.SetUIText();

        EventSystemReference.Instance.UpdateUiScoreEventTextHandler.AddListener(SetUIText);
    }

    public void SetUIText(int score)
    {
        _texts.SetUIText(score.ToString()); 
    }

    public void SetGameOverObjectActiveState(bool flag)
    {
        _gameOverText.SetActive(flag);
    }
}
