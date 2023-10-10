using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public bool _isAttack;
    [SerializeField] float _dmg;
    [SerializeField] float _forceKnockBack;
    [SerializeField] string _tagAvoid;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setDng(float dng)
    {
        this._dmg = dng;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_tagAvoid != null || _tagAvoid != "")
            if (collision.gameObject.CompareTag(_tagAvoid))
                return;

        IGetHit isCanGetHit = collision.gameObject.GetComponent<IGetHit>();

        Debug.Log(collision.name);

        if (isCanGetHit == null)
            return;

        if (_isAttack == false)
            return;
        isCanGetHit.GetHit(this._dmg);

        IGetKnockBack isCanKnockBack = collision.gameObject.GetComponent<IGetKnockBack>();

        if (isCanKnockBack == null)
            return;

        Vector2 dict = (collision.transform.position - this.transform.position).normalized;
        isCanKnockBack.GetKnockBack(dict, _forceKnockBack);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_tagAvoid != null || _tagAvoid != "")
            if (collision.gameObject.CompareTag(_tagAvoid))
                return;
        IGetHit isCanGetHit = collision.gameObject.GetComponent<IGetHit>();

        Debug.Log(collision.name);

        if (isCanGetHit == null)
            return;

        if (_isAttack == false)
            return;
        isCanGetHit.GetHit(2f);

        IGetKnockBack isCanKnockBack = collision.gameObject.GetComponent<IGetKnockBack>();

        if (isCanKnockBack == null)
            return;

        Vector2 dict = (collision.transform.position - this.transform.position).normalized;
        isCanKnockBack.GetKnockBack(dict, _forceKnockBack);
    }
}
