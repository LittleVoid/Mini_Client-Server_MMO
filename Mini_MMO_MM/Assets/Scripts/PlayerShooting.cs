using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    public GameObject projectilePrefab;
    public float shootForce = 10f;
    public float projectileLifetime = 3f;
    public float shootCooldown = 0.5f;

    private float lastShotTime = 0f;

    void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.Space) && Time.time > lastShotTime + shootCooldown)
        {
            lastShotTime = Time.time;
            ShootServerRpc();
        }
    }

    [ServerRpc]
    private void ShootServerRpc(ServerRpcParams rpcParams = default)
    {
        Vector3 spawnPosition = transform.position + transform.forward * 1.5f; // Slightly in front
        Quaternion spawnRotation = Quaternion.LookRotation(transform.forward);

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, spawnRotation);
        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(rpcParams.Receive.SenderClientId); // Assign ownership

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
        }
    }
}
