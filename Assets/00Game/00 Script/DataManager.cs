using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SimpleJSON;
using static UnityEditor.Progress;
using System;
using Unity.VisualScripting;
using UnityEngine.Playables;
using System.Security.Cryptography;

public class DataManager : Singleton<DataManager>
{
    [Header("----------------Items------------")]
    [SerializeField] ItemInventoryBase _prefabItemInventory;

    [SerializeField] List<ItemDataSO> _generalDataItems = new List<ItemDataSO>();
    public List<ItemDataSO> genaralDataItems => _generalDataItems;
   
    void Init()
    {
        string data= PlayerPrefs.GetString(CONSTANT.nameDataInventorySlider);
       

        var dataParsed = JSON.Parse(data);

        for (int i = 0; i < dataParsed.Count; i++)
        {
            int itemDataID = dataParsed[i]["ID"].AsInt;
            Debug.LogError(itemDataID +"--"+ dataParsed[i]["quantity"].AsInt);

            ItemDataSO itemData = this.getDataItemByID(itemDataID);
            if (itemData == null)
            {
                continue;
            }

            ItemInventoryBase _item = Instantiate(_prefabItemInventory).UpdateInfo(itemData).UpdateQuantity(dataParsed[i]["quantity"].AsInt); 
            InventoryManager.Instant.setItemOnInventory(_item, InventoryManager.Instant.itemSlot, InventoryManager.Instant.items, false);
        }

        string dataBag = PlayerPrefs.GetString(CONSTANT.nameDataInventoryBag);


        var dataParsedBag = JSON.Parse(dataBag);

        for (int i = 0; i < dataParsedBag.Count; i++)
        {
            int itemDataID = dataParsedBag[i]["ID"].AsInt;
            Debug.LogError(itemDataID + "--" + dataParsedBag[i]["quantity"].AsInt);
            ItemDataSO itemData = this.getDataItemByID(itemDataID);
            if (itemData == null)
            {
                continue;
            }

            ItemInventoryBase _item = Instantiate(_prefabItemInventory).UpdateInfo(itemData).UpdateQuantity(dataParsedBag[i]["quantity"].AsInt);
            InventoryManager.Instant.setItemOnInventory(_item, InventoryManager.Instant.itemSlotBag, InventoryManager.Instant.itemsBag, true);
        }


        for (int i = 0; i < InventoryManager.Instant.itemSlotShop.Count; i++)
        {

            ItemInventoryBase _item = Instantiate(_prefabItemInventory);
            InventoryManager.Instant.setItemOnShop(_item, InventoryManager.Instant.itemSlotShop, InventoryManager.Instant.itemsShop);
        }

    }

    void setUpDataPlayer()
    {
        string dataPlayer = PlayerPrefs.GetString(CONSTANT.nameDataPlayer);


        var dataParsedPlayer = JSON.Parse(dataPlayer);
        float maxHp = dataParsedPlayer["maxHp"].AsFloat;
        float tmpHp = dataParsedPlayer["tmpHp"].AsFloat;
        float speed = dataParsedPlayer["speed"].AsFloat;
        float armor = dataParsedPlayer["armor"].AsFloat;
        float dmg = dataParsedPlayer["dmg"].AsFloat;
        int gold = dataParsedPlayer["gold"].AsInt;
        GameManager.Instant.player.Init(maxHp, tmpHp, speed, armor, dmg, gold);
    }
    // Start is called before the first frame update
    void Start()
    {
        InventoryManager.Instant.Init();
        Init();
        IventorySliderController.Instant.updateSelectObj(0);
        setUpDataPlayer();
    }


    public void UpdateDataPlayer(string nameData, float data)
    {
        string dataPlayer = PlayerPrefs.GetString(CONSTANT.nameDataPlayer);


        var dataParsedPlayer = JSON.Parse(dataPlayer);

        dataParsedPlayer[nameData] = data;

        PlayerPrefs.SetString(CONSTANT.nameDataPlayer, dataParsedPlayer.ToString());

    }

    public void setupDataShop(string nameData)
    {
        string dataBag = PlayerPrefs.GetString(nameData);


        var dataParsedBag = JSON.Parse(dataBag);

        if (InventoryShopController.Instant._detailItem.Count < dataParsedBag.Count)
            return;

        for (int i = 0; i < dataParsedBag.Count; i++)
        {
            int itemDataID = dataParsedBag[i]["ID"].AsInt;
            Debug.LogError(itemDataID + "--" + dataParsedBag[i]["quantity"].AsInt);
            ItemDataSO itemData = this.getDataItemByID(itemDataID);
            if (itemData == null)
            {
                continue;
            }
            InventoryManager.Instant.itemsShop[i].UpdateInfo(itemData).UpdateQuantity(dataParsedBag[i]["quantity"].AsInt);
            InventoryShopController.Instant.UpdateDetailItem(itemData, i);
        }
    }


    public ItemDataSO getDataItemByID(int ID)
    {
        foreach (ItemDataSO item in _generalDataItems)
        {
            if (ID == item._ID)
                return item;
        }

        Debug.LogError($"ID {ID} not exist!");
        return null;
    }

    public void AddItem(ItemInventoryBase item, int num)
    {
        int numAfter = AddItemInterface(InventoryManager.Instant.items, item, num, InventoryManager.Instant.itemSlot, false);
        AddDataItemJSON(CONSTANT.nameDataInventorySlider, item, num, InventoryManager.Instant.itemSlot.Count);

        if (numAfter > 0)
        {
            AddItemInterface(InventoryManager.Instant.itemsBag, item, numAfter, InventoryManager.Instant.itemSlotBag, true);
            AddDataItemJSON(CONSTANT.nameDataInventoryBag, item, numAfter, InventoryManager.Instant.itemSlotBag.Count);
        }

    }

    int AddItemInterface(List<ItemInventoryBase> itemsData, ItemInventoryBase item, int num, List<Transform> slot, bool isInBag)
    { 
        for (int i = 0; i < itemsData.Count; i++)
        {
            if (itemsData[i].GetDataItem()._ID == item.GetDataItem()._ID)
            {
                if (itemsData[i].quantity + num <= itemsData[i].maxCapacity && item.GetDataItem()._isStack)
                {
                    itemsData[i].UpdateQuantity(itemsData[i].quantity + num);
                    return -1;
                }
                else if (item.GetDataItem()._isStack)
                {
                    num = itemsData[i].quantity + num - itemsData[i].maxCapacity;
                    itemsData[i].UpdateQuantity(itemsData[i].maxCapacity);
                } 
            }

        }

        if (itemsData.Count >= slot.Count)
            return num;

        ItemInventoryBase _item = Instantiate(_prefabItemInventory).UpdateInfo(item.GetDataItem()).UpdateQuantity(num).UpdateRectTransform(Vector2.zero);
        InventoryManager.Instant.setItemOnInventory(_item, slot, itemsData, isInBag);

        return -1;
    }
    int AddDataItemJSON(string nameData, ItemInventoryBase item, int num, int maxData)
    {
        string data = PlayerPrefs.GetString(nameData);


        var dataParsed = JSON.Parse(data);

        for (int i = 0; i < dataParsed.Count; i++)
        {
            int itemDataID = dataParsed[i]["ID"].AsInt;
            int itemDataQuantity = dataParsed[i]["quantity"].AsInt;

            if (item.GetDataItem()._ID == itemDataID)
            {
                if ((itemDataQuantity + num) <= item.maxCapacity && item.GetDataItem()._isStack)
                {
                    dataParsed[i]["quantity"] = itemDataQuantity + num;
                    PlayerPrefs.SetString(nameData, dataParsed.ToString());

                    return -1;
                }
                else if (item.GetDataItem()._isStack)
                {
                    dataParsed[i]["quantity"] = item.maxCapacity;
                    num = itemDataQuantity + num - item.maxCapacity;
                }

            }

        }

        if (dataParsed.Count >= maxData)
            return num;

        JSONObject moreItem = new JSONObject();
        moreItem["ID"] = item.GetDataItem()._ID;
        moreItem["quantity"] = num;
        dataParsed.Add(moreItem);
        PlayerPrefs.SetString(nameData, dataParsed.ToString());

        return -1;
    }
   
    int getIndexItemJSON(string nameData, ItemInventoryBase items, int exeption = -1)
    {

        string data = PlayerPrefs.GetString(nameData);

        var dataParsed = JSON.Parse(data);

        for (int i = 0; i < dataParsed.Count; i++)
        {
            int itemDataID = dataParsed[i]["ID"].AsInt;
            int itemDataQuantity = dataParsed[i]["quantity"].AsInt;
            if (items.GetDataItem()._ID == itemDataID
                && items.quantity == itemDataQuantity)
            {
                if (i == exeption)
                    continue;
                return i;
            }

        }
        return - 1;
    }
    public void SwitchDataJSON(string nameDataIn, string nameDataOut, ItemInventoryBase item)
    {
        string dataIn = PlayerPrefs.GetString(nameDataIn);

        var dataInParsed = JSON.Parse(dataIn);

        int index = getIndexItemJSON(nameDataIn, item);
        if (index == -1)
            return;

        string dataOut = PlayerPrefs.GetString(nameDataOut);

        var dataOutParsed = JSON.Parse(dataOut);

        dataOutParsed.Add(dataInParsed[index]);
        dataInParsed.Remove(index);

        PlayerPrefs.SetString(nameDataIn, dataInParsed.ToString());
        PlayerPrefs.SetString(nameDataOut, dataOutParsed.ToString());
    }

    public void UpdateMergeItemJSON(ItemInventoryBase a, ItemInventoryBase b)
    {

        bool isBagA = DragController.Instant.isToBag, isBagB = DragController.Instant.isInBag;

        string data = PlayerPrefs.GetString(CONSTANT.nameDataInventorySlider);

        var dataParsed = JSON.Parse(data);

        string dataBag = PlayerPrefs.GetString(CONSTANT.nameDataInventoryBag);

        var dataParsedBag = JSON.Parse(dataBag);


        int indexA = isBagA == true ? getIndexItemJSON(CONSTANT.nameDataInventoryBag, a) : getIndexItemJSON(CONSTANT.nameDataInventorySlider, a);
        int indexB = isBagB == true ? getIndexItemJSON(CONSTANT.nameDataInventoryBag, b, indexA) : getIndexItemJSON(CONSTANT.nameDataInventorySlider, b, indexA);


        if (indexA == -1 || indexB == -1)
            return;

        if (a.quantity + b.quantity <= 64)
        {
            if (isBagA == true && isBagB == true)
            {
                dataParsedBag[indexA]["quantity"] = a.quantity + b.quantity;
                dataParsedBag.Remove(indexB);
            }
            else if (isBagA == false && isBagB == false)
            {
                dataParsed[indexA]["quantity"] = a.quantity + b.quantity;
                dataParsed.Remove(indexB);
            }
            else if (isBagA == true && isBagB == false)
            {
                dataParsedBag[indexA]["quantity"] = a.quantity + b.quantity;
                dataParsed.Remove(indexB);
            }
            else
            {
                dataParsed[indexA]["quantity"] = a.quantity + b.quantity;
                dataParsedBag.Remove(indexB);
            }
        }
        else
        {
            if (isBagA == true && isBagB == true)
            {
                dataParsedBag[indexA]["quantity"] = 64;
                dataParsedBag[indexB]["quantity"] = a.quantity + b.quantity - 64;
            }
            else if (isBagA == false && isBagB == false)
            {
                dataParsed[indexA]["quantity"] = 64;
                dataParsed[indexB]["quantity"] = a.quantity + b.quantity - 64;
            }
            else if (isBagA == true && isBagB == false)
            {
                dataParsedBag[indexA]["quantity"] = 64;
                dataParsed[indexB]["quantity"] = a.quantity + b.quantity - 64;
            }
            else
            {
                dataParsed[indexA]["quantity"] = 64;
                dataParsedBag[indexB]["quantity"] = a.quantity + b.quantity - 64;
            }
        }
        PlayerPrefs.SetString(CONSTANT.nameDataInventorySlider, dataParsed.ToString());
        PlayerPrefs.SetString(CONSTANT.nameDataInventoryBag, dataParsedBag.ToString());
    }
    public bool RemoveItem(ItemInventoryBase item, int num, bool isBag)
    {
        string nameData = isBag ? CONSTANT.nameDataInventoryBag : CONSTANT.nameDataInventorySlider;

        RemoveDataItemJSON(nameData, item, num);
        return RemoveItemInterface(item, num);

    }
    public bool RemoveItemInterface(ItemInventoryBase item, int num)
    {

        if (item.quantity - num > 0)
        {
            item.UpdateQuantity(item.quantity - num);
            return true;
        }

        Destroy(item.gameObject);
        return false;
        

    }
    public void RemoveDataItemJSON(string nameData, ItemInventoryBase item, int num)
    {
        string data = PlayerPrefs.GetString(nameData);


        var dataParsed = JSON.Parse(data);

        int index = getIndexItemJSON(nameData, item);
        Debug.Log(index);
        if (index == -1)
            return;
        
        int itemDataQuantity = dataParsed[index]["quantity"].AsInt;
        if ((itemDataQuantity - num) <= 0)
        {
            dataParsed.Remove(index);
        }
        else
        {
            dataParsed[index]["quantity"] = itemDataQuantity - num;

        }
        PlayerPrefs.SetString(nameData, dataParsed.ToString());
    }


    public void DropDataItem(ItemInventoryBase item)
    {

    }
    private void OnDrawGizmosSelected()
    {
        _generalDataItems = Resources.LoadAll<ItemDataSO>("Items").ToList();
    }
}
