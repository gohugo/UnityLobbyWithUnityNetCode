using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LoadLevel : NetworkBehaviour
{


    public override void OnNetworkSpawn()
    {
        if (IsServer)
            NetworkManager.SceneManager.LoadScene("MainGame", LoadSceneMode.Single);

    }
}
