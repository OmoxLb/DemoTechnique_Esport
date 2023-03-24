using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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

    //Ui element contains list of player
    private ListView _viewPlayerList;
    //Button for add or remove player
    private Button _addButton;
    private Button _removeButton;
    //The InputName for the name of the player
    private TextField _inputName;


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

    private void Update()
    {
        
    }

    #endregion

    #region Customs Methods

    /// <summary>
    /// Initialise les reference des ui objects
    /// </summary>
    private void Initialize()
    {
        SetInputName();
        SetButtonList();
        SetListView();

    }

    /// <summary>
    /// Initialze the button for add or remove the list
    /// </summary>
    private void SetButtonList()
    {
        _addButton = _rootEsportUI.Q<Button>("AddList");

        _addButton.clickable.clicked += AddValueToList;

        if(string.IsNullOrWhiteSpace(_inputName.value))
        {
            _addButton.SetEnabled(false);
        }
    }


    /// <summary>
    /// Add a value to the list and reset the name selection
    /// </summary>
    private void AddValueToList()
    {
        if (_inputName == null) return;

        _inputName.value = "";


    }

    /// <summary>
    /// Set the input name and the event link
    /// </summary>
    private void SetInputName()
    {
        _inputName = _rootEsportUI.Q<TextField>("InputNom");

        //Disable add button if name input is empty
        _inputName.RegisterValueChangedCallback(evt =>
        {
            if (string.IsNullOrWhiteSpace(evt.newValue))
                _addButton.SetEnabled(false);
            else _addButton.SetEnabled(true);
        });
    }


    /// <summary>
    /// Initialize the list view
    /// </summary>
    private void SetListView()
    {
        //PART LISTE : 
        //Initialize Ui Element :
        _viewPlayerList = _rootEsportUI.Q<ListView>("PlayerListView");
        _viewPlayerList.fixedItemHeight = _playerList.heightMaxList;


        //Appear a scroll bar to the list view
        _viewPlayerList.horizontalScrollingEnabled = true;

        _viewPlayerList.makeItem = CreateItemList;
        _viewPlayerList.bindItem = BindListViewItem;
        _viewPlayerList.itemsSource = _playerList.players;
        _viewPlayerList.Rebuild();
    }


    /// <summary>
    /// Create the visual element for the players
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private VisualElement CreateItemList()
    {
        VisualElement visualElement = _playerUI.CloneTree();


        Debug.Log(visualElement);


        return visualElement;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="listItem"></param>
    /// <param name="index"></param>
    private void BindListViewItem(VisualElement listItem, int index)
    {
        Label textID = listItem.Q<Label>("ID");
        Label textName = listItem.Q<Label>("Name");

        textID.text = _playerList.players[index].id.ToString();
        textName.text = _playerList.players[index].name.ToString();

    }

    #endregion
}
