using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFirstScript : MonoBehaviour
{

    public int testOne; 
    public float testTwo;
    public bool testThree;
    string teststring;

    public enum test
    {

    }


    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(testOne + "in start");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(testTwo + "in update");
    }


}