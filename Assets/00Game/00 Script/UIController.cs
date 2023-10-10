using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : Singleton<UIController>
{
    [SerializeField] Slider _hpBar;
    [SerializeField] GameObject _backPack;
    [SerializeField] GameObject _shop;
    [SerializeField] GameObject _pause;
    [SerializeField] GameObject _info;
    [SerializeField] TMP_Text _OverOrWin;
    public bool isUI { get; private set; }
    void Start()
    {
        this.isUI = false;
        this._backPack.SetActive(false);
        this._shop.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instant._gameState == GameState.Over)
            return;
    }
    public void OpenShop(string nameDataShop, string titleShop, Sprite img)
    {
        if (this.isUI == true)
            return;
        this.isUI = true;
        this._shop.SetActive(true);
        DataManager.Instant.setupDataShop(nameDataShop);
        InventoryShopController.Instant.DisableSelectObject();
        InventoryShopController.Instant.Init(titleShop, img);
        GameManager.Instant.UpdateGameState(GameState.Pause);

    }
    public void CloseShop()
    {
        this._shop.SetActive(false);
        this.isUI = false;
        InventoryShopController.Instant.DisableSelectObject();
        GameManager.Instant.UpdateGameState(GameState.Play);
        IventorySliderController.Instant.updateSelectObj(IventorySliderController.Instant._pastSelectObj);
        IventorySliderController.Instant.addItem(IventorySliderController.Instant._pastSelectObj);
    }

    public void OpenBackPack()
    {
        if (this.isUI == true)
            return;
        this.isUI = true;
        this._backPack.SetActive(true);
        IventorySliderController.Instant.DisableSelectObject();
        GameManager.Instant.UpdateGameState(GameState.Pause);

    }
    public void CloseBackPack()
    {
        this._backPack.SetActive(false);
        this.isUI = false;
        InventoryBagController.Instant.DisableSelectObject();
        GameManager.Instant.UpdateGameState(GameState.Play);
        IventorySliderController.Instant.updateSelectObj(IventorySliderController.Instant._pastSelectObj);
        IventorySliderController.Instant.addItem(IventorySliderController.Instant._pastSelectObj);
    }
    public void UpdateHpBar(float currentHp, float maxHp)
    {
        _hpBar.maxValue = maxHp;
        _hpBar.value = currentHp;
    }
    public void OverOrWinUI(string idx)
    {
        this._OverOrWin.text = idx;
        this._OverOrWin.transform.parent.gameObject.SetActive(true);
        GameManager.Instant.UpdateGameState(GameState.Pause);
        PlayerPrefs.SetInt("isNewGame", 0);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void OpenInfo()
    {
        if (this.isUI == true)
            return;
        this.isUI = true;
        GameManager.Instant.UpdateGameState(GameState.Pause);
        this._info.gameObject.SetActive(true);
    }

    public void CloseInfo()
    {
        this._info.gameObject.SetActive(false);
        this.isUI = false;
        GameManager.Instant.UpdateGameState(GameState.Play);
    }

    public void OpenPause()
    {
        if (this.isUI == true)
            return;
        this.isUI = true;
        GameManager.Instant.UpdateGameState(GameState.Pause);
        this._pause.gameObject.SetActive(true);
    }

    public void ClosePasue()
    {
        this._pause.gameObject.SetActive(false);
        this.isUI = false;
        GameManager.Instant.UpdateGameState(GameState.Play);
    }
}
