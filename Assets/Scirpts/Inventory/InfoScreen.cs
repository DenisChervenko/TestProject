using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InfoScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _infoScreen;

    [Header("Button field")]
    [Space()]
    [SerializeField] private GameObject _equipButton;
    [SerializeField] private GameObject _useButton;
    [SerializeField] private GameObject _buyButton;

    [Header("Graphics info")]
    [Space()]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _defendIcon;
    [SerializeField] private Image _healthIcon;

    [Header("Text info")]
    [Space()]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _itemStat;
    [SerializeField] private TMP_Text _weight;

    private Cell _cell;
    public Cell Cell {get { return _cell; } }

    [Inject] private EventManager _eventManager;

    private void DisplayScreen(Cell cell)
    {
        _cell = cell;
        UIElementUpdate(cell);

        if(cell.item is Armor armor)
        {
            _defendIcon.enabled = true;
            _itemStat.text = $"{armor.protection}";
            _equipButton.SetActive(true);
        }
        else if(cell.item is Consumable consumable)
        {
            _healthIcon.enabled = true;
            _itemStat.text = $"{consumable.restoreHP}";
            _useButton.SetActive(true);
        }
        else if(cell.item is Ammo ammo)
        {
            _buyButton.SetActive(true);
            _itemStat.text = $"+1";
        }
    }

    private void UIElementUpdate(Cell cell)
    {
        _infoScreen.alpha = 1;

        _equipButton.SetActive(false);
        _useButton.SetActive(false);
        _buyButton.SetActive(false);

        _infoScreen.interactable = true;
        _infoScreen.blocksRaycasts = true;

        _defendIcon.enabled = false;
        _healthIcon.enabled = false;

        _name.text = $"{cell.item.itemName}";
        _itemIcon.sprite = cell.item.icon;
        _weight.text = $"{cell.item.weight + " KG"}";
    }

    public void HideScree()
    {
        _infoScreen.alpha = 0;
        _infoScreen.interactable = false;
        _infoScreen.blocksRaycasts = false;
    }

    public void Delete() => _cell.ResetValue(true);
    public void Equip() => _eventManager.onEquipItem?.Invoke(_cell);
    public void Use() => _eventManager.onUseItem?.Invoke(_cell);
    public void Buy() => _cell.UpdateValue(1);
    
    

    private void OnEnable()
    {
        _eventManager.onShowInfoScreen += DisplayScreen;
        _eventManager.onHideInfoScreen += HideScree;
    } 
    private void OnDisable()
    {
        _eventManager.onShowInfoScreen -= DisplayScreen;
        _eventManager.onHideInfoScreen -= HideScree;
    }
}
