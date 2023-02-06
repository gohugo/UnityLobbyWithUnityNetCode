using QFSW.QC;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;


public class RelayManager : MonoBehaviour
{
    private const int MAX_PLAYER_PER_LOBBY = 4;
    private async void Start() {

        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>{
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
                
         };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
   
    [Command]
    public static async Task<string> CreateRelay()
    {
        try
        {
            // number of player minus 1 for the host
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(MAX_PLAYER_PER_LOBBY - 1);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
          
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
            return joinCode;
        } catch(RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    [Command]
    public static async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
            LobbyUI.Instance.HideLobbyUIAfterGameStart();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

}
