using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private Button playButton, exitButton;
    public GameObject mainMenu, rooms;

    // Start is called before the first frame update
    void Start()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        playButton.onClick.AddListener(EnableRooms);
        
    }
    private void EnableRooms()
    {
        mainMenu.SetActive(false);
        rooms.SetActive(true);
        
    }
}
