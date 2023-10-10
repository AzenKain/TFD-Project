using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ChestController : Interactable
{
    [SerializeField] Animator _animator;
    [SerializeField] int _goldSpawm;
    public override void OnEndInteract()
    {
        return;
    }

    public override void OnStartInteract()
    {
        if (this._isInRange == true)
            if (Input.GetKeyDown(_interactKey))
            {
                if (this._isInteract == true)
                    return;

                this._isInteract = true;
                _animator.SetTrigger("Open");


                GameManager.Instant.SpawmGold(_goldSpawm, this.transform.position);
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        this._animator = GetComponentInChildren<Animator>();
        this._isInRange = false;
        this._interactKey = KeyCode.F;
    }


    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;
        this._UiAction.SetActive(true);
        if (this._isInRange == true)
            return;

        this._isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;
        this._UiAction.SetActive(false);
        if (this._isInRange == false)
            return;
        this._isInRange = false;
        if (this._isInteract == false)
            return;
        this._isInteract = false;
        _animator.SetTrigger("Close");
    }
}
