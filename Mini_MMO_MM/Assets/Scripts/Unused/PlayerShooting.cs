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
            ShootServerRpc(GetAimDirection());
        }
    }

    private Vector3 GetAimDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0; // Lock movement to XZ plane
            return direction.normalized;
        }
        return transform.forward;
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 direction, ServerRpcParams rpcParams = default)
    {
        Vector3 spawnPosition = transform.position + direction * 1.5f;
        Quaternion spawnRotation = Quaternion.LookRotation(direction);

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, spawnRotation);
        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(rpcParams.Receive.SenderClientId);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * shootForce, ForceMode.Impulse);
        }
    }
}
