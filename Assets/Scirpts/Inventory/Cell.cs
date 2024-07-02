using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

[System.Serializable]
public class Cell : MonoBehaviour, ISaveable, IResetable
{
    [Header("Cell option")]
    [SerializeField] private string _uniqueID;
    [SerializeField] private Image _iconItem;
    [SerializeField] private TMP_Text _countItemText;
    
    [HideInInspector] public int countItem;
    [HideInInspector] public Item item;

    [Inject] private EventManager _eventManager;

    private void Start() => RefreshCell();

    public string GetSaveKey()
    {
        return $"Cell_{_uniqueID}";;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void FromJson(string jsonData)
    {
        JsonUtility.FromJsonOverwrite(jsonData, this);
    }

    public void Reset()
    {
        _iconItem.sprite = null;
        _iconItem.enabled = false;
        _countItemText.text = "";
        countItem = 0;
        item = null;
        Start();
    }

    public bool SetValue(Item itemDrop)
    {
        if(itemDrop == null)
            return false;

        if(item == null)
        {
            item = itemDrop;
            _iconItem.enabled = true;
            _iconItem.sprite = item.icon;
            countItem = 1;
            _countItemText.text = " ";
            return true;
        }

        if(itemDrop.GetType() == item.GetType())
        {
            if(item.maxStack > countItem)
            {
                countItem++;
                _countItemText.text = $"{countItem}";
                return true;
            }
        }
        return false;
    } 

    public void ResetValue(bool isDelete)
    {
        if(item is Ammo ammo)
            return;

        if(isDelete)
        {
            if(item is Armor armor)
            {
                if(armor.isEquiped == true)
                {
                    _eventManager.onResetEquiped.Invoke(this);
                }
            }
        }

        _iconItem.enabled = false;
        _iconItem.sprite = null;
        countItem = 0;
        _countItemText.text = "";

        item = null;
        _eventManager.onHideInfoScreen?.Invoke();
    }

    public void UpdateValue(int indexUpdate)
    {
        if(countItem + indexUpdate > item.maxStack)
        {
            _eventManager.onAddNewItem.Invoke(item);
            return;
        }

        countItem += indexUpdate;
        _countItemText.text = countItem == 1 ? " " : $"{countItem}";

        if(item is Ammo ammo)
            return;

        if(countItem == 0)
            ResetValue(false);
    }

    public void InfoScreen()
    {
        if(item == null)
            return;

        _eventManager.onShowInfoScreen?.Invoke(this);
    }

    private void RefreshCell()
    {
        if(item != null)
        {
            _iconItem.enabled = true;
            _iconItem.sprite = item.icon;
            _countItemText.text = countItem == 1 ? " " : $"{countItem}";
        }
    }

    public void SiblingIndexBehaviour(int index) => gameObject.transform.SetSiblingIndex(index);
}
