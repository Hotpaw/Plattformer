using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CARD : MonoBehaviour
{
    public RogueliteCard cardData;

    Sprite icon;
    string name;
    GameObject ability;
    // Start is called before the first frame update
    void Start()
    {
        icon = cardData.icon;
        ability = cardData.ability;
        name = cardData.name;

    }

    // Update is called once per frame
    private void OnMouseDown()
    {
        // Add ability to ability list of player.
    }
}
