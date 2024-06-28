using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathController : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private Color _startColor;
    private Color _endColor;

    private bool _startToEndFlag;

    private float _currentMovingToCenterTime;
    private float _maxMovingToCenterTime = 1f;

    private float _currentFlashingColorTime;
    private float _maxFlashingColorTime = 0.05f;

    private MeshRenderer _shipBody;

    public void Initialize()
    {
        _endPosition = Vector3.zero;

        _currentMovingToCenterTime = 0;

        _startColor = Color.red;
        _endColor = Color.white;

        _currentFlashingColorTime = 0;

        _startToEndFlag = true;

        // _shipBody = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<MeshRenderer>();

        // _shipBody.material.SetColor("_Color", _startColor);
    }

    public Vector3 UpdateScript()
    {
        DoFlashingColorProcess();

        return MoveToCenterLerpProcess();
    }

    public void StartMovingToCenterLerpProcess(Vector3 startPosition)
    {
        _startPosition = startPosition;
    }

    private Vector3 MoveToCenterLerpProcess()
    {
        Vector3 results = Vector3.zero;
        float t = _currentMovingToCenterTime / _maxMovingToCenterTime;

        if (t > 1)
        {
            t = 1;
        }

        results = Vector3.Lerp(_startPosition, _endPosition, t);

        if (t == 1)
        {

        }
        else
        {
            _currentMovingToCenterTime += Time.deltaTime;
        }

        return results;
    }

    private void DoFlashingColorProcess()
    {
        if (_currentFlashingColorTime == _maxFlashingColorTime)
        {
            _currentFlashingColorTime = 0;
            _startToEndFlag = !_startToEndFlag;

            if (_startToEndFlag)
            {
                _shipBody.material.SetColor("_Color", _endColor);
            }
            else
            {
                _shipBody.material.SetColor("_Color", _startColor);
            }
        }
        else
        {
            _currentFlashingColorTime += Time.deltaTime;

            if (_currentFlashingColorTime > _maxFlashingColorTime)
            {
                _currentFlashingColorTime = _maxFlashingColorTime;
            }
        }
    }
}
