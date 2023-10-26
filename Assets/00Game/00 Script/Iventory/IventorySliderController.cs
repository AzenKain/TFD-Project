using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class IventorySliderController : Singleton<IventorySliderController> 
{

    // Start is called before the first frame update
    public int _pastSelectObj { get; private set; } = -1;
    [SerializeField] Text _nameItem;
    [SerializeField] GameObject _selectBorder;
    [SerializeField] ItemInventoryBase _selectObject;
    private RectTransform _rectBorder;
    void Start()
    {
        this._rectBorder = _selectBorder.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateSelectObj(int index)
    {
        this._selectBorder.gameObject.SetActive(true);
        if (index == this._pastSelectObj)
            return;

        if (UIController.Instant.isUI)
            return;

        Transform parent = this.transform;
        Transform currentSelect = parent.GetChild(index);
        _selectBorder.transform.SetParent(currentSelect);
        _selectBorder.transform.localScale = Vector3.one;
        _selectBorder.transform.localPosition = Vector3.zero;
        _selectBorder.transform.SetAsFirstSibling();
        this._rectBorder.sizeDelta = Vector2.zero;
        this._pastSelectObj = index;
        this.addItem(index);
    }
    void disableAllItem()
    {
        PlayerController player = GameManager.Instant.player;
        if (player.transform.childCount > 1)
            for (int i = 1; i < player.transform.childCount; i++)
            {
                MedicineBase medicine = player.transform.GetChild(i).gameObject.GetComponent<MedicineBase>();
                if (medicine != null && medicine.IsUse() == true)
                {
                    medicine.setUI(false);
                    continue;

                }


                player.transform.GetChild(i).gameObject.SetActive(false);
            }
    }

    public void addItem(int index)
    {
        Transform parent = this.transform;
        Transform currentSelect = parent.GetChild(index);
        PlayerController player = GameManager.Instant.player;
        this.disableAllItem();


        if (currentSelect.childCount <= 3)
        {
            this._nameItem.text = "";
            this._selectObject = null;
            return;
        }

        ItemInventoryBase iventory = currentSelect.GetChild(currentSelect.childCount - 1).gameObject.GetComponent<ItemInventoryBase>();


        if (iventory == null)
        {
            this._nameItem.text = "";
            this._selectObject = null;
            return;
        }
        this._selectObject = iventory;
        this._nameItem.text = iventory.GetDataItem()._name;
        
        if (iventory.GetDataItem()._weapon != null)
        {
            WeaponBase item = ObjectPooling.Instant.getComp<WeaponBase>(iventory.GetDataItem()._weapon);
            if (item.gameObject.activeSelf == false) item.gameObject.SetActive(true);
            item.transform.SetParent(player.transform);
            item.transform.localPosition = Vector3.zero;
        }
        else
        {
            MedicineBase item = ObjectPooling.Instant.getComp<MedicineBase>(iventory.GetDataItem()._medicine);
            if (item.gameObject.activeSelf == false) item.gameObject.SetActive(true);
            item.transform.SetParent(player.transform);
            item.transform.localPosition = Vector3.zero;
            item.setUI(true);
        }

        player.updateInUse();
    }
    public void RemoveItemsInSlider(int num)
    {
        if (this._selectObject == null)
            return;
        if (DataManager.Instant.RemoveItem(_selectObject, num, false))
            return;
        this.disableAllItem();
    }
    public void DisableSelectObject()
    {
        this._selectBorder.gameObject.SetActive(false);
    }
}
