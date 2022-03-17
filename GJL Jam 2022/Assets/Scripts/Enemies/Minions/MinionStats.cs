/*
Copyright JACPro 2022 - https://jacpro.github.io
GJL Game Parade Spring 2022 - https://itch.io/jam/game-parade-spring-2022
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MinionStats
{
    public int _level;
    public float _damage;
    public float _speed;
    public float _size;

    public MinionStats(int level, float damage, float speed, float size)
    {
        _level = level;
        _damage = damage;
        _speed = speed;
        _size = size;
    }
}
