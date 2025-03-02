using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    public GameObject projectilePrefab;
    public float shootForce = 10f;
    public float projectileLifetime = 3f;

    void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.Space))
        {
            ShootServerRpc();
        }
    }

    [ServerRpc]
    private void ShootServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, Quaternion.identity);
        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();
        networkObject.Spawn(true); // Spawn on network

        // Apply force ONLY on the server
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
        }

    }
}
