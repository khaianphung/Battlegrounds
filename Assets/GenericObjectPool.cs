using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool : MonoBehaviour {

    public static GenericObjectPool current;
    public GameObject pooledObject;
    public int pooledAmount = 1;
    public bool willGrow = true;

    List<GameObject> objectPool;

    void Awake()
    {
        current = this;
    }

    void Start() {
        objectPool= new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            objectPool.Add(obj);
        }

    }
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            objectPool.Add(obj);
            return obj;
        }
        return null;
    }
}
	
