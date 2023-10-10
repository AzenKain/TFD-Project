using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFX : MonoBehaviour
{
    [SerializeField] GameObject _prefabGhost;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Color color;
    [SerializeField] float _delayTime;
    Coroutine _coroutine;

    public void Init(SpriteRenderer spriteRenderer)
    {
        this._spriteRenderer = spriteRenderer;
        Debug.Log("Done!");
    }
    public void startGhostFX()
    {
        this._coroutine = StartCoroutine(routineGhostFX());
    }
    public void endGhostFX()
    {
        StopCoroutine(this._coroutine);
    }
    IEnumerator routineGhostFX()
    {
        while (true)
        {
            GameObject ghostInstant = ObjectPooling.Instant.getObj(_prefabGhost);
            ghostInstant.transform.position = this.transform.position;
            SpriteRenderer spriteGhostFX = ghostInstant.GetComponent<SpriteRenderer>();
            spriteGhostFX.sprite = this._spriteRenderer.sprite;
            spriteGhostFX.color = this.color;
            ghostInstant.GetComponent<Animator>().SetTrigger("Ghost");
            ghostInstant.SetActive(true);
            Debug.Log("hello");
            yield return new WaitForSeconds(_delayTime);
        }
    }

}
