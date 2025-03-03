using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ConnectionMenu : MonoBehaviour
{
   public void OnConnectButtonClick()
    {
        NetworkManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }

    public void OnHostButtonClick()
    {
        NetworkManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }
}
