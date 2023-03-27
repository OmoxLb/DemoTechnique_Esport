using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IComparerListByIndex : IComparer<Player>
{
    /// <summary>
    /// Compare les index et les remet dans l'ordre
    /// </summary>
    /// <param name="visual1"></param>
    /// <param name="visual2"></param>
    /// <returns></returns>
    public int Compare(Player visual1, Player visual2)
    {
        if (visual1.id >= visual2.id)
        {
            return 1;
        }
        else return -1;
    }
}
