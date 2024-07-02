using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public delegate void OnWeaponChange(int indexWeapon);
    public OnWeaponChange onWeaponChange;

    public delegate void OnArrayChanged(int dragedSibling, int targetSibling);
    public OnArrayChanged onArrayChanged;

    public delegate void OnShowInfoScreen(Cell cell);
    public OnShowInfoScreen onShowInfoScreen;
    public OnShowInfoScreen onEquipItem;
    public OnShowInfoScreen onResetEquiped;
    public OnShowInfoScreen onUseItem;

    public delegate bool OnTakeOffEquipment(Item item);
    public OnTakeOffEquipment onTakeOffEquipment;

    public delegate PlayerInfo OnGetPlayerInfo();
    public OnGetPlayerInfo onGetPlayerInfo;

    public delegate void OnAddNewItem(Item item);
    public OnAddNewItem onAddNewItem;

    public delegate bool OnUseAmmo(string ammoType);
    public OnUseAmmo onUseAmmo;

    public UnityAction onSaveData;
    public UnityAction onLoadData;

    public UnityAction onGameOver;
    public UnityAction onRestartLevel;
    public UnityAction onCellIsEmpty;

    public UnityAction onCombatStart;
    public UnityAction onEnemyDie;
    public UnityAction onHideInfoScreen;
    
}
