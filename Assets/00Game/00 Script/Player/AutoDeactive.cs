using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactive : MonoBehaviour
{
    [SerializeField] float _lifeTime;
    // Start is called before the first frame update
    Coroutine _coroutine;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator routinDeactive()
    {

        yield return new WaitForSeconds(_lifeTime);
        this.gameObject.SetActive(false);

    }
    private void OnEnable()
    {
        this._coroutine = StartCoroutine(routinDeactive());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }
}
