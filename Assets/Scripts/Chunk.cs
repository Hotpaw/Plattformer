
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
    public Tilemap layer3;
    public Tilemap hazardLayer;
   
    public float spriteDelay;
    public float multiplier;
    public ParticleSystem snowSpawnEffect;
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
            StartCoroutine(ActiveTileLayer(layer1));
            StartCoroutine(ActiveTileLayer(layer2));
            StartCoroutine(ActiveTileLayer(layer3));
            StartCoroutine(ActiveTileLayer(hazardLayer));

        }
       

    }
    IEnumerator ActiveTileLayer(Tilemap mapLayer)
    {
        
        if(mapLayer != null)
        {
            foreach (var position in mapLayer.cellBounds.allPositionsWithin)
            {
                if (mapLayer.HasTile(position))
                {
                    Tile tile = (Tile)mapLayer.GetTile(position);
                    tile.colliderType = Tile.ColliderType.None;
                    tile.color = new Color(255, 255, 255, 0);
                    mapLayer.RefreshTile(position);



                }
            }
            foreach (var position in mapLayer.cellBounds.allPositionsWithin)
            {
                if (mapLayer.HasTile(position))
                {
                    Tile tile = (Tile)mapLayer.GetTile(position);
                    

                    tile.color = new Color(255, 255, 255, 255);
                    tile.colliderType = Tile.ColliderType.Sprite;
                    CreateSprite(tile.sprite, position, spriteDelay);
                    yield return new WaitForSeconds(spriteDelay);
                    if (tile.color != new Color(255, 255, 255, 255))
                    {
                        tile.color = new Color(255, 255, 255, 255);
                        tile.colliderType = Tile.ColliderType.Sprite;
                        CreateSprite(tile.sprite, position, spriteDelay);

                    }
                    if (tile.colliderType != Tile.ColliderType.Sprite) { tile.colliderType = Tile.ColliderType.Sprite; }
                    mapLayer.RefreshTile(position);



                }
            }
        }
      
        
       
    }
   
    public void CreateSprite(Sprite sprite, Vector3 localPosition, float time)
    {
        snowSpawnEffect.Stop();
        Vector3 offset = new Vector3((35 * multiplier) + 0.5f, 2, 0);
        var gameObject = new GameObject();
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        
        gameObject.transform.position = localPosition + offset;
        gameObject.transform.DOMoveY(localPosition.y +0.5f, spriteDelay).SetEase(ease);
        //gameObject.transform.DOPunchScale(Vector3.one, 0.5f);
        spriteRenderer.DOFade(0, 0f);
        spriteRenderer.DOFade(1, 0.3f);
        snowSpawnEffect.transform.position = localPosition + offset;
        snowSpawnEffect.Play();
        spriteRenderer.sprite = sprite;
        Destroy(gameObject, time);
    }
    
    
    
}





