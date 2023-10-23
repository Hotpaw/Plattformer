using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] TileMap;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
  public void SpawnChunk(Transform currentChunkTransform)
    {
        GameObject Chunk = Instantiate(TileMap[0], currentChunkTransform.position + new Vector3(+18,0,0), Quaternion.identity);
       
    }
}
