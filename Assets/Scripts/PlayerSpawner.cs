using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject Bunny; //add prefab in inspector
    [SerializeField] private GameObject Cactoro; //add prefab in inspector
    [SerializeField] private GameObject Ninja; //add prefab in inspector
    [SerializeField] private GameObject Orc; //add prefab in inspector


    //[ServerRpc(RequireOwnership = false)] //server owns this object but client can request a spawn
    //public void SpawnPlayerServerRpc(ulong clientId, int prefabId)
    //{
    //    GameObject newPlayer;
    //    NetworkObject netObj;
    //    if (prefabId == 0)
    //        newPlayer = (GameObject)Instantiate(playerPrefabA);
    //    else
    //        newPlayer = (GameObject)Instantiate(playerPrefabB);
    //    netObj = newPlayer.GetComponent<NetworkObject>();
    //    newPlayer.SetActive(true);
    //    netObj.SpawnAsPlayerObject(clientId, true);
    //}
    [ServerRpc(RequireOwnership = false)] //server owns this object but client can request a spawn
    public void MP_CreatePlayerServerRpc(ulong clientId, CharacterType characterType)
    {
        GameObject tempGO;


        switch (characterType)
        {
            case CharacterType.Bunny:
                tempGO = (GameObject)Instantiate(Bunny, Vector3.zero, Quaternion.identity);
                break;
            case CharacterType.Cactoro:
                tempGO = (GameObject)Instantiate(Cactoro, Vector3.zero, Quaternion.identity);
                break;
            case CharacterType.Ninja:
                tempGO = (GameObject)Instantiate(Ninja, Vector3.zero, Quaternion.identity);
                break;
            case CharacterType.Orc:
                tempGO = (GameObject)Instantiate(Orc, Vector3.zero, Quaternion.identity);
                break;
            default:
                tempGO = (GameObject)Instantiate(Bunny, Vector3.zero, Quaternion.identity);
                break;
        }

        NetworkObject netObj = tempGO.GetComponent<NetworkObject>();
        tempGO.SetActive(true);
        netObj.SpawnAsPlayerObject(clientId, true);
    }

    //[ServerRpc(RequireOwnership = false)] //server owns this object but client can request a spawn
    //public void MP_CreatePlayerWithSelectedPrefabServerRpc(ulong clientId, int prefabId)
    //{
    //    GameObject tempGO;
    //    if (prefabId == 0)
    //        tempGO = (GameObject)Instantiate(playerPrefabA);
    //    else
    //        tempGO = (GameObject)Instantiate(playerPrefabB);
    //    NetworkObject netObj = tempGO.GetComponent<NetworkObject>();
    //    tempGO.SetActive(true);
    //    netObj.SpawnAsPlayerObject(clientId, true);
    //}
    public override void OnNetworkSpawn()
    {
        LoadPlayerSelectedChar();

        //if (IsServer)
        //    MP_CreatePlayerServerRpc(NetworkManager.Singleton.LocalClientId, 0);
        //else
        //    MP_CreatePlayerServerRpc(NetworkManager.Singleton.LocalClientId, 1);


    }
    public async void LoadPlayerSelectedChar() {
        string playerId = AuthenticationService.Instance.PlayerId;
        Debug.Log("LobbyManager LoadPlayerSelectedChar: playerId = " + playerId);
        try
        {
            //var lobbyId =  LobbyManager.Instance.GetCurrentLobbyID();
            //Debug.Log("PlayerSpawner LoadPlayerSelectedChar: lobbyIds = " + lobbyId);
            try
            {
                Lobby lobby = LobbyManager.Instance.GetJoinedLobby();
                //Lobby lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
                //Dictionary<string, DataObject> data = lobby.Data;
                List<Player> players = LobbyManager.Instance.GetListPlayers();
                Player player  = players.Find(x => x.Id.Contains(playerId));
                if (player!=null)
                {
                    Debug.Log("LobbyManager LoadPlayerSelectedChar: player.ID = " + player.Id);
                    string playerSelectedCharacter = player.Data[LobbyManager.KEY_PLAYER_CHARACTER].Value;
                    Debug.Log("LobbyManager LoadPlayerSelectedChar:  playerSelectedCharacter = " + playerSelectedCharacter);
                    CharacterType characterType;
                    Enum.TryParse(playerSelectedCharacter, out characterType);
                    MP_CreatePlayerServerRpc(NetworkManager.Singleton.LocalClientId, characterType);
                    
                }
                
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }


    }

}
