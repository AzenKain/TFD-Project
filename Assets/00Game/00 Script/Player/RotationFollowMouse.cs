using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollowMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 shoulderToMouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        shoulderToMouseDir.z = 0;
        float angle = Mathf.Atan2(shoulderToMouseDir.y, shoulderToMouseDir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation;
    }
}
