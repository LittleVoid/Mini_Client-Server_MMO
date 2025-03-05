using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkUptime : NetworkBehaviour
{
    private NetworkVariable<float> ServerUptime = new NetworkVariable<float>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    [SerializeField] private TextMeshProUGUI text;
    private float lastUpdate = 0.0f;

    private void Start()
    {
        if (text == null)
        {
            text = FindObjectOfType<TextMeshProUGUI>();
            if (text == null)
            {
                Debug.LogError("TextMeshProUGUI nicht gefunden! Bitte zuweisen.");
                return;
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            ServerUptime.Value = 0.0f;
        }
        else
        {
            ServerUptime.OnValueChanged += OnUptimeChanged;
        }
    }

    void Update()
    {
        if (IsServer)
        {
            ServerUptime.Value += Time.deltaTime;

            if (Time.time - lastUpdate >= 0.5f)
            {
                lastUpdate = Time.time;
                Debug.Log("Server-Uptime: " + ServerUptime.Value.ToString("F2") + " Sekunden");
            }
        }

        if (text != null)
        {
            text.text = ServerUptime.Value.ToString("F2") + "s";
        }
    }

    private void OnUptimeChanged(float oldValue, float newValue)
    {
        if (text != null)
        {
            text.text = newValue.ToString("F2") + "s";
        }
    }
}
