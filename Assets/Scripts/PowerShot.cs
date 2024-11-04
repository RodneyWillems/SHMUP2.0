using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerShot : MonoBehaviour
{
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.up * 20;
    }
}
