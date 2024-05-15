using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUiMainManager : MonoBehaviour
{
    [SerializeField]
    private UitextPlugingIn _texts;

    public void Init()
    {
        _texts = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<UitextPlugingIn>();

        _texts.Init();

        _texts.SetUIText("Yay");
    }

    public void SetUIText(string text)
    {
        _texts.SetUIText(text); 
    }
}
