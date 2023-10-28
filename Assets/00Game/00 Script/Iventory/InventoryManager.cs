using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] Transform _gridLayout;

    [SerializeField] List<ItemInventoryBase> _items = new List<ItemInventoryBase>();
    public List<ItemInventoryBase> items => _items;

    [SerializeField]
    List<Transform> _itemSlots = new List<Transform>();
    public List<Transform> itemSlot => _itemSlots;



    [SerializeField] Transform _gridLayoutBag;

    [SerializeField] List<ItemInventoryBase> _itemsBag = new List<ItemInventoryBase>();
    public List<ItemInventoryBase> itemsBag => _itemsBag;

    [SerializeField]
    List<Transform> _itemSlotsBag = new List<Transform>();
    public List<Transform> itemSlotBag => _itemSlotsBag;




    [SerializeField] Transform _gridLayoutShop;

    [SerializeField] List<ItemInventoryBase> _itemsShop = new List<ItemInventoryBase>();
    public List<ItemInventoryBase> itemsShop => _itemsShop;

    [SerializeField]
    List<Transform> _itemSlotsShop = new List<Transform>();
    public List<Transform> itemSlotShop => _itemSlotsShop;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        for (int i = 0; i < _gridLayout.childCount; i++)
        {
            _itemSlots.Add(_gridLayout.GetChild(i).GetComponent<Transform>());
        }

        int j = 0;
        foreach (ItemInventoryBase item in _items)
        {
            EventTrigger g = Instantiate(item.gameObject, _itemSlots[j]).GetComponent<EventTrigger>();
            var pDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };

            pDown.callback.AddListener(eventData => {
                InventoryBagController.Instant.ShowDetailItems(g.gameObject);
                DragController.Instant.setMovingItem(g.gameObject, false);
            });
            g.triggers.Add(pDown);

            var pUp = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };

            pUp.callback.AddListener(eventData => {
                DragController.Instant.removeMovingItem();
            });
            g.triggers.Add(pUp);
            j++;
        }


        for (int i = 0; i < _gridLayoutBag.childCount; i++)
        {
            _itemSlotsBag.Add(_gridLayoutBag.GetChild(i).GetComponent<Transform>());
        }

        j = 0;
        foreach (ItemInventoryBase item in _itemsBag)
        {
            EventTrigger g = Instantiate(item.gameObject, _itemSlotsBag[j]).GetComponent<EventTrigger>();
            var pDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            pDown.callback.AddListener(eventData => {
                InventoryBagController.Instant.ShowDetailItems(g.gameObject);
                DragController.Instant.setMovingItem(g.gameObject, true);
            });
            g.triggers.Add(pDown);

            var pUp = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };
            pUp.callback.AddListener(eventData => {
                DragController.Instant.removeMovingItem();
            });
            g.triggers.Add(pUp);

            j++;
        }


        for (int i = 0; i < _gridLayoutShop.childCount; i++)
        {
            _itemSlotsShop.Add(_gridLayoutShop.GetChild(i).GetComponent<Transform>());
        }

        j = 0;
        foreach (ItemInventoryBase item in _itemsBag)
        {
            EventTrigger g = Instantiate(item.gameObject, _itemSlotsShop[j]).GetComponent<EventTrigger>();
            var pDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            pDown.callback.AddListener(eventData => {
                InventoryShopController.Instant.ChoiceItem(g.gameObject);
            });
            g.triggers.Add(pDown);
            j++;
        }
    }

    public void setItemOnInventory(ItemInventoryBase item, List<Transform> itemSlots, List<ItemInventoryBase> items, bool isInBag)
    {
        if (items.IndexOf(item) != -1)
            return;
        int count = 0;
        foreach (Transform slot in itemSlots)
        {
            if ((slot.childCount > 2))
            {
                count++;
                continue;
            }

            item.transform.SetParent(slot);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
            item.UpdateRectTransform(new Vector2(-32, -32));
            
            EventTrigger g = item.GetComponent<EventTrigger>();
            g.triggers.Clear();
            var pDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };

            pDown.callback.AddListener(eventData => {
                InventoryBagController.Instant.ShowDetailItems(g.gameObject);
                DragController.Instant.setMovingItem(g.gameObject, isInBag);
            });
            g.triggers.Add(pDown);

            var pUp = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };

            pUp.callback.AddListener(eventData => {
                DragController.Instant.removeMovingItem();
            });
            g.triggers.Add(pUp);

            items.Add(item);
            count++;
            break;
        }
    }


    public void setItemOnShop(ItemInventoryBase item, List<Transform> itemSlots, List<ItemInventoryBase> items)
    {
        if (items.IndexOf(item) != -1)
            return;
        int count = 0;
        foreach (Transform slot in itemSlots)
        {
            if ((slot.childCount > 3))
            {
                count++;
                continue;
            }

            item.transform.SetParent(slot);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
            item.UpdateRectTransform(new Vector2(-32, -32));
            EventTrigger g = item.GetComponent<EventTrigger>();


            g.triggers.Clear();
            var pDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };

            pDown.callback.AddListener(eventData =>
            {
                InventoryShopController.Instant.ChoiceItem(g.gameObject);
            });
            g.triggers.Add(pDown);

            items.Add(item);
            count++;
            break;
        }
    }

}
