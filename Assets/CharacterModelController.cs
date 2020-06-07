using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelController : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] private CharacterModel[] _allModels;
    private static CharacterModel[] allModels;

    public static void SpawnModel(ComNet.CharacterInfo characterInfo)
    {
        foreach(CharacterModel model in allModels)
        {
            if(model.character == characterInfo.character)
            {
                MapTile tile = Map.GetTile(characterInfo.startingTile);
            }
        }
    }

    public void OnAfterDeserialize()
    {
        allModels = _allModels;
    }

    public void OnBeforeSerialize()
    {
        
    }
}
