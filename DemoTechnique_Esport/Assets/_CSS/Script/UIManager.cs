using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

    [Header("Component In UIDocument")]
    [Tooltip("Ui element contains list of player")]
    private ListView _viewPlayerList;

    [Tooltip("The Scroll of the ListView")]
    private ScrollView _scrollViewList;

    [Tooltip("Button for add  player")]
    private Button _addButton;

    [Tooltip("Button for remove player")]
    private Button _removeButton;

    [Tooltip("The InputName for the name of the player")]
    private TextField _inputName;

    [Tooltip("The text of the number of participation")]
    private Label _limitParticipantLabel;

    [Tooltip("Progress bar of the participation")]
    private ProgressBar _progressParticipation;

    [Tooltip("Toggle Button for the list")]
    private SlideToggle _slideToggle;

    [Tooltip("The left text of the Toggle")]
    private Label _leftText;

    [Tooltip("The right  text of the Toggle")]
    private Label _rightText;


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
        SetSlideToggle();
        SetLabel();
        SetProgressBar();
        SetInputName();
        SetButtonList();
        SetListView();
    }


    #region Button Add/Remove

    /// <summary>
    /// Initialze the button for add or remove the list
    /// </summary>
    private void SetButtonList()
    {
        _addButton = _rootEsportUI.Q<Button>("AddList");

        _addButton.clickable.clicked += AddValueToList;

        if (string.IsNullOrWhiteSpace(_inputName.value))
        {
            _addButton.SetEnabled(false);
        }

        _removeButton = _rootEsportUI.Q<Button>("RemoveList");

        _removeButton.clickable.clicked += RemoveValueToList;

        //Disable, user don't selection object at the start
        _removeButton.SetEnabled(false);
    }


    /// <summary>
    /// Add a value to the list and reset the name selection
    /// </summary>
    private void AddValueToList()
    {
        //Safety
        if (_inputName == null) return;
        if (string.IsNullOrWhiteSpace(_inputName.value)) return;


        //Add to our list
        if (_playerList.players.Count < _playerList.heightMaxList)
        {
            _addButton.SetEnabled(true);

            //Check if top or bottom of list
            if (_slideToggle.value)
            {
                Debug.Log("Bot");
                _playerList.players.Add(new Player(_inputName.value, _playerList.players.Count));
            }
            else
            {
                _playerList.players.Add(new Player(_inputName.value, -1));
            }

            RebuildListView();
        }
        else _addButton.SetEnabled(false);


        //Reset the name and the button
        _inputName.value = "";
        _addButton.SetEnabled(false);

    }

    /// <summary>
    /// Remove a value to the list
    /// </summary>
    private void RemoveValueToList()
    {
        // get the selecteed index
        int selectedIndex = _viewPlayerList.selectedIndex;

        // check if one element is enabled
        if (selectedIndex >= 0)
        {
            // Retirer l'élément de la liste
            _playerList.players.RemoveAt(selectedIndex);
            RebuildListView();
        }

        //Reset the selection of the player
        _viewPlayerList.selectedIndex = -1;

        if (_playerList.players.Count == 0)
            _removeButton.SetEnabled(false);
        else _removeButton.SetEnabled(true);

    }

    #endregion

    /// <summary>
    /// Set the input name and the event link
    /// </summary>
    private void SetInputName()
    {
        _inputName = _rootEsportUI.Q<TextField>("InputNom");

        //Disable add button if name input is empty
        _inputName.RegisterValueChangedCallback(evt =>
        {
            //Reset the button if
            if (!string.IsNullOrWhiteSpace(evt.newValue) && _playerList.players.Count < _playerList.heightMaxList)
                _addButton.SetEnabled(true);
            else
                _addButton.SetEnabled(false);

        });

        //Detect if enter is poressed
        _inputName.RegisterCallback<KeyDownEvent>(evt =>
        {
            if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
            {
                AddValueToList();
            }
        });
    }

    /// <summary>
    /// Set the slide toggle for the list view
    /// </summary>
    private void SetSlideToggle()
    {
        _slideToggle = _rootEsportUI.Q<SlideToggle>("SlideToggle");
        _rightText = _rootEsportUI.Q<Label>("RightText");
        _leftText = _rootEsportUI.Q<Label>("LeftText");



        _slideToggle.RegisterValueChangedCallback(evt =>
        {
            _rightText.SetEnabled(evt.newValue);
            _leftText.SetEnabled(!evt.newValue);
        });

        _rightText.SetEnabled(_slideToggle.value);
        _leftText.SetEnabled(!_slideToggle.value);
    }

    /// <summary>
    /// Set the label that count the actual number of participation
    /// </summary>
    private void SetLabel()
    {
        _limitParticipantLabel = _rootEsportUI.Q<Label>("LimitParticipant");

        _limitParticipantLabel.text = _playerList.players.Count + " / " + _playerList.heightMaxList;
    }

    /// <summary>
    /// Set the progress bar of the actual number of participation according to the height max list and the player count
    /// </summary>
    private void SetProgressBar()
    {
        _progressParticipation = _rootEsportUI.Q<ProgressBar>("ParticipantsBar");

        _progressParticipation.value = _playerList.players.Count * 100 / _playerList.heightMaxList;
    }


    #region ListView

    /// <summary>
    /// Initialize the list view
    /// </summary>
    private void SetListView()
    {
        //PART LISTE : 
        //Initialize Ui Element :
        _viewPlayerList = _rootEsportUI.Q<ListView>("PlayerListView");
        _scrollViewList = _rootEsportUI.Q<ScrollView>("");

        //Maj scroll speed (because default is so low)
        _scrollViewList.scrollDecelerationRate = 10;


        _viewPlayerList.fixedItemHeight = _playerList.heightMaxList;

        //When item is add, execute this methods to implements the visual element and maj the data
        _viewPlayerList.makeItem = CreateItemList;
        _viewPlayerList.bindItem = BindListViewItem;
        _viewPlayerList.itemsSource = _playerList.players;

        RebuildListView();

        //Make a reorderable to the list view for drag and drop 
        _viewPlayerList.reorderable = true;

        //Detect when user change the item in the list
        _viewPlayerList.itemIndexChanged += ItemIndexChanged;
        _viewPlayerList.onSelectionChange += OnSelectionChanged;

    }

    /// <summary>
    /// Rebuild the list and maj the id of all the player in the list
    /// </summary>
    private void RebuildListView()
    {
        //_playerList.players.Sort(new IComparerListByIndex());

        //Maj Info text and progress bar
        _limitParticipantLabel.text = _playerList.players.Count + " / " + _playerList.heightMaxList;
        _progressParticipation.value = _playerList.players.Count * 100 / _playerList.heightMaxList;

        _playerList.players.Sort(new IComparerListByIndex());

        for (int i = 0; i < _playerList.players.Count; i++)
        {
            _playerList.players[i].id = i;
        }


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
        return visualElement;
    }

    /// <summary>
    /// Link the data to the item view in the list
    /// </summary>
    /// <param name="listItem"></param>
    /// <param name="index"></param>
    private void BindListViewItem(VisualElement listItem, int index)
    {
        Label textID = listItem.Q<Label>("ID");
        Label textName = listItem.Q<Label>("Name");

        //Maj the data of the item in players list
        textID.text = (_playerList.players[index].id + 1).ToString();
        textName.text = _playerList.players[index].name.ToString();

        //Reset selection
        listItem.Focus();
    }

    /// <summary>
    /// When player change the ordrer of the list
    /// </summary>
    /// <param name="previousIndex"></param>
    /// <param name="newIndex"></param>
    private void ItemIndexChanged(int previousIndex, int newIndex)
    {
        _playerList.players[newIndex].id = newIndex; 
        _playerList.players[previousIndex].id = previousIndex;

        RebuildListView();
    }

    /// <summary>
    /// When the user change the selection on the list view
    /// </summary>
    /// <param name="selectedItems"></param>
    private void OnSelectionChanged(IEnumerable<object> selectedItems)
    {
        // Check if an object is selected, disable the remove button if not
        if (selectedItems != null && selectedItems.Any())
            _removeButton.SetEnabled(true);
        else
            _removeButton.SetEnabled(false);

    }

    #endregion

    #endregion
}
