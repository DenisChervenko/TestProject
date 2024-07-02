using UnityEngine;
using UnityEngine.UI;
using Zenject;

[System.Serializable]
public class CombatButton : MonoBehaviour, ISaveable, IResetable
{
    [Header("Button")]
    [SerializeField] private Button _pistolButton;
    [SerializeField] private Button _riffleButton;

    [HideInInspector][SerializeField] private int _selectedWeapon;

    [Inject] private EventManager _eventManager;

    private void Start()
    {
        _pistolButton.interactable = _selectedWeapon == 0 ? false : true;
        _riffleButton.interactable = _selectedWeapon == 1 ? false : true;

        if(_selectedWeapon == 1)
            _eventManager.onWeaponChange.Invoke(_selectedWeapon);
    }

    public string GetSaveKey()
    {
        return "CombatButton";
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
        _selectedWeapon = 0;
        Start();
    }

    public void SetButton()
    {
        _selectedWeapon++;

        if(_selectedWeapon > 1)
            _selectedWeapon = 0;

        ButtonState();
    }

    private void ButtonState()
    {
        _pistolButton.interactable = !_pistolButton.interactable;
        _riffleButton.interactable = !_riffleButton.interactable;

        _eventManager.onWeaponChange.Invoke(_selectedWeapon);
    }
    public void OnButtonAttack() => _eventManager.onCombatStart.Invoke();
}
