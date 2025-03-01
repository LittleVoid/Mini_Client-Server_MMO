using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public class NetworkUptime : NetworkBehaviour
{
    private NetworkVariable<float> ServerUptimeNetworkVariable = new NetworkVariable<float>();
    private float last_t = 0.0f;

    [SerializeField] private TextMeshPro text;

    private void Start()
    {
        Assert.IsNotNull(text);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            ServerUptimeNetworkVariable.Value = 0.0f;
            Debug.Log("Server's uptime initialized to: " + ServerUptimeNetworkVariable.Value);
        }
    }

    void Update()
    {
        if (IsServer)
        {
            ServerUptimeNetworkVariable.Value += Time.deltaTime;

            if (Time.time - last_t >= 0.5f)
            {
                last_t = Time.time;
                Debug.Log("Server uptime: " + ServerUptimeNetworkVariable.Value.ToString("F2") + " seconds");
            }
        }

        if (!IsServer)
        {
            text.text = ServerUptimeNetworkVariable.Value.ToString("F2") + "s";
        }
    }
}
