using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalBulletObject : BulletBlueprint
{
    public override void Initialization(Vector3 velocity, int damage)
    {
        _velocity = velocity;
        _damage = damage;
    }

    public override Vector3 UpdateScript(Vector3 currentPosition)
    {
        currentPosition += _velocity * Time.deltaTime;

        return currentPosition;
    }
}