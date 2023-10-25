using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] TileMap;
    public float multiplier = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
       
    }
    // Update is called once per frame
    public void SpawnChunk(Transform currentChunkTransform)
    {
        GameObject Chunk = Instantiate(TileMap[0], currentChunkTransform.position + new Vector3(+35,0,0), Quaternion.identity);
       
    }
    public void UpdateMultiplier()
    {
       
        multiplier++;
    }
}
