using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card", order = 1)]
public class RogueliteCard : ScriptableObject
{

        public string prefabName;

    public Sprite icon;
    public GameObject ability;

    
}
