using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MinionStats
{
    public int _level;
    public int _damage;
    public float _speed;
    public float _size;

    public MinionStats(int level, int damage, float speed, float size)
    {
        _level = level;
        _damage = damage;
        _speed = speed;
        _size = size;
    }
}
