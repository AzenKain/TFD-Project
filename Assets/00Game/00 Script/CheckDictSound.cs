using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDictSound : MonoBehaviour
{
    [SerializeField] AudioClip _clip;
    [SerializeField] AudioSource _source;
    void Start()
    {
        this._source = this.transform.parent.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;
        if (_source.clip == _clip)
        {
            return;
        }
        if (_source.isPlaying )
        {
            _source.Stop();
        }
        _source.clip = _clip;
        _source.Play();
    }
}
