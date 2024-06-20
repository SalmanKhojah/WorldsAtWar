using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FadeinManager : MonoBehaviour
{
    private SpriteRenderer _sr;

    private Color _startColor;
    private Color _endColor;

    private float _currentFadeInTime;
    private float _maxFadeOutTime = 0.5f;


    public void Initialize()
    {
        _sr = GetComponent<SpriteRenderer>();

        _startColor = Color.black;
        _endColor = Color.black;

        _currentFadeInTime = 0;
        _startColor.a = 0;

        _sr.color = _startColor;
    }

    public void UpdateScript()
    {
        ColorLerpProcess();
    }

    private void ColorLerpProcess()
    {
        float t = _currentFadeInTime / _maxFadeOutTime;

        if (t > 1)
        {
            t = 1;
        }

        _sr.color = Color.Lerp(_startColor, _endColor, t);

        if (t == 1)
        {

        }
        else
        {
            _currentFadeInTime += Time.deltaTime;
        }
    }

}
