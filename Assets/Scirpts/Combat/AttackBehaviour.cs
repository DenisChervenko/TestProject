using System;
using UnityEngine;
using Zenject;

[Serializable]
public class AttackBehaviour : MonoBehaviour
{   
    private int _pistolDamage;
    private int _riffleDamage;
    private int _selectedWeapon;
    
    private PlayerInfo _playerInfo;
    private IDamagable _enemyDamagable;
    
    [Inject] private EventManager _eventManager;

    private void Start()
    {
        _enemyDamagable = GameObject.Find("Enemy").GetComponent<IDamagable>();
        _playerInfo = _eventManager.onGetPlayerInfo?.Invoke();

        _pistolDamage = _playerInfo.pistolDamage;
        _riffleDamage = _playerInfo.riffleDamage;
    }

    public void Attack()
    {
        if(_enemyDamagable == null)
            return;
        if(!_eventManager.onUseAmmo.Invoke(_selectedWeapon == 0 ? "9х18" : "5.45х39"))
            return;

        _enemyDamagable.TakeDamage(_selectedWeapon == 0 ? _pistolDamage : _riffleDamage);
    }

    private void WeaponChange(int indexWeapon) => _selectedWeapon = indexWeapon;

    private void OnEnable()
    {
        _eventManager.onCombatStart += Attack;
        _eventManager.onWeaponChange += WeaponChange;
    } 
    private void OnDisable() 
    {
        _eventManager.onCombatStart -= Attack;
        _eventManager.onWeaponChange -= WeaponChange;
    }
}       
