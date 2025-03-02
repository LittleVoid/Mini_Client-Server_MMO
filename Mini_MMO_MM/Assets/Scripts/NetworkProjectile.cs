using Unity.Netcode;
using UnityEngine;

public class NetworkProjectile : NetworkBehaviour
{
    public float lifetime = 3f;

    public override void OnNetworkSpawn()
    {
        if (IsServer) StartCoroutine(DestroyAfterDelay());
    }

    private System.Collections.IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(lifetime);

        if (IsServer && IsSpawned)
        {
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}
