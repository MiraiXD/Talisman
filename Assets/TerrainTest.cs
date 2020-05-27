using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTest : MonoBehaviour
{
    Terrain t;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Terrain>();
        print(t.terrainData.GetHeight(5000000, 5000000));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
