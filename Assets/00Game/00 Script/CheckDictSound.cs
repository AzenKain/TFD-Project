using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDictSound : MonoBehaviour
{
    [SerializeField] AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;
        SoundManager.Instant.PlaySoudBG(_clip);
    }
}
