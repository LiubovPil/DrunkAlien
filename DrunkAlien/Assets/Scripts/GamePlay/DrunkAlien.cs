using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkAlien : MonoBehaviour
{
    private void Awake()
    {
        GetComponentInChildren<MeshRenderer>().enabled = true;
    }
}
