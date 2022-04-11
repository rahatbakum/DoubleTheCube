using UnityEngine;

public class FreezeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigidbody = other.GetComponentInParent<Rigidbody>();
        if(rigidbody != null)
            rigidbody.isKinematic = true;
    }
}
