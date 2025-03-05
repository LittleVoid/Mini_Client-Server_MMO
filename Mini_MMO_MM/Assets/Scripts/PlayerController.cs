using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameObject projectilePrefab;
    float speed = 10f;
    CharacterController characterController;
    Material material;
    MeshRenderer meshrenderer;
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
        meshrenderer = GetComponent<MeshRenderer>();
        material = meshrenderer.material;
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

        transform.position = new Vector3(transform.position.x, -4, transform.position.z);


        if (Input.GetKeyDown(KeyCode.C))
        {
            int randoIndex = Random.Range(0, colorList.Count);
            color.Value = colorList[randoIndex];
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 spawnPosition = transform.position;
            Vector3 shootDirection = GetMouseDirection();
            Quaternion rotation = Quaternion.LookRotation(shootDirection);
            SpawnProjectileRpc(spawnPosition, rotation);
        }
    }

    private Vector3 GetMouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            return new Vector3(direction.x, 0, direction.z);
        }
        return transform.forward;
    }

    [Rpc(SendTo.Server)]
    private void SpawnProjectileRpc(Vector3 position, Quaternion rotation)
    {
        var projectileInstance = Instantiate(projectilePrefab, position, rotation);
        projectileInstance.GetComponent<NetworkObject>().Spawn();
    }

    private void OnColorChanged(Color previous, Color newColor)
    {
        material.color = newColor;
    }
}
