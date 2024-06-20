using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{

    private Image _fadeImage;

    private Color _startColor = Color.black;
    private Color _endColor = Color.black;

    private bool _startFade = false;

    [SerializeField]
    private float _currentColorLerpTime = 0;
    private float _maxColorLerpTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        _fadeImage = GetComponent<Image>();

        _startColor.a = 0;

        _startFade = false;

        _currentColorLerpTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentColorLerpTime = 0;
            _startFade = true;
        }

        if (_startFade == true)
        {
            ColorLerpProcess();
        }
    }

    private void ColorLerpProcess()
    {
        float t = _currentColorLerpTime / _maxColorLerpTime;

        if (t > 1)
        {
            t = 1;
        }

        _fadeImage.color = Color.Lerp(_startColor,_endColor,t);

        if (t == 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            _currentColorLerpTime += Time.deltaTime;
        }
    }
}
