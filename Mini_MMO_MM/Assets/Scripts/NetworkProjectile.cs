using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkProjectiile : MonoBehaviour
{
    public float speed = 15f;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);    
    }
}