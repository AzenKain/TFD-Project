using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pathfinding;
using Unity.VisualScripting;
using SimpleJSON;

public enum EnemyState
{
    Run,
    Attack,
    Death
}
public abstract class EnemyController : MonoBehaviour, IGetHit, IGetKnockBack, IGetStun
{
    [SerializeField] protected EnemyState _enemyState;
    [SerializeField] protected BossState _bossState;
    [SerializeField] protected EnemyDataSO _enemyData;
    [SerializeField] protected Rigidbody2D _rigi;
    [SerializeField] protected Collider2D _colli;
    [SerializeField] protected Transform _player;
    [SerializeField] protected AnimationControllerBase _animController;
    [SerializeField] protected LayerMask _layerMask, _playerLayerMask;
    [SerializeField] protected GameManager _gameManager;
    [SerializeField] protected Seeker _seeker;
    [SerializeField] protected HealthBar _hpBar;
    [SerializeField] protected float _detectTargetRadius;
    [SerializeField] protected float _rangeAttack;
    [SerializeField] protected float _HP, _speed, _armor, _baseDmg, _forceKnockBack;
    [SerializeField] protected WeaponBase _weapon;
    [SerializeField] protected Vector2 _pastPosition;
    protected RotationFollowKeyBoard weaponRotation;
    protected int CurrentWayPoint = 0;
    protected float _maxHp;
    protected Vector2 _movement;
    protected bool _isKnockBack;
    protected bool _isStun;
    protected bool _isBackToPossition;
    protected Coroutine routineKnockBack;
    protected Coroutine routineStun;
    protected Path _path;


    void Awake()
    {
        this._rigi = this.GetComponent<Rigidbody2D>();
        this._colli = this.GetComponent<Collider2D>();
        this._hpBar = this.GetComponentInChildren<HealthBar>();
        this._animController = this.GetComponentInChildren<AnimationControllerBase>();
        this._weapon = this.GetComponentInChildren<WeaponBase>();
        this.weaponRotation = this.GetComponentInChildren<RotationFollowKeyBoard>();
        this._seeker = this.GetComponent<Seeker>();
        this.Init();
    }
    void Start()
    {

    }
    public void setEnemyState(EnemyState idx)
    {
        this._enemyState = idx;
    }
    public void setBossState(BossState idx)
    {
        this._bossState = idx;
    }
    public EnemyState getEnemyState()
    {
        return this._enemyState;
    }
    protected abstract void CaculatePath();

    protected abstract void OnPathComplate(Path p);

    protected abstract void MoveToTarger();

    // Update isc alled once per frame
    void Update()
    {
        if (GameManager.Instant._gameState != GameState.Play)
            return;

        if (DetectPlayer())
        {
            _player = null;
        }
        this.MoveToTarger();
        this.updateAni();
        this.updateState();
    }

    public virtual void Init()
    {
        Debug.Log(this.name);
        this._HP = _enemyData._HP;
        this._speed = _enemyData._speed;
        this._armor = this._enemyData._armor;
        this._detectTargetRadius = this._enemyData._detectTargetRadius;
        this._baseDmg = this._enemyData._baseDmg;
        this._forceKnockBack = this._enemyData._forceKnockBack;
        this._rangeAttack = this._enemyData._rangeAttack;
        this._maxHp = _HP;
        this._isKnockBack = false;
        this._isBackToPossition = false;
        if (this._weapon != null)
            this._weapon.Init(this);
        if (this._gameManager == null)
            this._gameManager = GameManager.Instant;
        this._player = this._gameManager.player.transform;
        this._enemyState = EnemyState.Run;
        this._pastPosition = this.transform.position;
        this._hpBar.updateHealthBar(this._HP, this._maxHp);
        InvokeRepeating("CaculatePath", 0f, 0.5f);
    }

    protected virtual void FixedUpdate()
    {
        if (_isKnockBack)
            return;

        if (_isStun)
            return;

        this._rigi.velocity = this._movement * _speed;
    }


    protected virtual bool DetectPlayer()
    {
        Collider2D[] collectHit = Physics2D.OverlapCircleAll(transform.position, _detectTargetRadius, _playerLayerMask);
        foreach (Collider2D cl in collectHit)
        {
            if (cl.gameObject.GetInstanceID() == this.gameObject.GetInstanceID()) continue;


            if (cl.gameObject.CompareTag("Player"))
            {
                this._player = cl.gameObject.transform;
                if(Vector2.Distance(this.transform.position, this._player.position) <= _rangeAttack)
                {
                    if (_isStun)
                        continue;
                    this._weapon.attack();
                }
                return false;
            }

        }

        return true;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, _detectTargetRadius);
    }

    public void GetHit(float dmg)
    {
        if (this.gameObject.activeSelf == false)
            return;
        this._animController.SetGetHit();
        dmg = (dmg < this._armor) ? dmg : dmg - this._armor;
        this.setHp(this._HP - dmg);
        SoundManager.Instant.PlaySound("tap");
        if (this._HP <= 0)
        {
            this.OnDead();
        }
    }

    protected virtual void OnDead()
    {
        this.gameObject.SetActive(false);
        this._enemyState = EnemyState.Death;
        GameManager.Instant.SpawmGold(_enemyData._cost, this.transform.position);
        string data = PlayerPrefs.GetString("boss");
        var dataParsed = JSON.Parse(data);
        int count = 0;
        for (int i = 0; i < dataParsed.Count; i++)
        {
            if (dataParsed[i]["ID"].AsInt == this._enemyData._ID)
            {
                dataParsed[i]["quantity"] -= 1;
            }

            if (dataParsed[i]["quantity"] <= 0)
            {
                count++;
            }
        }
        PlayerPrefs.SetString("boss", dataParsed.ToString());

        if (count == dataParsed.Count)
        {
            GameManager.Instant.UpdateGameState(GameState.Pause);
            UIController.Instant.OverOrWinUI("You Win");
        }
    }


    public void GetKnockBack(Vector2 knockBack, float force)
    {
        if (this.gameObject.activeSelf == false)
            return;
        this.routineKnockBack = StartCoroutine(processKnockBack(knockBack, force));
    }

    public IEnumerator processKnockBack(Vector2 knockBack, float force)
    {
        this._isKnockBack = true;
        this._rigi.velocity = Vector2.zero;
        this._rigi.AddForce(knockBack * force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        this._rigi.velocity = Vector2.zero;
        this._isKnockBack = false;
        StopCoroutine(this.routineKnockBack);
    }

    protected abstract void updateAni();

    protected abstract void updateState();

    public void GetStun(float timeStun)
    {
        this.routineStun = StartCoroutine(processStun(timeStun));
    }

    public IEnumerator processStun(float timeStun)
    {
        this._isStun = true;
        this._rigi.velocity = Vector2.zero;
        yield return new WaitForSeconds(timeStun);
        this._isStun = false;
        StopCoroutine(this.routineKnockBack);
    }

    public void AddForceEnmemy(Vector2 force)
    {
        this._rigi.AddForce(force * _speed);
    }

    public Vector2 GetPositionPlayer()
    {
        return (Vector2)_player.position;
    }

    public float getDmg()
    {
        return this._baseDmg;
    }

    public void setHp(float hp)
    {
        if (hp > this._maxHp)
            return;
        this._HP = hp;
        if (this._HP < 0) this._HP = 0;
        this._hpBar.updateHealthBar(this._HP, this._maxHp);
    }

    public EnemyDataSO getDataEnemy()
    {
        return this._enemyData;
    }
}
