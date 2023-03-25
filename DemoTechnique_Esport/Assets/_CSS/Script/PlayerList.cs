using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Struct contenant les donnees sur les joueurs
/// name => Nom du joueur 
/// id => emplacement dans la liste
/// </summary>
[System.Serializable]
public class Player
{
    public string name;
    public int id;

    public Player (string name, int id)
    {
        this.name = name;
        this.id = id;
    }
    
}

/// <summary>
/// Scriptable object contenant une liste de donnee 
/// Change les valeurs d'id selon l'edition de la list
/// </summary>
[System.Serializable]
[CreateAssetMenu(menuName = "EsportUI/ListPlayer", fileName = "NewListPlayer")]
public class PlayerList : ScriptableObject
{
    public int heightMaxList = 20;

    public List<Player> players = new List<Player>();

    /// <summary>
    /// Call when value change on inspector (only editor mode)
    /// </summary>
    private void OnValidate()
    {
        
    }

}