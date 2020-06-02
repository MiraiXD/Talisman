using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour
{
    [SerializeField] private CharacterCard[] characterCards;
    [SerializeField] private Button okButton;
    public void ShowCard(ComNet.CharacterInfo characterInfo)
    {
        foreach(CharacterCard card in characterCards)
        {
            if(card.character == characterInfo.character)
            {
                card.gameObject.SetActive(true);
                card.SetCharacterCard(characterInfo);
                break;
            }
        }
    }
}
