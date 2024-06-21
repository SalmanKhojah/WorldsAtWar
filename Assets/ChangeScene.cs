using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{

    private Image _fadeImage;

    private Color _startColor = Color.green;
    private Color _endColor = Color.blue;

    private bool _startFade = false;

    [SerializeField]
    private float _currentColorLerpTime = 0;
    private float _maxColorLerpTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        _startColor.a = 0;

        _startFade = false;

        _currentColorLerpTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_startFade == true)
        {
            ColorLerpProcess();
        }
    }

    public void StartSceneChange()
    {
        _currentColorLerpTime = 0;
        _startFade = true;
    }

    public void ColorLerpProcess()
    {
        _fadeImage = GetComponent<Image>();

    // Check if _fadeImage is not null and ensure it's enabled
        if (_fadeImage != null)
        {
        _fadeImage.enabled = true; // Enable the Image component if it's disabled
        }
        float t = _currentColorLerpTime / _maxColorLerpTime;

        if (t > 1)
        {
            t = 1;
        }

        _fadeImage.color = Color.Lerp(_startColor,_endColor,t);

        if (t == 1)
        {
            SceneManager.LoadScene("00_Title");
        }
        else
        {
            _currentColorLerpTime += Time.deltaTime;
        }
    }
}
