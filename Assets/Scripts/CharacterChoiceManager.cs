using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterChoiceManager : NetworkBehaviour
{
    private Dictionary<int, string> playerChoices = new Dictionary<int, string>();

    // Initialize the dictionary on the server
    public override void OnNetworkSpawn()
    {
        playerChoices = new Dictionary<int, string>();
    }

    // Register the player's choice when they make it
    //public override void OnStartLocalPlayer()
    //{
    //    //int playerId = GetComponent<NetworkId>().Value;
    //    //string characterChoice = "Knight"; // Example choice
    //    //playerChoices.Add(playerId, characterChoice);
    //    //UpdateChoiceClientRpc(playerId, characterChoice);
    //}

    // Send the dictionary to the client
    [ClientRpc]
    private void UpdateChoiceClientRpc(int playerId, string characterChoice)
    {
        playerChoices[playerId] = characterChoice;
    }

    // remove player from dictionary when he leaves 
    public override void OnNetworkDespawn()
    {
        //int playerId = GetComponent<NetworkId>().Value;
        //playerChoices.Remove(playerId);
    }
}
