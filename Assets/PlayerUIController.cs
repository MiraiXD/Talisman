using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    private static Button rollButton;
    [SerializeField] private Button _rollButton;
    [SerializeField] private Text _rollText;

    private void Start()
    {
        rollButton.onClick.AddListener(Roll);
    }
    public static void Enable(bool enable)
    {
        rollButton.gameObject.SetActive(enable);
    }
    public static void EnableRoll(bool enable)
    {
        rollButton.interactable = enable;
    }
    public static void Roll()
    {
        TalismanClient.Roll();
    }
}
