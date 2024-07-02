using System.Collections.Generic;
using UnityEngine;
using Zenject;

[System.Serializable]
public class Inventory : MonoBehaviour, ISaveable, IResetable
{
    [Header("Inventory cell")]
    [SerializeField] private Transform _cellsParent;
    [SerializeField] private List<Cell> _cells;

    [Header("Items whic can droped")]
    [Space()]
    [SerializeField] private Item[] _drop;
    [SerializeField] private Item[] _ammo;

    [HideInInspector] [SerializeField] private bool _firstStart = true;
    [Inject] private EventManager _eventManager;

    private void Awake()
    {
        if(_cells[0] == null)
            _cells = new List<Cell>(_cellsParent.GetComponentsInChildren<Cell>());

        for(int  i = 0; i < _cells.Count; i++)
            _cells[i].SiblingIndexBehaviour(i);
    }

    private void Start()
    {
        if(_firstStart)
            CreateStartItem();

        _firstStart = false;
    }

    public string GetSaveKey()
    {
        return "Inventory";
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
        _firstStart = true;
    }

    private void CreateStartItem()
    {
        int lastIndex = 0;

        for(int i = 0; i < _drop.Length; i++)
        {
            if(_drop[i].maxStack == 0)
            {
                _cells[i].SetValue(_drop[i]);
            } 
            else
            {
                for(int j = 0; j < _drop[i].maxStack; j++)
                {
                    _cells[i].SetValue(_drop[i]);
                }
            }
            lastIndex = i + 1;
        }

        for(int i = 0; i < _ammo.Length; i++)
        {
            for(int j = 0; j < _ammo[i].maxStack; j++)
                _cells[lastIndex].SetValue(_ammo[i]);

            lastIndex++;
        }
    }

    private void AddRandomItem()
    {
        Item item = _drop[Random.Range(0, _drop.Length)];
        FindEmptyCell(item);
    }

    private void AddNewItem(Item item) => FindEmptyCell(item);

    private void FindEmptyCell(Item item)
    {
        for(int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i].item != null && _cells[i].item.itemName == item.itemName && _cells[i].item.maxStack > _cells[i].countItem)
            {
                _cells[i].SetValue(item);
                return;
            }
        }

        for(int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i].item == null)
            {
                _cells[i].SetValue(item);
                return;
            }
        }
    }

    private bool AddTakeOffItem(Item item)
    {
        for(int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i].item == null)
            {
                _cells[i].SetValue(item);
                if(item is Armor armor)
                    armor.isEquiped = false;
                return true;
            }
        }
        return false;
    }

    private void SortArray(int dragedSibling, int targetSibling)
    {
        Cell draggedCell = _cells[dragedSibling];
        Cell targetCell = _cells[targetSibling];

        _cells[dragedSibling] = targetCell;
        _cells[targetSibling] = draggedCell;

        _cells[dragedSibling].SiblingIndexBehaviour(dragedSibling);
        _cells[targetSibling].SiblingIndexBehaviour(targetSibling);
    }

    private bool UseAmmo(string ammoType)
    {
        foreach(Cell cell in _cells)
        {
            if(cell.item is Ammo ammo && ammo.ammoType == ammoType)
            {
                int useAmmoCount = ammoType == "9х18" ? -1 : -3;
                if(cell.countItem > -useAmmoCount)
                {
                    cell.UpdateValue(useAmmoCount);
                    return true;
                }
            }
        }

        return false;
    }

    private void OnEnable()
    {
        _eventManager.onEnemyDie += AddRandomItem;
        _eventManager.onArrayChanged += SortArray;
        _eventManager.onTakeOffEquipment += AddTakeOffItem;
        _eventManager.onAddNewItem += AddNewItem;
        _eventManager.onUseAmmo += UseAmmo;
        _eventManager.onCellIsEmpty += Start;
    } 
    private void OnDisable()
    {
        _eventManager.onEnemyDie -= AddRandomItem;
        _eventManager.onArrayChanged -= SortArray;
        _eventManager.onTakeOffEquipment -= AddTakeOffItem;
        _eventManager.onAddNewItem -= AddNewItem;
        _eventManager.onUseAmmo -= UseAmmo;
        _eventManager.onCellIsEmpty -= Start;
    } 
}
