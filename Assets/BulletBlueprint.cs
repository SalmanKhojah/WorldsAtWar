using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

public abstract class BulletBlueprint : MonoBehaviour
{
    protected float _speed = 5.0f;
    protected int _damage = 0;
    
    public abstract void Init(float speed, int damage);

    public abstract void UpdateScript();
}