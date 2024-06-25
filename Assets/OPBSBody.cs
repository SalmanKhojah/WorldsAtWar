using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OPBSBody : MonoBehaviour
{
    private PlayerMainManger playerMainManager;

    public void Initialize()
    {

        playerMainManager = FindObjectOfType<PlayerMainManger>();

        

    }

    public void BeginObject()
    {

        transform.GetComponent<Collider2D>().enabled = true;


        gameObject.SetActive(true);
    }

    public void UpdateScript()
    {

    }




    public void RemoveObject()
    {
        gameObject.SetActive(false);
    }




    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void RemoveOPBS()
    {
        EventSystemReference.Instance.OPBSPutObjectBackToSleepEventHandler.Invoke(this);
        transform.GetComponent<Collider2D>().enabled = false;
        gameObject.SetActive(false);
    }

}