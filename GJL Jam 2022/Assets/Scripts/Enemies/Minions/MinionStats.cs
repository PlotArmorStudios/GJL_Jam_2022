/*
Copyright JACPro 2022 - https://jacpro.github.io
GJL Game Parade Spring 2022 - https://itch.io/jam/game-parade-spring-2022
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MinionStats
{
    public int Level;
    public float Damage;
    public float Speed;
    public float Size;

    public MinionStats(int level, float damage, float speed, float size)
    {
        Level = level;
        Damage = damage;
        Speed = speed;
        Size = size;
    }
}
