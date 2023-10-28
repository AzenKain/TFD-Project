using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DragController : Singleton<DragController>
{
    [SerializeField] GameObject _movingItem;
    [SerializeField]
    Transform _parent;

    Transform _targetSlot;
    public bool isToBag { get; private set; } = false;
    public bool isInBag { get; private set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_movingItem == null)
            return;
        Vector2 mousePos = Input.mousePosition;
        _movingItem.transform.position = mousePos;

        _targetSlot = null;
        foreach (Transform t in InventoryManager.Instant.itemSlot)
        {
      
            if(Vector2.Distance(_movingItem.transform.position,t.position)<= 50f)
            {
                this.isToBag = false;
                _targetSlot = t;
            }
        }

        foreach (Transform t in InventoryManager.Instant.itemSlotBag)
        {

            if (Vector2.Distance(_movingItem.transform.position, t.position) <= 50f)
            {
                this.isToBag = true;
                _targetSlot = t;
            }
        }
    }

    public void setMovingItem(GameObject g, bool isInBag)
    {
        Debug.Log("set moving item");

        _movingItem = g;
        _parent = g.transform.parent;
        if (_parent == this.transform)
            return;
        g.transform.SetParent(this.transform);



        this.isInBag = isInBag;
    }

    public void removeMovingItem()
    {
        if(_movingItem != null)
        {
            CheckTargetSlot();
        }
        _movingItem = null;
        _targetSlot = null;

    }

    void CheckTargetSlot()
    {
        if(_targetSlot == null)
        {
            _movingItem.transform.SetParent(_parent);
            _movingItem.transform.localPosition = Vector3.zero;
            return;
        }

        ItemInventoryBase itemInSlot = null;
        if (_targetSlot != null && _targetSlot.childCount > 2)
        {
            itemInSlot = _targetSlot.GetComponentInChildren<ItemInventoryBase>();
        }

        ItemInventoryBase movingItem = _movingItem.GetComponent<ItemInventoryBase>();
        RectTransform recMovingItem = _movingItem.GetComponent<RectTransform>();

        if (itemInSlot == null)
        {
            if (isToBag == true && isInBag == false)
            {
                InventoryManager.Instant.setItemOnInventory(movingItem, InventoryManager.Instant.itemSlotBag, InventoryManager.Instant.itemsBag, true);
                DataManager.Instant.SwitchDataJSON(CONSTANT.nameDataInventorySlider, CONSTANT.nameDataInventoryBag, movingItem);
            }
            else if (isToBag == false && isInBag == true)
            {
                InventoryManager.Instant.setItemOnInventory(movingItem, InventoryManager.Instant.itemSlot, InventoryManager.Instant.items, false);
                DataManager.Instant.SwitchDataJSON(CONSTANT.nameDataInventoryBag, CONSTANT.nameDataInventorySlider, movingItem);
            }    


            _movingItem.transform.SetParent(_targetSlot);
            _movingItem.transform.localPosition = Vector3.zero;
            movingItem.UpdateRectTransform(new Vector2(-32, -32));
            return;
        }

        if(itemInSlot.quantity + movingItem.quantity <= itemInSlot.maxCapacity 
            && movingItem.GetDataItem()._isStack 
            && movingItem.GetDataItem()._ID == itemInSlot.GetDataItem()._ID)
        {
            DataManager.Instant.UpdateMergeItemJSON(itemInSlot, movingItem);
            Destroy(_movingItem.gameObject);
            itemInSlot.UpdateQuantity(itemInSlot.quantity + movingItem.quantity);
            return;
        }

        if (movingItem.GetDataItem()._isStack && movingItem.GetDataItem()._ID == itemInSlot.GetDataItem()._ID)
        {
            DataManager.Instant.UpdateMergeItemJSON(itemInSlot, movingItem);
            movingItem.UpdateQuantity((itemInSlot.quantity + movingItem.quantity) - itemInSlot.maxCapacity);
            itemInSlot.UpdateQuantity(itemInSlot.maxCapacity);
        }


        _movingItem.transform.SetParent(_parent);
        _movingItem.transform.localPosition = Vector3.zero;
        movingItem.UpdateRectTransform(new Vector2(-32, -32));

    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
