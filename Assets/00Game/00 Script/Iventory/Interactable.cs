using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected bool _isInRange;
    [SerializeField] protected bool _isInteract;
    [SerializeField] protected KeyCode _interactKey;
    [SerializeField] protected UnityEvent _interactAction;
    [SerializeField] protected GameObject _UiAction;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnStartInteract();

        OnEndInteract();
    }

    public abstract void OnStartInteract();
    public abstract void OnEndInteract();
}
