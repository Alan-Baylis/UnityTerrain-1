using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class OfferListHandler : MonoBehaviour {
    private ItemDatabase _itemDatabase;
    public GameObject OfferContent;
    public Sprite CoinSprite;
    public Sprite GemSprite;
    public Sprite MoneySprite;

    private ModalPanel _modalPanel;
    private GameObject _contentPanel;
    private CharacterManager _characterManager;
    private List<Offer> _offers = new List<Offer>();

    public int SceneIdForTerrainView = 0;
    void Awake()
    {
        _itemDatabase = ItemDatabase.Instance();
        _modalPanel = ModalPanel.Instance();
        _characterManager =CharacterManager.Instance();
        _contentPanel = GameObject.Find("ContentPanel");
    }

    void Start()
    {
        _offers = _itemDatabase.LoadOffers();
        for (int i = 0; i < _offers.Count; i++)
        {
            if (_offers[i].IsSpecial)
                _offers[i] = NormalizeOffer(_offers[i]);
            if (!_offers[i].IsEnable)
                continue;
            GameObject offerObject = Instantiate(OfferContent);
            offerObject.transform.SetParent(_contentPanel.transform);
            offerObject.transform.name = "Offer " + _offers[i].Id;

            var images = offerObject.GetComponentsInChildren<Image>();
            var texts = offerObject.GetComponentsInChildren<Text>();
            var buttons = offerObject.GetComponentsInChildren<Button>();



            images[1].sprite = GetSprite(_offers[i].SellProd);
            texts[0].text = _offers[i].SellAmount.ToString();

            buttons[0].name = i.ToString();
            buttons[0].onClick.AddListener(ShopOffer);
            images[3].sprite = GetSprite(_offers[i].PayProd);
            texts[1].text = _offers[i].PayProd == "Money" ? 
                (_offers[i].PayAmount - 0.01f).ToString(CultureInfo.InvariantCulture) 
                : _offers[i].PayAmount.ToString();
            offerObject.transform.localScale = Vector3.one;
        }
    }

    private Offer NormalizeOffer(Offer offer)
    {
        switch (offer.SellProd)
        {
            case "CarryCnt":
                if (_characterManager.CharacterSetting.CarryCnt >25)
                    offer.IsEnable = false;
                else
                    offer.PayAmount = (_characterManager.CharacterSetting.CarryCnt) * offer.PayAmount;
                break;
        }
        return offer;
    }

    void ShopOffer() {
        
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        Offer offer = _offers[Int32.Parse(buttonName)];

        if (Regex.IsMatch(offer.SellProd, @"\d"))
        {
            //checked for available spot in inv
            if (!_characterManager.HaveAvailableSlot())
            {
                _modalPanel.Choice("No available slot in inventory! ", ModalPanel.ModalPanelType.Ok);
                return;
            }
        }

        if (ProcessThePay(offer.PayProd, offer.PayAmount))
        {
            if (Regex.IsMatch(offer.SellProd, @"\d"))
            {
                ItemContainer item = _itemDatabase.FindItem(Int32.Parse(offer.SellProd));
                item.setStackCnt(offer.SellAmount);
                item.Print();
                _characterManager.AddItemToInventory(item);
            }
            else
                ProcessTheSell(offer.SellProd, offer.SellAmount);
        }
        
    }

    private void ProcessTheSell(string sellProd, int sellAmount)
    {
        switch (sellProd)
        {
            case "Coin":
                _characterManager.AddCharacterSetting(sellProd, sellAmount);
                break;
            case "Gem":
                _characterManager.AddCharacterSetting(sellProd, sellAmount);
                break;
            case "CarryCnt":
                _characterManager.AddCharacterSetting(sellProd, sellAmount);
                break;
        }
    }

    private bool ProcessThePay(string payProd, int payAmount)
    {
        switch (payProd)
        {
            case "Coin":
                if (_characterManager.CharacterSetting.Coin > payAmount)
                {
                    _characterManager.AddCharacterSetting(payProd, -payAmount);
                    return true;
                }
                _modalPanel.Choice("You don't have enough Coin ! ", ModalPanel.ModalPanelType.Ok);
                return false;
            case "Gem":
                if (_characterManager.CharacterSetting.Gem > payAmount)
                {
                    _characterManager.AddCharacterSetting(payProd, -payAmount);
                    return true;
                }
                _modalPanel.Choice("You don't have enough Gem ! ", ModalPanel.ModalPanelType.Ok);
                return false;
            case "Money":
                if (ProcessThePayment(payAmount))
                    return true;
                _modalPanel.Choice("Your Purches diddn't process ", ModalPanel.ModalPanelType.Ok);
                return false;
        }
        _modalPanel.Choice("Something went wrong", ModalPanel.ModalPanelType.Ok);
        return false;
    }

    private bool ProcessThePayment(int payAmount)
    {
        //todo:send to store
        return true;
    }

    private Sprite GetSprite(string spriteName)
    {
        switch (spriteName)
        {
            case "Coin":
                return CoinSprite;
            case "Gem":
                return GemSprite;
            case "Money":
                return MoneySprite;
            default:
                if (Regex.IsMatch(spriteName, @"\d"))
                {
                    ItemContainer item = _itemDatabase.FindItem(Int32.Parse(spriteName));
                    return item.GetSprite();
                }
                return null;
        }
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene(SceneIdForTerrainView);
    }
}
