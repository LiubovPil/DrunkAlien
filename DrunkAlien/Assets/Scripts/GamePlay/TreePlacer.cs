using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TreePool))]

public class TreePlacer : MonoBehaviour
{
    private bool _isSpawning = false;
    private bool _allowedSpawn = false;

    //support for parametres of target
    private float _speedOfAlienZ;
    private float _speedOfAlienX;
    private float _sizeOfAlienX;

    //support for position of tree
    private List<int> _positionX = new List<int>();
    private int _widthOfWindow;
    private int _countPosX;

    private int _currentPosZStart;
    private int _countPosZEnd;

    private int _distanceBetweenTwoTreesZ = 0;

    //support for tree size
    private float _treeWidth;
    private float _treeScale;

    //support for tree counting
    private int _levelNumberOfTree;
    private int _totalTreeCount;

    //support for spawning area (from Game Manager)
    private int _leftSide;
    private int _rightSide;
    private int _totalPath;

    private void Awake()
    { 
        Managers.Instance.EventManager.StartListening(EventNames.ResetSceneEvent, ResetScene);
        Managers.Instance.EventManager.StartListening(EventNames.MovingStartEvent, Initialized);
    }

    private void ResetScene(StageName stageIn)
    {
        _allowedSpawn = false;
        _isSpawning = false;

        StopAllCoroutines();

        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");

        if (trees != null)
        {
            foreach (GameObject tree in trees)
            {
                TreePool.Instance.ReturnToPool(tree);
            }
        }
        _positionX.Clear();
        _distanceBetweenTwoTreesZ = 0;
        _totalTreeCount = 0;

        _isSpawning = false;
    }
    private void Initialized(StageName stageIn)
    {
        _treeWidth = TreePool.Instance.TreeWidth;
        _treeScale = TreePool.Instance.TreeScale;

        _levelNumberOfTree = Managers.Instance.GameManager.LevelTreeNumber;

        _leftSide = Managers.Instance.GameManager.LeftPositionX;
        _rightSide = Managers.Instance.GameManager.RightPositionX;

        _totalPath = Managers.Instance.GameManager.TotalPathLenght;

        GameObject alien = Managers.Instance.GameManager.MainCharacter;
        if (alien != null)
        {
            _sizeOfAlienX = alien.GetComponent<BoxCollider>().size.x;
            _speedOfAlienZ = alien.GetComponent<DrunkAlienMovement>().CurrentSpeed;
            _speedOfAlienX = alien.GetComponent<DrunkAlienMovement>().MoveXFoce;
        }
        else
        {
            Debug.LogWarning("Object alien isn't found");
        }

        CalculateXPosition();
        CalculateZPosition();

        _allowedSpawn = true;
    }

    private void Update()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        _totalTreeCount = trees.Length;

        if (!_isSpawning && (_totalTreeCount < _levelNumberOfTree) && _allowedSpawn)
        {
            _isSpawning = true;
            StartCoroutine(SetTree());
        }
    }

    private IEnumerator SetTree()
    {
        while (_totalTreeCount < _levelNumberOfTree + 1)
        {
            int setPositions = (int)Random.Range(1, (Mathf.Pow(2, _countPosX) - 1));
            int[] arraySetPositions = new int[1] {setPositions};

            BitArray bitSetPositions = new BitArray( arraySetPositions );

            for (int i = 0; i < _countPosX; i++)
            {
                if (bitSetPositions[i] == true && _currentPosZStart < _countPosZEnd)
                {
                    Vector3 newTreePosition = new Vector3(_positionX[i], 0, _currentPosZStart * _distanceBetweenTwoTreesZ);
                    PutTree(newTreePosition);
                }
            }
            _currentPosZStart++;

            if (_currentPosZStart >= _countPosZEnd)
            {
                _currentPosZStart = _countPosZEnd;
            }
            yield return null;
        }
        _isSpawning = false;
    }
    private void PutTree(Vector3 position)
    {
        GameObject tree = TreePool.Instance.GetFromPool();
        tree.transform.position = position;
        tree.SetActive(true);
    }
    #region Calculation
    /// <summary>
    /// This method determines the minimum z distance between tree
    /// </summary>
    private void CalculateZPosition()
    {
        float sumSpeed = Mathf.Sqrt((_speedOfAlienX * _speedOfAlienX) + (_speedOfAlienZ * _speedOfAlienZ)); //the total speed at which the target is moving
        float time = (_rightSide - _leftSide) / sumSpeed; //the time it takes to move from the far right to the far left
        float totalZ = time * _speedOfAlienZ;

        _distanceBetweenTwoTreesZ = (int)Mathf.Round(totalZ);
        Debug.Log("_distanceBetweenTwoTreesZ " + _distanceBetweenTwoTreesZ);

        _currentPosZStart = 3; //the position of first tree
        _countPosZEnd = (int)(_totalPath/_distanceBetweenTwoTreesZ) + 2;

        Debug.Log("Count Z Start = " + _currentPosZStart);
        Debug.Log("Count Z End = " + _countPosZEnd);
    }
    /// <summary>
    /// This method determines the possible x-positions given the width of the tree and the size of the target
    /// </summary>
    private void CalculateXPosition()
    {
        _widthOfWindow = (int)(2f * (_sizeOfAlienX * 0.2f + _treeWidth * _treeScale)); //This is the width of the target and the width of the tree

        Debug.Log("widthOfWindow" + _widthOfWindow);
        int tempXPos = (int)(_leftSide + _treeWidth * _treeScale);
        
        while (tempXPos <= _rightSide)
        {
            _positionX.Add(tempXPos);
            Debug.Log("New x position of tree" + tempXPos);
            tempXPos += _widthOfWindow;
        }

        _countPosX = _positionX.Count;
        Debug.Log("The number of x position of tree" + _countPosX);
    }
    #endregion
}
