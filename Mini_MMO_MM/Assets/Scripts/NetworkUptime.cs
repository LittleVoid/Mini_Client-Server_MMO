using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public class NetworkUptime : NetworkBehaviour
{
    private NetworkVariable<float> ServerUptimeNetworkVariable = new NetworkVariable<float>();
    private float last_t = 0.0f;

    [SerializeField] private TextMeshProUGUI text;

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
            // Use Time.deltaTime to accumulate real-time elapsed
            ServerUptimeNetworkVariable.Value += Time.deltaTime;

            // Log every 0.5 seconds for debugging
            if (Time.time - last_t >= 0.5f)
            {
                last_t = Time.time;
                Debug.Log("Server uptime: " + ServerUptimeNetworkVariable.Value.ToString("F2") + " seconds");
            }
        }

        // Update the UI for clients
        //if (!IsServer)
        //{
            text.text = ServerUptimeNetworkVariable.Value.ToString("F2") + "s";
        //}
    }
}
