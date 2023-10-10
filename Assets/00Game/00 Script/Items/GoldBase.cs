using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBase : MonoBehaviour
{
    [SerializeField] int _gold;
    [SerializeField] float _speed;
    [SerializeField] Rigidbody2D _rigiGold;
    [SerializeField] CapsuleCollider2D _colliGold;
    [SerializeField] PlayerController _player;

    public void PlayerGetGold(PlayerController player)
    {
        this._player = player;
        this._colliGold.isTrigger = true;

    }
    void Start()
    {
        this._colliGold = GetComponent<CapsuleCollider2D>();
        this._rigiGold = GetComponent<Rigidbody2D>();
        this._rigiGold.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_player == null)
            return;
        this._rigiGold.velocity = (this._player.transform.position - this._rigiGold.transform.position).normalized * _speed;
    }
    public int getValueGold()
    {
        return this._gold;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_player == null)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        this._player.setGold(this._player._gold + _gold);
        this.gameObject.SetActive(false);
        this._player = null;
        this._colliGold.isTrigger = false;
    }
}
