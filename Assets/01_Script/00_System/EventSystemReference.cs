
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class BulletRequestUnityEvent : UnityEvent<BulletContiner>
{

}
[Serializable]
public class BulletPutBulletBackToSleepEvent : UnityEvent<BulletBody>
{

}

[Serializable]
public class SendScoreToPlayerEvent : UnityEvent<int>
{

}


[Serializable]
public class UiTextScoreUpdateRequestUnityEvent : UnityEvent<int>
{

}

[Serializable]
public class EnemyPutObjectBackToSleepEvent : UnityEvent<EnemyBody>
{

}

[Serializable]
public class ESPutObjectBackToSleepEvent : UnityEvent<ESBody>
{

}

[Serializable]
public class ExplostionRequestEvent : UnityEvent<Vector3>
{

}


public class EventSystemReference : MonoBehaviour
{
    public static EventSystemReference Instance;

    public BulletRequestUnityEvent BulletRequestEventHandler;
    public BulletPutBulletBackToSleepEvent BulletPutBulletBackToSleepEventHandler;
    public SendScoreToPlayerEvent SendScoreToPlayerEventHandler;
    public UiTextScoreUpdateRequestUnityEvent UpdateUiScoreEventTextHandler;
    public EnemyPutObjectBackToSleepEvent EnemyPutObjectBackToSleepEventHandler;
    public ESPutObjectBackToSleepEvent ESPutObjectBackToSleepEventHandler;
    public ExplostionRequestEvent ExplostionRequestEventHandler;
    public UnityEvent GameManagerStartPlayerDeathSequenceHandler;

    public void Initialize()
    {
        Instance = this;
    }
}
