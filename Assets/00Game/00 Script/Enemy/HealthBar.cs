using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Camera _camera;
    [SerializeField] Transform _target;
    [SerializeField] Vector3 offset = new Vector3(0, 0, 0);
    public void updateHealthBar(float currHealth, float maxHealth)
    {
        if (_slider == null)
        {
            this._slider = GetComponentInChildren<Slider>();
        }
        _slider.maxValue = maxHealth;
        _slider.value = currHealth;

    }

    private void Start()
    {
        this._slider = GetComponentInChildren<Slider>();
        this._target = this.transform;
        this._camera = Camera.main;
    }
    void Update()
    {
        transform.rotation = _camera.transform.rotation;
        transform.position = _target.position + offset;
    }

}
