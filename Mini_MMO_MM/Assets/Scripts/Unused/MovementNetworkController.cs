using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class MovementNetworkController : NetworkBehaviour
{
    public int MovementSpeed = 250;
    public NetworkVariable<Vector3> Position = new();

    [Rpc(SendTo.Server)]
    void SubmitPositionRequestServerRpc(Vector3 position, RpcParams rpcParams = default) => Position.Value = position;


    private void Update()
    {
        if(IsOwner && !IsServer)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveX, 0f, moveZ) * MovementSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);

            SubmitPositionRequestServerRpc(transform.position);
        }

        if(IsServer)
            transform.position = Position.Value;
    }
}
