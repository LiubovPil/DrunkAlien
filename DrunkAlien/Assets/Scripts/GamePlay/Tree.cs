using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private Transform _mainCharacter;

    private int _offsetZ = 50;
    //private void OnBecameVisible()
    private void Awake()
    {
        Managers.Instance.EventManager.StartListening(EventNames.MovingStartEvent, FindMainCharacter);
    }
    //private void OnBecameInvisible()
    private void Update()
    {
        if (transform.position.z <= _mainCharacter.position.z - _offsetZ)
        {
            TreePool.Instance.ReturnToPool(this.gameObject);
        }
    }
    private void FindMainCharacter(StageName stageIn)
    {
        _mainCharacter = Managers.Instance.GameManager.MainCharacter.GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Alien")
        {
            Managers.Instance.AudioManager.PlaySound(AudioClipName.FailedSound);
            TreePool.Instance.ReturnToPool(this.gameObject);
            Managers.Instance.EventManager.TriggerEvent(EventNames.CowLostEvent, Managers.Instance.GameManager.CurrentStage);
        }
    }
}
