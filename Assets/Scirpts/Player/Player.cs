using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


[System.Serializable]
public class Player : MonoBehaviour, IDamagable, ISaveable, IResetable
{
    [Header("Player text info")]
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _playerHealth;
    [SerializeField] private TMP_Text[] _equipmentStats;

    [Header("Graphic info")]
    [Space()]    
    [SerializeField] private Image _healthAmount;
    [SerializeField] private Image[] _equipmentImage;

    [Header("Components")]
    [Space()]
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private Armor[] _equipment;

    [Space()]
    [SerializeField] private int _healthPlayer;
    private int _targetOfDamage;

    [Inject] private EventManager _eventManager;

    private void Start()
    {
        if(_healthPlayer <= 0)
            _eventManager.onGameOver.Invoke();


        UpdateEquipmentData();
        UpdatePlayerHealth();
        _playerHealth.text = $"{_healthPlayer}";
        _playerName.text = $"{_playerInfo.name}";
    }

    public string GetSaveKey()
    {
        return "PlayerData";
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
        _playerName.text = $"{_playerInfo.name}";
        _playerHealth.text = $"{_playerInfo.health}";

        foreach(var equipment in _equipmentStats)
            equipment.text = "";

        foreach(var equipment in _equipmentImage)
            equipment.sprite = null;

        _equipment[0] = null;
        _equipment[1] = null;

        _healthPlayer = _playerInfo.health;

        UpdateEquipmentData();
        UpdatePlayerHealth();
    }
        
    public void TakeDamage(int damage)
    {
        if(_equipment[_targetOfDamage] != null)
            damage -= _equipment[_targetOfDamage].protection;
        
        _healthPlayer -= damage;
        UpdatePlayerHealth();

        if (_healthPlayer <= 0)
        {
            _eventManager.onGameOver.Invoke();
        }
            
        _targetOfDamage++;
        if(_targetOfDamage > 1)
            _targetOfDamage = 0;
    }

    public void RestoreHP(Cell cell)
    {
        if(cell.item is Consumable consumable)
        {
            _healthPlayer += consumable.restoreHP;
            cell.UpdateValue(-1);
            UpdatePlayerHealth();
        }
    }

    private void UpdatePlayerHealth()
    {
        _healthPlayer = Mathf.Clamp(_healthPlayer, 0, _playerInfo.health);
        _healthAmount.fillAmount = (float)_healthPlayer / _playerInfo.health;
        _playerHealth.text = $"{_healthPlayer}";
    }

    private void SetEquipment(Cell cell)
    {
        if(cell.item is Armor armor)
        {
            int indexEquipment = armor.typeEquipment == "Helment" ? 0 : 1;
            Armor armorTemp = _equipment?[indexEquipment];

            _eventManager.onHideInfoScreen?.Invoke();

            _equipmentStats[indexEquipment].text = $"{"+ " + armor.protection}";
            _equipment[indexEquipment] = armor;
            _equipmentImage[indexEquipment].sprite = armor.icon;

            armor.isEquiped = true;

            cell.ResetValue(false);

            if(armorTemp != null)
                cell.SetValue(armorTemp);
        }
    }

    private void UpdateEquipmentData()
    {
        int index = 0;
        foreach(var equipment in _equipment)
        {
            if(equipment != null)
            {
                _equipmentStats[index].text = $"{"+ " + equipment.protection}";
                _equipment[index] = equipment;
                _equipmentImage[index].sprite = equipment.icon;

                equipment.isEquiped = true;
            }
            index++;
        }
    }

    private void ResetEquipment(Cell cell)
    {
        if(cell.item is Armor armor)
        {
            int indexEquipment = armor.typeEquipment == "Helment" ? 0 : 1;

            _equipmentStats[indexEquipment].text = " ";
            _equipment[indexEquipment] = null;
            _equipmentImage[indexEquipment].sprite = null;

            armor.isEquiped = false;
        }
    } 

    public void ClearEquipSlot(int indexSlot) 
    {
        if(_eventManager.onTakeOffEquipment.Invoke(_equipment[indexSlot]))
        {
            _equipmentStats[indexSlot].text = " ";
            _equipment[indexSlot] = null;
            _equipmentImage[indexSlot].sprite = null;
        }
    } 

    private PlayerInfo GetPlayerInfo()
    {
        return _playerInfo;
    }

    private void OnEnable()
    {
        _eventManager.onEquipItem += SetEquipment;
        _eventManager.onResetEquiped += ResetEquipment;
        _eventManager.onUseItem += RestoreHP;
        _eventManager.onGetPlayerInfo += GetPlayerInfo;
    }

    private void OnDisable()
    {
        _eventManager.onEquipItem -= SetEquipment;
        _eventManager.onResetEquiped -= ResetEquipment;
        _eventManager.onUseItem -= RestoreHP;
        _eventManager.onGetPlayerInfo -= GetPlayerInfo;
    }
}
