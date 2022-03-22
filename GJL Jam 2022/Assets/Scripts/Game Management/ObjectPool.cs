/*
Copyright JACPro 2022 - https://jacpro.github.io
GJL Game Parade Spring 2022 - https://itch.io/jam/game-parade-spring-2022
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool _sharedInstance;
    private List<GameObject> _pooledObjects;
    private List<GameObject> _activeObjects;
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private int _poolSize = 30;
    

    protected virtual void Awake()
    {
        _sharedInstance = this;
        _pooledObjects = new List<GameObject>();
        _activeObjects = new List<GameObject>();

        GameObject temp;
        for (int i = 0; i < _poolSize; i++)
        {
            temp = Instantiate(_objectPrefab, _objectPrefab.transform.position, _objectPrefab.transform.rotation);
            temp.SetActive(false);
            temp.gameObject.transform.SetParent(this.gameObject.transform, false);
            _pooledObjects.Add(temp);
        }    
    }

    public virtual GameObject GetObject()
    {
        for (int i=0; i < _poolSize; i++)
        {
            GameObject pooledObject = _pooledObjects[i];
            if(!pooledObject.activeInHierarchy)
            {
                pooledObject.SetActive(true);
                _activeObjects.Add(pooledObject);
                return pooledObject;
            }
        }   
        GameObject myObject = Instantiate(_objectPrefab, _objectPrefab.transform.position, _objectPrefab.transform.rotation);
        myObject.transform.SetParent(this.gameObject.transform, false);
        _pooledObjects.Add(myObject);
        _poolSize++;
        return myObject;
    }

    public List<GameObject> ResetActiveObjects()
    {
        int activeCount = _activeObjects.Count;

        List<GameObject> temp = _activeObjects;
        _activeObjects = new List<GameObject>();
        
        return temp;
    }
}