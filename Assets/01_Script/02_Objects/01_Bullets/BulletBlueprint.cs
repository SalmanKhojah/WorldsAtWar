using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBlueprint
{
    protected Vector3 _velocity;
    protected int _damage = 0;
    
    public int Damage { get { return _damage; } }

    public abstract void Initialization(Vector3 velocity, int damage);

    public abstract Vector3 UpdateScript(Vector3 currentPosition);
        
}


