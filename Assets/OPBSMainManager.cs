using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OPBSMainManager : MonoBehaviour
{
    private List<OPBSBody> _activeOPBSObjectList;
    private OPBSSpawner _OPBSSpawnerController;


    private bool _beginOPBSFlag;

    public void Initialize()
    {
        _beginOPBSFlag = false;
        


        _OPBSSpawnerController = transform.GetChild(0).GetComponent<OPBSSpawner>();

        _OPBSSpawnerController.Initialize();
        EventSystemReference.Instance.OPBSPutObjectBackToSleepEventHandler.AddListener(PutObjectToSleep);


    }

    public void BeginOPBS(int numberOfFires)
    {
        if (!_beginOPBSFlag)
        _activeOPBSObjectList = _OPBSSpawnerController.Spawn(numberOfFires);

        if (_activeOPBSObjectList.Count > 0)
        {
            _activeOPBSObjectList[_activeOPBSObjectList.Count - 1].BeginObject();
            _beginOPBSFlag = true;
        }


    }

    public void UpdateScript()
    {
            BeginOPBS(83);



            // // int count = _activeOPBSObjectList.Count;

            // // for (int i = count - 1; i >= 0; i--)
            // // {
            // //     _activeOPBSObjectList[i].UpdateScript();

            // //     // if (i > 0)
            // //     // {
            // //     //     if (_activeOPBSObjectList[i].StartMovingCheck == true && _activeOPBSObjectList[i - 1].IsCountingDown == false)
            // //     //     {
            // //     //         _activeOPBSObjectList[i - 1].IsCountingDown = true;
            // //     //     }
            // //     // }
            // }
        }





    private void PutObjectToSleep(OPBSBody opbs)
    {

        _activeOPBSObjectList.Remove(opbs);
        _OPBSSpawnerController.PutOPBSBackToSleep(opbs); 
    }
    

    public void PutAllOPBSToSleep()
{
        int count = _activeOPBSObjectList.Count;

        for(int i = count - 1; i >=0; i--)
        {
            OPBSBody opbs = _activeOPBSObjectList[i];

            _activeOPBSObjectList.Remove(opbs);
            _OPBSSpawnerController.PutOPBSBackToSleep(opbs);

        }
// }



}
}