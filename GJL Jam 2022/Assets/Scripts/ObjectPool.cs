using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool _sharedInstance;
    public List<GameObject> _pooledObjects;
    public List<GameObject> _activeObjects;
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private int _initialPoolSize = 30;
    

    private void Awake()
    {
        _sharedInstance = this;
    }

    private void Start() {
        _pooledObjects = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < _initialPoolSize; i++)
        {
            temp = Instantiate(_objectPrefab, _objectPrefab.transform.position, _objectPrefab.transform.rotation);
            temp.SetActive(false);
            temp.gameObject.transform.SetParent(this.gameObject.transform, false);
            _pooledObjects.Add(temp);
        }    
    }

    public virtual GameObject GetObject()
    {
        for (int i=0; i < _initialPoolSize; i++)
        {
            GameObject pooledObject = _pooledObjects[i];
            if(!pooledObject.activeInHierarchy)
            {
                pooledObject.SetActive(pooledObject);
                _activeObjects.Add(pooledObject);
                return pooledObject;
            }
        }   
        GameObject myObject = Instantiate(_objectPrefab, _objectPrefab.transform.position, _objectPrefab.transform.rotation);
        myObject.transform.SetParent(this.gameObject.transform, false);
        _pooledObjects.Add(myObject);
        _activeObjects.Add(myObject);
        return myObject;
    }

    public GameObject RemoveObject()
    {
        int activeCount = _activeObjects.Count;

        if (activeCount == 0) return null;
        
        GameObject temp = _activeObjects[activeCount - 1];

        temp.SetActive(false);
        _activeObjects.RemoveAt(activeCount - 1);
        return temp;
    }
}