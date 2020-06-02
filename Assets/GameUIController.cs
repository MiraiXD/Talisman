using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    //private CharacterSelectionController _characterSelectionController;
    private static CharacterSelectionController characterSelectionController;

    private void Start()
    {
        characterSelectionController = GetComponentInChildren<CharacterSelectionController>();
    }

    public static void ShowCharacterCard(ComNet.CharacterInfo characterInfo)
    {
        characterSelectionController.gameObject.SetActive(true);
        characterSelectionController.ShowCard(characterInfo);

    }
}
