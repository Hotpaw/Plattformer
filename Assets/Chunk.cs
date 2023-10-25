
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;


public class Chunk : MonoBehaviour
{
    public Ease ease;
    public Tilemap layer1;
    public Tilemap layer2;
    public AnimatedTile animatedTile;
    public Tile baseTile;
    public float spriteDelay;
    public float multiplier;
   
    public void OnEnable()
    {
        RevealTiles();
       
    }
    private void Update()
    {
        multiplier = FindAnyObjectByType<TileManager>().multiplier;
    }

    public void RevealTiles()
    {
        if (Application.isPlaying)
        {
            StartCoroutine(ActiveTileLayer1());
            StartCoroutine(ActiveTileLayer2());

        }
       

    }
    IEnumerator ActiveTileLayer1()
    {
        foreach (var position in layer1.cellBounds.allPositionsWithin)
        {
            if (layer1.HasTile(position))
            {
                Tile tile = (Tile)layer1.GetTile(position);
                tile.colliderType = Tile.ColliderType.None;
                tile.color = new Color(255, 255, 255, 0);
                layer1.RefreshTile(position);

                

            }
        }
        foreach (var position in layer1.cellBounds.allPositionsWithin)
        {
            if (layer1.HasTile(position))
            {
                Tile tile = (Tile)layer1.GetTile(position);
                Vector3 tilePosition = position;
              
                tile.color = new Color(255, 255, 255, 255);
                tile.colliderType = Tile.ColliderType.Sprite;
                CreateSprite(tile.sprite, position, spriteDelay);
                yield return new WaitForSeconds(spriteDelay);
              
                layer1.RefreshTile(position);
               


            }
        }
        
       
    }
    IEnumerator ActiveTileLayer2()
    {
        foreach (var position in layer2.cellBounds.allPositionsWithin)
        {
            if (layer2.HasTile(position))
            {
                Tile tile = (Tile)layer2.GetTile(position);
                tile.colliderType = Tile.ColliderType.None;
                tile.color = new Color(255, 255, 255, 0);
                layer2.RefreshTile(position);



            }
        }
        foreach (var position in layer2.cellBounds.allPositionsWithin)
        {
            if (layer2.HasTile(position))
            {
                Tile tile = (Tile)layer2.GetTile(position);
                Vector3 tilePosition = position;
                
                tile.color = new Color(255, 255, 255, 255);
                CreateSprite(tile.sprite, position, spriteDelay);
                yield return new WaitForSeconds(spriteDelay);
                tile.colliderType = Tile.ColliderType.Sprite;
                layer2.RefreshTile(position);



            }
        }


    }
    public void CreateSprite(Sprite sprite, Vector3 localPosition, float time)
    {
        
        Vector3 offset = new Vector3((35 * multiplier) + 0.5f, 2, 0);
        var gameObject = new GameObject();
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        
        gameObject.transform.position = localPosition + offset;
        gameObject.transform.DOMoveY(localPosition.y +0.5f, spriteDelay).SetEase(ease);
        //gameObject.transform.DOPunchScale(Vector3.one, 0.5f);
        spriteRenderer.DOFade(0, 0f);
        spriteRenderer.DOFade(1, 0.3f);

        spriteRenderer.sprite = sprite;
        Destroy(gameObject, time);
    }
    
    
    
}





