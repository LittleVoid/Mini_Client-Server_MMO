using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameObject projectilePrefab;

    float speed = 10f;
    CharacterController characterController;
    Material material;
    MeshRenderer renderer;
    NetworkVariable<Color> color = new NetworkVariable<Color>(writePerm: NetworkVariableWritePermission.Owner);

    List<Color> colorList = new List<Color>()
    {
        Color.blue,
        Color.red,
        Color.green,
        Color.yellow,
    };

    public override void OnNetworkSpawn()
    {
        characterController = GetComponent<CharacterController>();
        renderer = GetComponent<MeshRenderer>();
        material = renderer.material;
        int randoIndex = Random.Range(0, colorList.Count);


        color.OnValueChanged += OnColorChanged;

        if (IsOwner)
            color.Value = colorList[randoIndex];
    }

    private void Start()
    {
        material.color = color.Value;
    }

    private void Update()
    {
        if (!IsOwner) return;

        Vector3 horizontalMovement = Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector3 forwardMovement = Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 motion = horizontalMovement + forwardMovement;
        characterController.Move(motion);
        

        if (Input.GetKeyDown(KeyCode.C))
        {
            int randoIndex = Random.Range(0, colorList.Count);
            color.Value = colorList[randoIndex];
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
           SpawnProjectileRpc(transform.position, Quaternion.identity); 
        }

    }

    [Rpc(SendTo.Server)]
    private void SpawnProjectileRpc(Vector3 position, Quaternion rotation)
    {
        var projetileInstance = Instantiate(projectilePrefab, position, rotation);
        projetileInstance.GetComponent<NetworkObject>().Spawn();
    }

    private void OnColorChanged(Color previous, Color newColor)
    {
        material.color = newColor;
    }
}
