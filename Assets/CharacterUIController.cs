using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ComNet;

public class CharacterUIController : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] private Transform _cardsHolder;    
    [SerializeField] private GameObject _emptyCard;
    [SerializeField] private Button _rollButton, _acceptButton;
    private static Transform cardsHolder;
    private static CharacterCard[] characterCards;
    private static GameObject emptyCard;
    private static Button rollButton, acceptButton;
    public static bool active { get; private set; } = false;
    public static void Enable()
    {
        active = true;
        characterCards = cardsHolder.GetComponentsInChildren<CharacterCard>(true);
        
        HideAll();
        cardsHolder.gameObject.SetActive(true);
        emptyCard.gameObject.SetActive(true);
        rollButton.gameObject.SetActive(true);
        rollButton.GetComponentInChildren<Text>().text = string.Format("Losuj ({0})", TalismanClient.talismanPlayerInfo.rerollsLeft);
        rollButton.onClick.AddListener(GiveMeRandomCharacter);
        acceptButton.gameObject.SetActive(true);
        acceptButton.interactable = false;        
    }    
    public static void Disable()
    {
        HideAll();
    }
    public static void RandomCharacterArrived(ServerResponds.RandomCharacterResult result)
    {        
        if (!active) return;

        foreach(CharacterCard card in characterCards)
        {
            if(card.character == result.characterInfo.character)
            {
                emptyCard.SetActive(false);
                card.gameObject.SetActive(true);
                card.SetCharacterCard(result.characterInfo);
            }
            else
            {
                card.gameObject.SetActive(false);
            }
        }

        rollButton.interactable = result.rerollsLeft > 0;
        rollButton.GetComponentInChildren<Text>().text = string.Format("Losuj ({0})", result.rerollsLeft);
        acceptButton.interactable = true;
        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(() => { AcceptCharacter(result.characterInfo); });
        Debug.Log("Character: " + result.characterInfo.character);
    }
    public static void GiveMeRandomCharacter()
    {
        if (!active) return;

        rollButton.interactable = false;
        ClientTCP.SendObject(ComNet.ClientPackets.CGiveMeRandomCharacter, new ClientRequests.RandomCharacter());
    }
    public static void AcceptCharacter(ComNet.CharacterInfo characterInfo)
    {
        if (!active) return;

        acceptButton.interactable = false;
        TalismanClient.talismanPlayerInfo.characterInfo = characterInfo;
        ClientTCP.SendObject(ComNet.ClientPackets.CCharacterAcceptedAndReadyToPlay);
    }
    private static void HideAll()
    {
        foreach (CharacterCard card in characterCards) card.gameObject.SetActive(false);
        emptyCard.SetActive(false);
        rollButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        rollButton.onClick.RemoveAllListeners();
        acceptButton.onClick.RemoveAllListeners();
    }
    //public static System.Action onOKPressed;

    //private static CharacterCard currentActiveCard;
    //public static void ShowCard(ComNet.CharacterInfo characterInfo)
    //{
    //    foreach(CharacterCard card in characterCards)
    //    {
    //        if(card.character == characterInfo.character)
    //        {
    //            card.gameObject.SetActive(true);                
    //            card.SetCharacterCard(characterInfo);
    //            currentActiveCard = card;
    //            okButton.onClick.AddListener(()=> { HideActiveCard(); onOKPressed?.Invoke(); onOKPressed = null; });
    //            break;
    //        }
    //    }
    //}
    //public static void HideActiveCard()
    //{
    //    currentActiveCard?.gameObject.SetActive(false);
    //    currentActiveCard = null;
    //    //onCardShownOrHidden?.Invoke(false);
    //}

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        cardsHolder = _cardsHolder;
        rollButton = _rollButton;
        acceptButton = _acceptButton;
        emptyCard = _emptyCard;
    }
}
