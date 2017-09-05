using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PooledObject
{
    public GameObject pooledObject;
    public int poolCount;
    public List<GameObject> pool;

    public PooledObject(GameObject obj, int count, List<GameObject> list)
    {
        this.pooledObject = obj;
        this.poolCount = count;
        this.pool = list;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }

    public void IncrementCount()
    {
        this.poolCount++;
    }
}

public class ObjectPool : MonoBehaviour {

    //*****private Varibles*****
    //private List<GameObject> pooledObject = new List<GameObject>();    //The GameObject to be pooled
    //private List<int> poolCount = new List<int>();                   //The number of pooledObject
    //private List<List<GameObject>>[] pool = new List<List<GameObject>>();       //The pool, collection of Instantiated Gameobjects
    private List<PooledObject> pool = new List<PooledObject>();
    private bool expandIfEmpty = true;
    
    //Property for length of pool
    public int _OBP_PoolLength
    {
        get
        {
            return pool.Count;
        }
    }

    /// <summary>
    /// Instantiates another GameObject to add to pool with index
    /// </summary>
    /// <param name="index">The ith pool item in the list to expand</param>
    /// <returns></returns>
    private GameObject ExpandList(int index)
    {
        GameObject newGO = Instantiate(pool[index].pooledObject, transform.position, transform.rotation);
        pool[index].pool.Add(newGO);
        pool[index].IncrementCount();
        newGO.SetActive(false);
        return (newGO);
    }

    /// <summary>
    /// Creates and returns list of instatiated gameobjects
    /// </summary>
    /// <param name="ob">GameObject to pool</param>
    /// <param name="count">Number of GameObjects to pool</param>
    /// <returns>List<GameObject> list of instantiated GameObjects</returns>
    private List<GameObject> CreatePool(GameObject ob, int count)
    {
        List<GameObject> returnPool = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject newGO = Instantiate(ob, this.transform.position, transform.rotation);
            newGO.SetActive(false);
            returnPool.Add(newGO);
        }

        return returnPool;
    }

    /// <summary>
    /// This function has to be called inorder to pool objects.
    /// </summary>
    /// <param name="ob"></param>
    /// <param name="count"></param>
    /// <param name="expand"></param>
	public void _OBP_ConstructObjectPool(GameObject ob, int count, bool expand = true)
    {
        this.expandIfEmpty = expand;
        pool.Add(new PooledObject(ob, count, CreatePool(ob, count)));
    }

    public void _OBP_ConstructObjectPool(GameObject[] ob, int count, bool expand = true)
    {
        for (int i = 0; i < ob.Length; i++)
        {
            pool.Add(new PooledObject(ob[i], count, CreatePool(ob[i], count)));
        }
        this.expandIfEmpty = expand;
    }
    
    /// <summary>
    /// Adds another pool of gameobjects to the list
    /// </summary>
    /// <param name="ob"></param>
    /// <param name="count"></param>
    public void _OBP_AddPooledObject(GameObject ob, int count)
    {
        pool.Add(new PooledObject(ob, count, CreatePool(ob, count)));
    }

    /// <summary>
    /// Retrieves an inactive pooled object
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject _OBP_GetPooledObject(int index = 0)
    {
        if (index > pool.Count)
        {
            index = pool.Count - 1;
        }
        GameObject obj = pool[index].GetPooledObject();
        if (obj != null)
        {
            return obj;
        }
        else if (expandIfEmpty)
        {
            return ExpandList(index);
        }
        return null;
    }
    
    /// <summary>
    /// Passes reference of pool to GameManager to be destroyed.
    /// </summary>
    private void _OBP_DestroyPool()
    {
        GameManager.instance.GarbageCollectPooledObjects(pool);
    }

    public void OnDestroy()
    {
        _OBP_DestroyPool();
    }
}
