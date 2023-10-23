
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;

using UnityEngine.Tilemaps;


public class Chunk : MonoBehaviour
{

    public Tilemap groundLayer;
    public Tilemap wallLayer;
    public AnimatedTile animatedTile;
    public Tile baseTile;

    public void OnEnable()
    {
        RevealTiles();
    }


    public void RevealTiles()
    {
        if (Application.isPlaying)
        {
            StartCoroutine(ChangeTilesDelay());
            StartCoroutine(ChangeWallTiles());
        }

    }
    IEnumerator ChangeTilesDelay()
    {
        foreach (var position in groundLayer.cellBounds.allPositionsWithin)
        {
            if (groundLayer.HasTile(position))
            {
                Tile tile = (Tile)groundLayer.GetTile(position);
                tile.colliderType = Tile.ColliderType.None;
                tile.color = new Color(255, 255, 255, 0);
                groundLayer.RefreshTile(position);



            }
        }
        foreach (var position in groundLayer.cellBounds.allPositionsWithin)
        {
            if (groundLayer.HasTile(position))
            {
                Tile tile = (Tile)groundLayer.GetTile(position);
                tile.colliderType = Tile.ColliderType.Sprite;
                tile.color = new Color(255, 255, 255, 255);
                groundLayer.RefreshTile(position);
                yield return new WaitForSeconds(0.2f);


            }
        }

    }
    IEnumerator ChangeWallTiles(
        )
    {
        foreach (var position in wallLayer.cellBounds.allPositionsWithin)
        {
            if (wallLayer.HasTile(position))
            {
                Tile tile = (Tile)wallLayer.GetTile(position);

                tile.color = new Color(255, 255, 255, 0);
                wallLayer.RefreshTile(position);



            }
        }
        foreach (var position in wallLayer.cellBounds.allPositionsWithin)
        {
            if (wallLayer.HasTile(position))
            {
                Tile tile = (Tile)wallLayer.GetTile(position);

                tile.color = new Color(255, 255, 255, 255);
                wallLayer.RefreshTile(position);
                yield return new WaitForSeconds(0.2f);


            }
        }
    }
}





