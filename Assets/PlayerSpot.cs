using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpot : MonoBehaviour
{
    public bool occupied { get; private set; }
    public int enteringOrder = 0;
    public MapTile tile { get; set; }
}
