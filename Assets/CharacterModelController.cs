using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComNet;

public class CharacterModelController : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] private CharacterModel[] _allModels;
    private static CharacterModel[] allModels;
    private static Transform modelsParent;
    private void Start()
    {
        modelsParent = transform;
    }

    public static CharacterModel SpawnModel(ComNet.CharacterInfo characterInfo)
    {        
        foreach(CharacterModel model in allModels)
        {
            if(model.character == characterInfo.character)
            {
                PlayerSpot spot = null;
                MapTileInfo spawnTile = characterInfo.startingTile;
                for (int i = 0; i < 100; i++) // while loop crashes unity
                {
                    spot = Map.GetSpot(spawnTile);
                    if(spot == null) // find spot on an adjacent tile in counterclockwise direction
                    {
                        spawnTile = Map.GetNextTile(spawnTile, true).tileInfo;
                    }
                }
                if (spot == null) { Debug.LogError("Could not find a spot!"); return null; }

                CharacterModel newModel = Instantiate(model, modelsParent.transform);
                newModel.Init();
                newModel.transform.position = CharacterModel.GetPositionOnMapTile(spot.transform.position);
                return newModel;
            }
        }
        Debug.LogError("No such model :< ");
        return null;
    }

    public void OnAfterDeserialize()
    {
        allModels = _allModels;
    }

    public void OnBeforeSerialize()
    {
        
    }
}
