using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour, ISerializationCallbackReceiver
{
    private static Button rollButton;
    private static Text rollText;
    [SerializeField] private Button _rollButton;
    [SerializeField] private Text _rollText;

    private void Start()
    {
        rollButton.onClick.AddListener(Roll);
    }
    public static void ActivateRoll(bool active)
    {
        rollButton.gameObject.SetActive(active);
        rollText.gameObject.SetActive(active);
        rollText.text = "Roll";
        EnableRoll(active);
    }
    public static void EnableRoll(bool enable)
    {
        rollButton.interactable = enable;
    }
    private static void Roll()
    {
        TalismanClient.Roll();
    }

    public void OnBeforeSerialize()
    {        
    }

    public void OnAfterDeserialize()
    {
        rollButton = _rollButton;
        rollText = _rollText;
    }
}
