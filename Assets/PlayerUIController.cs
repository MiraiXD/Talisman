using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ComNet;
public class PlayerUIController : MonoBehaviour, ISerializationCallbackReceiver
{
    private static Button rollButton;    
    [SerializeField] private Button _rollButton;    

    private void Start()
    {
        rollButton.onClick.AddListener(Roll);
    }
    public static void Enable()
    {
        rollButton.gameObject.SetActive(true);
        rollButton.interactable = true;
    }
    public static void Disable()
    {
        rollButton.gameObject.SetActive(false);
    }
    public static void RollResult(RollInfo rollInfo)
    {
        Debug.Log("Rolled: " + rollInfo.rollResult);

    }
    //public static void ActivateRoll(bool active)
    //{
    //    rollButton.gameObject.SetActive(active);                
    //    EnableRoll(active);
    //}
    //public static void EnableRoll(bool enable)
    //{
    //    rollButton.interactable = enable;
    //}
    private static void Roll()
    {
        ClientTCP.SendObject(ClientPackets.CRoll);
        rollButton.interactable = false;
    }    
    public void OnBeforeSerialize()
    {        
    }

    public void OnAfterDeserialize()
    {
        rollButton = _rollButton;        
    }
}
