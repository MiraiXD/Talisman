using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerSpot
{
    public Transform transform;
    [HideInInspector] public bool occupied = false;
}
public class MapTile : MonoBehaviour
{
    public PlayerSpot[] playerSpots;
}
