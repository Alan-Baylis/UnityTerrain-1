using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ItemMixture : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    private static ItemMixture _itemMixture;

    private InventoryHandler _inv;
    private Vector2 _offset;
    private Tooltip _tooltip;
    private DateTime _time;
    private Transform _parent;

    public Sprite DefaultSprite;
    public ItemContainer Item;
    public bool ItemLocked;


    void Awake()
    {
        _itemMixture = ItemMixture.Instance();
        _inv = GameObject.Find("Inventory").GetComponent<InventoryHandler>();
        _tooltip = Tooltip.Instance();
    }

    // Update is called once per frame
    void Update()
    {
        if (ItemLocked)
        {
            Text[] texts = this.transform.parent.GetComponentsInChildren<Text>();
            texts[1].text = TimeHandler.PrintTime(_time - DateTime.Now);
            if (DateTime.Now > _time)
            {
                texts[1].text = "<color=Green>Ready</color>";
                ItemLocked = false;
                _inv.PrintMessage("GRE: " + Item.Name + " Is ready");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        _tooltip.Activate(Item);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        _tooltip.Dectivate();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        if (ItemLocked)
            return;
        _offset = eventData.position - (Vector2)this.transform.position;
        this.transform.position = eventData.position - _offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        if (ItemLocked)
            return;
        _parent = transform.parent;
        this.transform.SetParent(this.transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        if (ItemLocked)
            return;
        this.transform.position = eventData.position - _offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        this.transform.position = _parent.position;
        this.transform.SetParent(_parent);
        this.transform.SetSiblingIndex(0);

        if (Item.Id == -1)
        {
            Text[] texts = this.transform.parent.GetComponentsInChildren<Text>();
            texts[0].text = "";
            texts[1].text = "Empty";
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    internal void LoadItem(ItemContainer item, int durationMinutes)
    {
        _time = DateTime.Now.AddMinutes(durationMinutes);
        _inv.SaveCharacterMixture(item, _time);
        LoadItem(item, _time);
    }

    internal void LoadItem(ItemContainer item, DateTime time)
    {
        Item = item;
        GetComponent<Image>().sprite = Item.GetSprite();
        _time = time;
        ItemLocked = true;

        Text[] texts = this.transform.parent.GetComponentsInChildren<Text>();
        texts[0].text = item.StackCnt > 1 ? item.StackCnt.ToString() : "";
        texts[1].text = (_time - DateTime.Now).ToString();
    }

    public void LoadEmpty()
    {
        Item = new ItemContainer();
        GetComponent<Image>().sprite = DefaultSprite;
        _time = DateTime.MinValue;
        ItemLocked = false;
        Text[] texts = this.transform.parent.GetComponentsInChildren<Text>();
        texts[1].text = "Empty";
    }


    public static ItemMixture Instance()
    {
        if (!_itemMixture)
        {
            _itemMixture = FindObjectOfType(typeof(ItemMixture)) as ItemMixture;
            if (!_itemMixture)
                Debug.LogError("There needs to be one active ItemMixture script on a GameObject in your scene.");
        }
        return _itemMixture;
    }
}
