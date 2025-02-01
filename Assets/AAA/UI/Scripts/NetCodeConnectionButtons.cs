using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NetCodeConnectionButtons : MonoBehaviour
{
    [SerializeField]
    private Button _joinButton;
    
    [SerializeField]
    private Button _hostButton;
    void Awake()
    {
        _joinButton.onClick.AddListener(OnJoinButtonClicked);
        _hostButton.onClick.AddListener(OnHostButtonClicked);
    }

    private void OnDestroy()
    {
        _joinButton.onClick.RemoveListener(OnJoinButtonClicked);
        _hostButton.onClick.RemoveListener(OnHostButtonClicked);
    }

    private void OnJoinButtonClicked()
    {
        NetworkManager.Singleton.StartClient();
    }
    private void OnHostButtonClicked()
    {
        NetworkManager.Singleton.StartHost();
    }
}
