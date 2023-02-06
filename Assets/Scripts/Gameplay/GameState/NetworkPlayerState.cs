//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.Collections;
//using Unity.Netcode;
//using UnityEngine;

//public class NetworkPlayerState : NetworkBehaviour
//{
//    public struct PlayerCharacterChoice : INetworkSerializable, IEquatable<PlayerCharacterChoice>
//    {
//        public ulong PlayerID;
//        public FixedString64Bytes CharacterType;
//        public PlayerCharacterChoice( ulong playerID, FixedString64Bytes characterType)
//        {
//            PlayerID = playerID;
//            CharacterType = characterType;
//        }

//        public bool Equals(PlayerCharacterChoice other)
//        {
//            return  PlayerID == other.PlayerID && CharacterType == other.CharacterType;
//        }

//        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
//        {
//            serializer.SerializeValue(ref PlayerID);
//            serializer.SerializeValue(ref CharacterType);

//        }

//    }
//    //public struct PlayerCharacterChoice {
//    //    FixedString64Bytes playerID;
//    //    FixedString64Bytes characterType;
//    //}

//    public static NetworkPlayerState Instance { get; private set; }
//    public CharacterVisual[] charactersVisuals;
//    private NetworkList<PlayerCharacterChoice> ListPlayerCharacterChoice;


//    private void Awake()
//    {
//        Instance = this;
//        ListPlayerCharacterChoice = new NetworkList<PlayerCharacterChoice>();
//    }
//    public void ChangePlayerChoice(int playerIdx, CharacterType characterType)
//    {
//        int choiceIdx = Convert.ToInt32(characterType);
        
//    }
//    public void AddPlayerChoice (string playerIdx, string characterType)
//    { 
//    }

//}
