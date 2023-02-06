using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyAssets : MonoBehaviour {

    public static LobbyAssets Instance { get; private set; }


    [SerializeField] private Sprite bunnySprite;
    [SerializeField] private Sprite cactoroSprite;
    [SerializeField] private Sprite ninjaSprite;
    [SerializeField] private Sprite orcSprite;


    private void Awake() {
        Instance = this;
    }

    public Sprite GetSprite(CharacterType playerCharacter) {
        switch (playerCharacter) {
            default:
            case CharacterType.Bunny:   return bunnySprite;
            case CharacterType.Cactoro:   return cactoroSprite;
            case CharacterType.Ninja:    return ninjaSprite;
            case CharacterType.Orc:   return orcSprite;
        }
    }

}