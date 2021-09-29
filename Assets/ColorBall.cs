using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBall : MonoBehaviour
{
    Vector3 defaultPosition = new Vector3();
    Rigidbody rigid = null;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        defaultPosition = this.transform.localPosition;
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        this.transform.localPosition = defaultPosition;
    }
}
