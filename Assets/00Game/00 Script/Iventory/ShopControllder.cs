using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopControllder : Interactable
{
    [SerializeField] string _nameDataShop;
    [SerializeField] string _nameShop;
    [SerializeField] Sprite _imgShop;

    public override void OnEndInteract()
    {
        return;
    }

    public override void OnStartInteract()
    {
        if (this._isInRange == true && this._isInteract == false)
            if (Input.GetKeyDown(_interactKey))
            {
                this._isInteract = true;

                UIController.Instant.OpenShop(_nameDataShop, _nameShop, _imgShop);
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        this._isInRange = false;
        this._interactKey = KeyCode.F;
    }

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

    }
}
