
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;


public class Chunk : MonoBehaviour
{
    public Ease ease;
    public Tilemap groundLayer;
    public Tilemap wallLayer;
    public AnimatedTile animatedTile;
    public Tile baseTile;

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
                Vector3 tilePosition = position;
                tile.colliderType = Tile.ColliderType.Sprite;
                tile.color = new Color(255, 255, 255, 255);
                CreateSprite(tile.sprite, position);
                yield return new WaitForSeconds(0.54f);
              
                groundLayer.RefreshTile(position);
               


            }
        }
        
       
    }
    public void CreateSprite(Sprite sprite, Vector3 localPosition)
    {
        
        Vector3 offset = new Vector3((18 * multiplier) + 0.5f, 2, 0);
        var gameObject = new GameObject();
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        //var rigidbody = gameObject.AddComponent<Rigidbody2D>();
        //rigidbody.gravityScale = 1f;
        gameObject.transform.position = localPosition + offset;
        gameObject.transform.DOMoveY(localPosition.y +0.5f, 0.56f).SetEase(ease);
        //gameObject.transform.DOPunchScale(Vector3.one, 0.5f);
        spriteRenderer.DOFade(0, 0f);
        spriteRenderer.DOFade(1, 0.3f);

        spriteRenderer.sprite = sprite;
        Destroy(gameObject, 0.56f);
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
    public IEnumerator ColorChange()
    {
        Vector4 colorValue = new Vector4(0,0,0,0);
        for (int i = 0; i < 255; i++)
        {
            Debug.Log(colorValue);
            colorValue += new Vector4(+1,+1,+1,+1);
           
            yield return new WaitForSeconds(0.001f);
            
        }
      
        
    }
}





