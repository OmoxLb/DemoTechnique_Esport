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

    public Player(string name, int id)
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

    public string defaultText = "Nouveau participant";

    [Space(5)]
    public List<Player> players = new List<Player>();

    /// <summary>
    /// Call when value change on inspector (only editor mode)
    /// </summary>
    private void OnValidate()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].id = i + 1;
        }
    }

    [ContextMenu("Clear the list")]
    /// <summary>
    /// Clear the list in the inspector
    /// </summary>
    private void ClearList()
    {
        players.Clear();
    }

    [ContextMenu("Fill the list")]
    /// <summary>
    /// Fill the list in the inspecotr
    /// </summary>
    private void FillTheList()
    {
        Debug.Log("Item to add " + (heightMaxList - players.Count));
        int itemToAdd = heightMaxList - players.Count;
        int initialPlayerCount = players.Count;

        for (int i = 1; i <= itemToAdd; i++)
        {
            players.Add(new Player("Player " + (initialPlayerCount + i), (initialPlayerCount + i)));
        }
    }
}