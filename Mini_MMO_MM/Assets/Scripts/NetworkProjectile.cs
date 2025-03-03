using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkProjectile : NetworkBehaviour
{
    public float speed = 15f;
    public float lifetime = 5f;

    private void Start()
    {
        if (IsServer)
        {
            StartCoroutine(DestroyAfterTime(lifetime));
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        NetworkObject.Despawn();
        Debug.Log("Hit");
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn();
        }
    }
}
