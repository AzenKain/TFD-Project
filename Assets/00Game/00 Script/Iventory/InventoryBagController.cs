using DG.Tweening.Core.Easing;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryBagController : Singleton<InventoryBagController>
{
    [SerializeField] Image _imageDetail;
    [SerializeField] Text _nameDetail;
    [SerializeField] Text _describeDetail;
    [SerializeField] GameObject _selectBorder;
    private RectTransform _rectBorder;
    [SerializeField] ItemInventoryBase _selectObject;
    [SerializeField] Slider _sliderObject;
    [SerializeField] GameObject _Delete_Drop;
    private Text[] _max_min_value;
    // Start is called before the first frame update
    void Start()
    {
        this._rectBorder = _selectBorder.GetComponent<RectTransform>();
        this._max_min_value = _sliderObject.GetComponentsInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_selectObject == null) return;
        if (_selectObject.gameObject.activeSelf)
            return;
        this._max_min_value[0].text = $"{_sliderObject.maxValue}";
        this._max_min_value[1].text = $"{_sliderObject.value}";
    }

    public void ShowDetailItems(GameObject g)
    {
        if (UIController.Instant.isUI == false)
            return;
        Debug.Log("OnClick");
        this._selectBorder.SetActive(true);
        this._selectBorder.transform.SetParent(g.transform.parent);
        this._selectBorder.transform.localPosition = Vector3.zero;
        this._selectBorder.transform.SetAsFirstSibling();
        this._rectBorder.sizeDelta = Vector2.zero;
        ItemInventoryBase iventory = g.GetComponent<ItemInventoryBase>();
        if (iventory == null)
        {
            return;
        }
        this._selectObject = iventory;
        ItemDataSO deb = iventory.GetDataItem();
        this._imageDetail.sprite = deb._image;
        this._nameDetail.text = deb._name;
        this._describeDetail.text = $"{deb._describe.Replace("\\n", Environment.NewLine)}";
    }
    public void OpenSlider()
    {
        _sliderObject.transform.parent.gameObject.SetActive(true);
        _sliderObject.maxValue = _selectObject.quantity;
        _sliderObject.minValue = 1;
        _sliderObject.value = 1;
        _Delete_Drop.gameObject.SetActive(false);
    }

    public void CloseSlider()
    {
        _sliderObject.transform.parent.gameObject.SetActive(false);
        _sliderObject.maxValue = 1;
        _sliderObject.minValue = 1;
        _Delete_Drop.gameObject.SetActive(true);
    }
    public void DeleteItems()
    {
        DataManager.Instant.RemoveItem(_selectObject, (int)(_sliderObject.value), DragController.Instant.isInBag);
        CloseSlider();
    }
    public void DisableSelectObject()
    {
        this._selectBorder.gameObject.SetActive(false);
        this._selectBorder.transform.SetParent(this.transform);
    }
}
