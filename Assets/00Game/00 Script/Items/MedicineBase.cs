using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MedicineBase : MonoBehaviour
{
    [SerializeField] protected float _timeUnitDelay;
    [SerializeField] protected Transform _uiItem;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    protected float _tmpTimeUnitDelay;
    protected Slider _slider;
    protected bool _isUse = false;
    protected PlayerController _player;
    [SerializeField] protected float _timeUseEffect;
    protected Coroutine routineUseEffect;
    private Vector3 mousePosition;
    void Start()
    {

        this._slider = GetComponent<Slider>();
        this._spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        this._tmpTimeUnitDelay = 0;
    }
    public void Init(PlayerController player)
    {
        this._player = player;
    }

    // Update is called once per frame
    protected void Update()
    {

        this.selectDirection();
        this._tmpTimeUnitDelay -= Time.deltaTime;

    }
    void selectDirection()
    {

        this.mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = (mousePosition - transform.position).normalized;

        if (mouseDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle += 360;
            }

            if ((angle >= 0 && angle <= 90) || (angle >= 270 && angle <= 360))
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                this.transform.localRotation = targetRotation;
                this._uiItem.localPosition = new Vector2(1f, 0f);
                this._spriteRenderer.flipY = false;
                Quaternion targetRotationUi = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                this._uiItem.localRotation = targetRotationUi;
            }

            else if (angle >= 90 && angle <= 270)
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
                this.transform.localRotation = targetRotation;
                this._uiItem.localPosition = new Vector2(1f, 0f);
                this._spriteRenderer.flipY = true;
                Quaternion targetRotationUi = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                this._uiItem.localRotation = targetRotationUi;
            }
        }
    }
    public abstract void use();
    public abstract void onEndUse();
    public abstract void onStartUse();

    private void removeItemUsed()
    {
        IventorySliderController.Instant.RemoveItemsInSlider(1);
    }
    protected IEnumerator useEffect()
    {
        onStartUse();
        removeItemUsed();
        yield return new WaitForSeconds(this._timeUseEffect);
        onEndUse();
    }

    public bool IsUse()
    {
        return this._isUse;
    }
    
    public void setUI(bool value)
    {
        this._uiItem.gameObject.SetActive(value);
    }
}
