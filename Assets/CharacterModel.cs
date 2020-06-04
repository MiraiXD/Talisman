using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    private int mapTileMask = LayerMask.GetMask("MapTile");
    public void PositionOnMapTile()
    {
        Vector3 origin = transform.position + Vector3.up * 5f;
        Vector3 direction = Vector3.down;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 10f, mapTileMask)
        {
            transform.position = hit.point;
        }        
    }
    private void Start()
    {        
        PositionOnMapTile();
    }
}
