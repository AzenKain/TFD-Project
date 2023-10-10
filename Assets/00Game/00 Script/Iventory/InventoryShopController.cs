using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryShopController : Singleton<InventoryShopController>
{ 
    [SerializeField] ItemInventoryBase _selectObject;
    [SerializeField] GameObject _selectBorder;
    [SerializeField] Button _buyButton;
    [SerializeField] Image _imageShop;
    [SerializeField] Text _titleShop;
    [SerializeField] Text _curentGold;
    private RectTransform _rectBorder;

    public List<List<Text>> _detailItem { get; private set; } = new List<List<Text>>();

    private void Awake()
    {
        this._buyButton.onClick.AddListener(BuyItem);
        this.setUpDetailItem();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void setUpDetailItem()
    {
        for  (int i = 0; i < this.transform.GetChild(2).childCount; i++)
        {
            _detailItem.Add(this.transform.GetChild(2).GetChild(i).GetComponentsInChildren<Text>().ToList());
        }
    }
    public void Init(string nameShop, Sprite img) {

        this._imageShop.sprite = img;
        this._titleShop.text = nameShop;
        this._curentGold.text = $"{GameManager.Instant.player._gold}$";
    }
    public void ChoiceItem(GameObject g)
    {

        _selectObject = g.GetComponent<ItemInventoryBase>();
        if (_selectObject == null)
        {
            return;
        }
        this._selectBorder.SetActive(true);
        this._selectBorder.transform.SetParent(g.transform.parent);
        this._selectBorder.transform.localPosition = Vector3.zero;
        this._selectBorder.transform.SetAsFirstSibling();
        this._selectBorder.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
    }
    public void BuyItem()
    {
        if (this._selectObject == null)
            return;

        if (GameManager.Instant.player._gold < this._selectObject.GetDataItem()._cost)
            return;

        DataManager.Instant.AddItem(_selectObject, _selectObject.quantity);

        GameManager.Instant.player.setGold((GameManager.Instant.player._gold - this._selectObject.GetDataItem()._cost));
        this._curentGold.text = $"{GameManager.Instant.player._gold}$";
    }
    public void DisableSelectObject()
    {
        this._selectBorder.gameObject.SetActive(false);
        this._selectBorder.transform.SetParent(this.transform);
    }

    public void AddDetailItem(List<Text> detail)
    {
        this._detailItem.Add(detail);
    }

    public void UpdateDetailItem(ItemDataSO detail, int index)
    {
        _detailItem[index][0].text = $"{detail._name}";
        _detailItem[index][1].text = $"{detail._cost}$";
    }
}
