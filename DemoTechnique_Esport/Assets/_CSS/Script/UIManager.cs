using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Script gerant les ui documents et les events de chaque ui element
/// </summary>
public class UIManager : MonoBehaviour
{

    #region Variables

    [SerializeField]
    [Header("Component")]
    [Tooltip("Le document uxml UI Esport")]
    private UIDocument _esportUI;
    private VisualElement _rootEsportUI;

    [SerializeField] 
    [Tooltip("UI Document du visuel representant les donne du joueur")]
    private VisualTreeAsset _playerUI;

    [SerializeField]
    [Tooltip("Scriptable object contenant les datas des players")]
    private PlayerList _playerList;


    #endregion


    #region Built In Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (_esportUI) _rootEsportUI = _esportUI.rootVisualElement;
        else Debug.LogError("Null Reference Object : \n UI Document _esportUI");

        if (!_playerList) Debug.LogError("Null Reference Object : \n (ScriptableObject) PlayerList _playerList");

        Initialize();
    }

    #endregion

    #region Customs Methods

    /// <summary>
    /// Initialise les reference des ui objects
    /// </summary>
    private void Initialize()
    {

    }

    #endregion
}
