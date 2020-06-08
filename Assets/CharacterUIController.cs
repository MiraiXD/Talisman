using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIController : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] private CharacterCard[] _characterCards;
    [SerializeField] private Button _okButton;
    private static CharacterCard[] characterCards;    
    public static Button okButton;
    public static System.Action onOKPressed;

    private static CharacterCard currentActiveCard;
    public static void ShowCard(ComNet.CharacterInfo characterInfo)
    {
        foreach(CharacterCard card in characterCards)
        {
            if(card.character == characterInfo.character)
            {
                card.gameObject.SetActive(true);
                //onCardShownOrHidden?.Invoke(true);
                Debug.Log("jp2");
                card.SetCharacterCard(characterInfo);
                currentActiveCard = card;
                okButton.onClick.AddListener(()=> { HideActiveCard(); onOKPressed?.Invoke(); onOKPressed = null; });
                break;
            }
        }
        Debug.Log("jp");
    }
    public static void HideActiveCard()
    {
        currentActiveCard?.gameObject.SetActive(false);
        currentActiveCard = null;
        //onCardShownOrHidden?.Invoke(false);
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        characterCards = _characterCards;
        okButton = _okButton;
    }
}
