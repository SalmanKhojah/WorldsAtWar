using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamDialogueMainManager : MonoBehaviour
{
    private TextBox _textBoxManager;
    private PlayerMainManger playerMainManager;

    private Vector3 _playerStartPosition;
    private float _movementThreshold = 10f; // Distance the player needs to move to trigger the wave
    private bool _playerHasMovedEnough; 
    private bool _dialogueShown;

    public void Initialize()
    {
        _playerHasMovedEnough = false;
        _dialogueShown = false;
        

        playerMainManager = FindObjectOfType<PlayerMainManger>();

        _textBoxManager = transform.GetChild(0).GetComponent<TextBox>();


        _playerStartPosition = playerMainManager.ShipTransform.position;
    }

    private bool PlayerMovedEnough()
    {
        // if (playerMainManager.ShipTransform.position == null || _playerStartPosition == null)
        // {
        //     return false; 
        // }

        float distanceMoved = Vector3.Distance(_playerStartPosition, playerMainManager.ShipTransform.position);
        return distanceMoved >= _movementThreshold;
    }

    
    public void UpdateScript()
    {
        if (!_playerHasMovedEnough && PlayerMovedEnough())
        {
            _playerHasMovedEnough = true;
        }

        if (_playerHasMovedEnough && !_dialogueShown)
        {
            _textBoxManager.ShowTextbox("....");
            _textBoxManager.ShowTextbox("No...no...creatures, in this barren land ♫");
            _textBoxManager.ShowTextbox("....");

            _dialogueShown = true;

        }

    }
}
