using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class JoinClient : MonoBehaviour
{
    private Button _joinButton;
    void Awake()
    {
        _joinButton = GetComponent<Button>();
        _joinButton.onClick.AddListener(OnJoinButtonClicked);
    }

    private void OnDestroy()
    {
        _joinButton.onClick.RemoveListener(OnJoinButtonClicked);
    }

    private void OnJoinButtonClicked()
    {
        NetworkManager.Singleton.StartClient();
    }
}
