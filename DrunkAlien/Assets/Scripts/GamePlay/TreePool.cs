using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePool : MonoBehaviour
{
    static private bool _initializied = false;

    static private TreePool _instance = null;

    static private List<GameObject> _pool;
    
    private GameObject _tree;

    private int _poolCapacity = 18;
    public static TreePool Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("There is no tree pool");
            }
            return _instance;
        }
    }
    public float TreeWidth
    {
        get { return Resources.Load<GameObject>("TreeModel").GetComponent<CapsuleCollider>().radius; }
    }

    public float TreeScale
    {
        get { return Resources.Load<GameObject>("TreeModel").GetComponent<Transform>().localScale.x; }
    }

    private void Awake()
    {
        _instance = this;

        if(!_initializied)
        {
            Initialize();
        }
    }
    private void Initialize()
    {
        _tree = Resources.Load<GameObject>("TreeModel");
        if (_tree == null)
        {
            Debug.LogWarning("Tree model isn't found");
        }

        _pool = new List<GameObject>();

        for (int i = 0; i < _poolCapacity; i++)
        {
            GameObject newTree = GameObject.Instantiate(_tree);
            newTree.SetActive(false);

            GameObject.DontDestroyOnLoad(newTree); //objects will be saved during transition through the scenes
            _pool.Add(newTree);
        }
        _initializied = true;
    }

    public GameObject GetFromPool()
    {
        for (int i = 0; i < _poolCapacity; i++)
        {
            if(!_pool[i].activeInHierarchy)
            {
                return _pool[i];
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject treeUsed)
    {
        treeUsed.SetActive(false);
        _pool.Add(treeUsed);
    }
}
