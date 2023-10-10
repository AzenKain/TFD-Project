using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollowKeyBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 dict;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // Tính góc giữa trục X và trục Y
        float angle = Mathf.Atan2(dict.y, dict.x) * Mathf.Rad2Deg + 90;

        // Áp dụng góc để xoay người chơi
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
