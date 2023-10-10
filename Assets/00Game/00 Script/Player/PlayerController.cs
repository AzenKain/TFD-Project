using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public enum PlayerState
{
    Idle,
    Run,
    Roll,
    Slash,
    Skill
}

public enum SkillState
{
    Skill1,
    Skill2,
    Skill3
}

public class PlayerController : MonoBehaviour, IGetHit, IGetKnockBack
{
    [SerializeField] PlayerState _playerState;
    [SerializeField] SkillState _skillState;
    private float xMove;
    private float yMove;
    private Vector2 movement;
    private Rigidbody2D _rigiPlayer;
    AnimationControllerBase _animController;
    private float _tmpTimeSlash, _tmpTimeRoll;
    private bool isRoll, isSlash;
    private float _tmpSFXFoot;
    private Coroutine _coroutineDash;
    [SerializeField] float _timeDelaySlash, _timeDelayRoll, _detectTargetRadius;
    [SerializeField] public float _hp { get; private set; }
    [SerializeField] public float _speed { get; private set; }
    [SerializeField] public float _armor { get; private set; }
    [SerializeField] public float _baseDmg { get; private set; }
    [SerializeField] public float _forceKnockBack { get; private set; }
    [SerializeField] public float _maxHp { get; private set; }
    [SerializeField] public float _dodgeRate { get; private set; }
    [SerializeField] public int _gold { get; private set; }
    [SerializeField] WeaponBase _weapon;
    [SerializeField] MedicineBase _item;
    [SerializeField] bool _isKnockBack;
    Coroutine routineKnockBack;

    void Start()
    {
        this._rigiPlayer = this.GetComponent<Rigidbody2D>();
        this._rigiPlayer.gravityScale = 0;
        this._tmpSFXFoot = 0;
        this._playerState = PlayerState.Idle;
        _animController = this.GetComponentInChildren<AnimationControllerBase>();
        this._tmpTimeSlash = this._timeDelaySlash;
        this._tmpTimeRoll = this._timeDelayRoll;
        this._isKnockBack = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instant._gameState != GameState.Play)
            return;
        this.getGoldOnMap();
        this.fight();
        this.useItem();
        this.choiceItems();
        this.updateState();
        this.updateAni();
        if (isSlash || isRoll)
            return;
        this.Moving();

    }

    public void Init(float maxHp, float tmpHp, float speed, float armor, float dmg, int gold) 
    {
        this._maxHp = maxHp;
        this._hp = tmpHp;
        this._speed = speed;
        this._armor = armor;
        this._baseDmg = dmg;
        this._dodgeRate = 0;
        this._gold = gold;
        UIController.Instant.UpdateHpBar(this._hp, this._maxHp);
    }
    private void FixedUpdate()
    {
        if (isSlash || isRoll)
            return;

        if (_isKnockBack)
            return;

        this._rigiPlayer.velocity = this.movement * _speed;
    }

    IEnumerator removeDashMode()
    {
        this.movement = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        this.isSlash = false;
        this.isRoll = false;
        if (this._coroutineDash != null)
            StopCoroutine(_coroutineDash);
    }
    void fight()
    {
        if (this._weapon != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _weapon.attack();
                this.movement *= 1.2f;
            }
        }
        else
        {
            this._weapon = GetComponentInChildren<WeaponBase>();
        }
    }

    void useItem()
    {
        if (this._item != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _item.use();
                this.movement *= 1.2f;
            }
        }
        else
        {
            this._item = GetComponentInChildren<MedicineBase>();
        }
    }
    void Moving()
    {

        this.xMove = Input.GetAxis("Horizontal");
        this.yMove = Input.GetAxis("Vertical");
        this.movement.x = this.xMove;
        this.movement.y = this.yMove;

        if (this.movement != Vector2.zero && this._tmpSFXFoot < 0)
        {
            SoundManager.Instant.PlaySound("foots");
            this._tmpSFXFoot = 1;
        }

        isRoll = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        isSlash = Input.GetKeyDown(KeyCode.Space);

        if (isRoll && this._tmpTimeRoll < 0)
        {
            SoundManager.Instant.PlaySound("dash_1");
            this._rigiPlayer.AddForce(this.movement.normalized * 75 * _speed);
            this._tmpTimeRoll = this._timeDelayRoll;
            this._coroutineDash = StartCoroutine(removeDashMode());
        }

        else if (isSlash && this._tmpTimeSlash < 0)
        {
            SoundManager.Instant.PlaySound("dash_2");
            this._rigiPlayer.AddForce(this.movement.normalized * 100 * _speed);
            this._tmpTimeSlash = this._timeDelaySlash;
            this._coroutineDash = StartCoroutine(removeDashMode());
        }
        else
        {
            this.isSlash = false;
            this.isRoll = false;
        }
        this._tmpTimeSlash -= Time.deltaTime;
        this._tmpTimeRoll -= Time.deltaTime;
        this._tmpSFXFoot -= Time.deltaTime;
    }

    void choiceItems()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) IventorySliderController.Instant.updateSelectObj(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) IventorySliderController.Instant.updateSelectObj(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) IventorySliderController.Instant.updateSelectObj(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) IventorySliderController.Instant.updateSelectObj(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) IventorySliderController.Instant.updateSelectObj(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) IventorySliderController.Instant.updateSelectObj(5);
    }
    void updateState()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = (mousePosition - transform.position).normalized;
        int animationIndex = 0;
        if (mouseDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle += 360;
            }
            if (angle > 157.5 && angle < 202.5) animationIndex = 8;
            else if (angle > 202.5 && angle < 247.5) animationIndex = 7;
            else if (angle > 247.5 && angle < 292.5) animationIndex = 6;
            else if (angle > 292.5 && angle < 337.5) animationIndex = 5;
            else if (angle > 337.5 && angle < 360) animationIndex = 4;
            else if (angle > 0 && angle < 22.5) animationIndex = 4;
            else if (angle > 22.5 && angle < 67.5) animationIndex = 3;
            else if (angle > 67.5 && angle < 112.5) animationIndex = 2;
            else if (angle > 112.5 && angle < 157.5) animationIndex = 1;
        }

        if (isRoll)
        {
            this._playerState = PlayerState.Roll;
            if (this.xMove > 0 && this.yMove > 0)
            {
                // up-right
                this._animController.UpdateValidateAmin(CONSTANT.stateRoll, 3);
            }
            else if (this.xMove < 0 && this.yMove > 0)
            {
                // up-left
                this._animController.UpdateValidateAmin(CONSTANT.stateRoll, 1);
            }
            else if (this.xMove > 0 && this.yMove < 0)
            {
                // down-right
                this._animController.UpdateValidateAmin(CONSTANT.stateRoll, 5);
            }
            else if (this.xMove < 0 && this.yMove < 0)
            {
                // down-left
                this._animController.UpdateValidateAmin(CONSTANT.stateRoll, 7);
            }
            else if (this.yMove > 0)
            {
                // up
                this._animController.UpdateValidateAmin(CONSTANT.stateRoll, 2);
            }
            else if (this.xMove < 0)
            {
                // left
                this._animController.UpdateValidateAmin(CONSTANT.stateRoll, 8);
            }
            else if (this.xMove > 0)
            {
                // right
                this._animController.UpdateValidateAmin(CONSTANT.stateRoll, 4);
            }
            else if (this.yMove < 0)
            {
                // down
                this._animController.UpdateValidateAmin(CONSTANT.stateRoll, 6);
            }
            return;
        }

        if (isSlash)
        {
            this._playerState = PlayerState.Slash;
            if (this.yMove > 0)
            {
                // up
                this._animController.UpdateValidateAmin(CONSTANT.stateSlash, 4);
            }
            if (this.yMove < 0)
            {
                // up
                this._animController.UpdateValidateAmin(CONSTANT.stateSlash, 1);
            }
            else if (this.xMove < 0)
            {
                // left
                this._animController.UpdateValidateAmin(CONSTANT.stateSlash, 3);
            }
            else if (this.xMove > 0)
            {
                // right
                this._animController.UpdateValidateAmin(CONSTANT.stateSlash, 2);
            }

            return;
        }

        if ((this.xMove != 0 || this.yMove != 0) && !isRoll && !isSlash)
        {
            this._playerState = PlayerState.Run;
            this._animController.UpdateValidateAmin(CONSTANT.stateRun, animationIndex);
        }

        else
        {
            this._playerState = PlayerState.Idle;
            this._animController.UpdateValidateAmin(CONSTANT.stateIdle, animationIndex);
        }

    }

    void updateAni()
    {
        _animController.UpdateAnim(_playerState);
    }

    public void GetHit(float dmg)
    {
        float randomValue = UnityEngine.Random.Range(0, 100);
        if ((int)(_dodgeRate * 100) > randomValue && _dodgeRate != 0)
            return;
        this._animController.SetGetHit();
        SoundManager.Instant.PlaySound("tap");
        dmg = (dmg < this._armor) ? dmg : dmg - this._armor;
        this._hp -= dmg;
        if (this._hp <= 0)
        {
            this._hp = 0;
            GameManager.Instant.UpdateGameState(GameState.Pause);
            UIController.Instant.OverOrWinUI("You Lose");
        }
        UIController.Instant.UpdateHpBar(this._hp, this._maxHp);
    }

    public void updateInUse()
    {
        this._weapon = GetComponentInChildren<WeaponBase>();
        if (this._weapon != null )
        {
            this._weapon.Init(this);
        }
        this._item = GetComponentInChildren<MedicineBase>();
        if (this._item != null)
        {
            this._item.Init(this);
        }
    }
    
    public void GetKnockBack(Vector2 knockBack, float force)
    {
        this.routineKnockBack = StartCoroutine(processKnockBack(knockBack, force));
    }

    public IEnumerator processKnockBack(Vector2 knockBack, float force)
    {
        this._isKnockBack = true;
        this._rigiPlayer.velocity = Vector2.zero;
        this._rigiPlayer.AddForce(knockBack * force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        this._isKnockBack = false;
        StopCoroutine(this.routineKnockBack);
    }

    public void restoreHp(float hp)
    {
        this._hp += hp;
        if (this._hp > this._maxHp) 
            this._hp = this._maxHp;

        UIController.Instant.UpdateHpBar(this._hp, this._maxHp);
    }

    public void setArmor(float armor)
    {
        this._armor = armor;
        Debug.Log(this._armor);
    }

    public void setSpeed(float speed)
    {
        this._speed = speed;
        Debug.Log(this._speed);
    }

    public void setDmg(float dmg)
    {
        this._baseDmg = dmg;
        Debug.Log(this._baseDmg);
    }

    public void setMaxHp(float maxHp)
    {
        this._maxHp = maxHp;
        Debug.Log(this._maxHp);
        UIController.Instant.UpdateHpBar(this._hp, this._maxHp);
    }

    public void setDodge(float dodge)
    {
        this._dodgeRate = dodge;
        Debug.Log(this._dodgeRate);
    }

    public void setGold(int gold)
    {
        this._gold = gold;
        Debug.Log(this._gold);
        DataManager.Instant.UpdateDataPlayer("gold", gold);
    }

    public void getGoldOnMap()
    {
        Collider2D[] collectHit = Physics2D.OverlapCircleAll(transform.position, _detectTargetRadius);
        foreach (Collider2D cl in collectHit)
        {
            if (cl.gameObject.GetInstanceID() == this.gameObject.GetInstanceID()) continue;


            if (cl.gameObject.CompareTag("Gold"))
            {
                GoldBase gold = cl.GetComponent<GoldBase>();
                gold.PlayerGetGold(this);
            }

        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _detectTargetRadius);
    }
}
