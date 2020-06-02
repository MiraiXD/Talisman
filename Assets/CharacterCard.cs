using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComNet;


public class CharacterCard : MonoBehaviour
{
    public ComNet.CharacterInfo.Characters character;
    [SerializeField] private TMPro.TextMeshProUGUI strengthText, healthText, powerText;
    private ComNet.CharacterInfo characterInfo;
    public void SetCharacterCard(ComNet.CharacterInfo characterInfo)
    {
        this.characterInfo = characterInfo;
        strengthText.text = "Siła: " + characterInfo.maxStrength;
        healthText.text = "Życie: " + characterInfo.maxHealth;
        powerText.text = "Moc: " + characterInfo.maxPower;
    }
}
