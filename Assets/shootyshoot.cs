using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootyshoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void endAnimation()
    {
        Debug.Log("EndAnimation");
    }

    void fireProjectile()
    {
        Debug.Log("FireProjectile");
        // apply recoil to parent transform
        Rigidbody parentRb = transform.parent.GetComponent<Rigidbody>();
        parentRb.AddForce(transform.localRotation.eulerAngles, ForceMode.Impulse);
        floatyShip parentShip = transform.parent.GetComponent<floatyShip>();
        parentShip.Notify("projectile fired", gameObject);
    }
}
