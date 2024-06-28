using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManDialogueManager : MonoBehaviour
{
    private OldManTextBox _textBoxManager;
    private PlayerMainManger playerMainManager;

    private bool _dialogueShown;

    public void Initialize()
    {
        _dialogueShown = false;
        

        playerMainManager = FindObjectOfType<PlayerMainManger>();

        _textBoxManager = transform.GetChild(0).GetComponent<OldManTextBox>();


    }

    
    public void UpdateScript()
    {
        bool oldManInitiate = playerMainManager.OldManDialogue;

        if (oldManInitiate && !_dialogueShown)
        {
            _textBoxManager.ShowTextbox("Omar, oh my young boy, are you hurt?");
            _textBoxManager.ShowTextbox("Forget that, there's no time, the fire is spreading.");
            _textBoxManager.ShowTextbox("Follow the path and open the hatch.");
            _textBoxManager.ShowTextbox("Get inside it Omar, be quick!");

            _dialogueShown = true;

        }

    }
}
