using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPlacer : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The options for possible terrains")]
    [SerializeField] private GameObject[] _terrain;
    
    private List<GameObject> _currentTerrains = new List<GameObject>();
    
    private Transform _objectPos;
    void Start()
    {
        Debug.Log("Terrain placer is ready");

        _objectPos = Managers.Instance.GameManager.MainCharacter.GetComponent<Transform>();

        firstTerrain();
    }

    void Update()
    {
        if (_objectPos.position.z > _currentTerrains[_currentTerrains.Count - 1].transform.position.z)
        {
            terrainSpawn();
        }
        if(_currentTerrains.Count >= 3)
        {
            Destroy(_currentTerrains[0]);
            _currentTerrains.RemoveAt(0);
        }
        if ((_objectPos.position.z - _currentTerrains[0].transform.position.z) < 0)
        {
            firstTerrain();
        }
    }

    void terrainSpawn()
    {
        GameObject currentTerrain = Instantiate(_terrain[Random.Range(0, 2)]);
        currentTerrain.transform.position = _currentTerrains[_currentTerrains.Count - 1].transform.position + new Vector3(0f, 0f, 1000f);
        _currentTerrains.Add(currentTerrain);
    }

    void firstTerrain()
    {
        for (int i = 0; i < _currentTerrains.Count; i++)
        {
            Destroy(_currentTerrains[i]);
        }
        _currentTerrains.Clear();

        GameObject firstTerrain = Instantiate(_terrain[Random.Range(0, 2)]);
        firstTerrain.transform.position = new Vector3(0f, 0f, 0f);
        _currentTerrains.Add(firstTerrain);
    }
}
