using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    #region Private 변수
    private Rigidbody rb;
    #endregion

    #region Public 변수
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
           rb.isKinematic = true;
        }
    }

    public void SetKinematic(bool active)
    {
        rb.isKinematic = active;
    }
}
