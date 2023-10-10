using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalItem : ItemInventoryBase
{
    [SerializeField] Image _imageItem;
    [SerializeField] Text _quantityItem;

    // Start is called before the first frame update
    void Start()
    {
        if (_info == null)
            return;
        _imageItem = GetComponent<Image>(); 
        _quantityItem = GetComponentInChildren<Text>();
        _imageItem.sprite = _info._image;
        _quantityItem.text = this._quantity.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override ItemInventoryBase UpdateInfo(ItemDataSO itemInfo)
    {
        base.UpdateInfo(itemInfo);
        updateView();

        return this;
    }

    public override ItemInventoryBase UpdateQuantity(int newQuantity)
    {
        base.UpdateQuantity(newQuantity);
        updateView();

        return this;
    }

    void updateView()
    {
        if (_info == null)
            return;
        _imageItem = GetComponent<Image>();
        _quantityItem = GetComponentInChildren<Text>();
        _imageItem.sprite = _info._image;
        _quantityItem.text = this._quantity.ToString();
    }

    private void OnDrawGizmosSelected()
    {
        this.updateView();
    }

    public override ItemDataSO GetDataItem()
    {
        if (_info == null)
            return null;
        return _info;
    }
}
