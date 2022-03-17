/*
Copyright JACPro 2022 - https://jacpro.github.io
GJL Game Parade Spring 2022 - https://itch.io/jam/game-parade-spring-2022
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartManager : MonoBehaviour
{
    private Transform[] _bodyParts;
    [SerializeField] GameObject sphere;

    private void Awake() 
    {
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
        _bodyParts = new Transform[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            _bodyParts[i] = colliders[i].gameObject.transform;
            _bodyParts[i].tag = "Body Part";
            _bodyParts[i].gameObject.layer = LayerMask.NameToLayer("BodyParts");
            Vector3 worldPos = colliders[i].transform.TransformPoint(colliders[i].center);
            GameObject myObject = Instantiate(sphere, worldPos, Quaternion.identity);
            myObject.transform.parent = _bodyParts[i].transform;
        }
    }

    public Transform GetRandomBodyPart()
    {
        return _bodyParts[Random.Range(0, _bodyParts.Length)];
    }
}