using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    public ComNet.CharacterInfo.Characters character;
    private int mapTileMask;
    public Vector3 GetPositionOnMapTile()
    {
        Vector3 origin = transform.position + Vector3.up * 5f;
        Vector3 direction = Vector3.down;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 10f, mapTileMask))
        {
            return hit.point;
        }
        else return Vector3.zero;
    }
    public void MoveTo(PlayerSpot from, PlayerSpot to)
    {

    }
    private void Start()
    {
        //PositionOnMapTile();
        mapTileMask = LayerMask.GetMask("MapTile");
    }
}
