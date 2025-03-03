using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    float speed = 10f;
    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (IsOwner && !IsServer)
        {
            Vector3 horizontalMovement = Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            Vector3 forwardMovement = Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
            Vector3 motion = horizontalMovement + forwardMovement;
            characterController.Move(motion);
        }
    }
}
