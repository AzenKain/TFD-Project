using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncController : MonoBehaviour
{
    [Header("Menu Screen")]
    [SerializeField] Sprite _menuBG;
    [SerializeField] Image _BG;
    [SerializeField] GameObject _mainMenu;

    [Header("Slider")]
    [SerializeField] Sprite _loadingBG;
    [SerializeField] GameObject _loadingScreen;
    [SerializeField] Slider _loadingSlider;
    // Start is called before the first frame update

    public void NewGameOption()
    {
        PlayerPrefs.SetString("detailPlayer", "{\"maxHp\":400,\"tmpHp\":400,\"speed\":2,\"armor\":20,\"dmg\":30,\"gold\":0}");
        PlayerPrefs.SetString("inventoryDatasSlider", "[{\"ID\":1,\"quantity\":1},{\"ID\":4,\"quantity\":3}]");
        PlayerPrefs.SetString("inventoryDatasBag", "[]");
        PlayerPrefs.SetString("boss", "[{\"ID\":3,\"quantity\":1},{\"ID\":5,\"quantity\":1}]");
        PlayerPrefs.SetInt("isNewGame", 1);
        this.LoadingScreen(1);
    }

    public void ContinueOption()
    {
        if (PlayerPrefs.GetInt("isNewGame") == 0)
        {
            this.NewGameOption();
            return;
        }
        this.LoadingScreen(1);
    }
    public void ExitOption()
    {
        Application.Quit();
    }
    void LoadingScreen(int index)
    {
        _BG.sprite = _loadingBG;
        _mainMenu.SetActive(false);
        _loadingScreen.SetActive(true);
        StartCoroutine(LoadingScreenAsync(index));
    }


    IEnumerator LoadingScreenAsync(int index)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(index);
        while (!loadingOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            this._loadingSlider.value = progressValue;
            yield return null;
        }

    }
    void Start()
    {
        this._BG = GetComponentInChildren<Image>();
        this._BG.sprite = _menuBG;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
