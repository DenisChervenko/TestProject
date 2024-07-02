using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

[System.Serializable]
public class Enemy : MonoBehaviour, IDamagable, ISaveable, IResetable
{
    [Header("Info diplay option")]
    [SerializeField] private Image _healthAmount;
    [SerializeField] private TMP_Text _enemyHealthText;
    [SerializeField] private TMP_Text _enemyNameText;

    [Header("Enemy info")]
    [Space()]
    [SerializeField] private string _enemyName;
    [SerializeField] private int _enemyDamage;
    [SerializeField] private int _enemyHealth;
    [SerializeField] private int _maxEnemyHealth;

    private IDamagable _playerDamagable;
    [Inject] private EventManager _eventManager;

    private void Start() 
    {
        _playerDamagable = GameObject.Find("Player").GetComponent<IDamagable>();
        TakeDamage(0);

        _enemyNameText.text = $"{_enemyName}";
        _enemyHealthText.text = $"{_enemyHealth}";
    }

    public string GetSaveKey()
    {
        return "EnemyData";
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
        _enemyHealth = _maxEnemyHealth;
        _enemyHealthText.text = $"{_enemyHealth}";
        _enemyNameText.text = $"{_enemyName}";
        TakeDamage(0);
    }

    private void AttackPlayer()
    {
        if(_playerDamagable != null)
            _playerDamagable.TakeDamage(_enemyDamage);
    }

    public void TakeDamage(int damage)
    {
        _enemyHealth -= damage;
        _enemyHealth = Mathf.Clamp(_enemyHealth, 0, _maxEnemyHealth);

        _healthAmount.fillAmount = (float)_enemyHealth / _maxEnemyHealth;
        _enemyHealthText.text = $"{_enemyHealth}";

        if(_enemyHealth <= 0)
        {
            _eventManager.onEnemyDie?.Invoke();
            _enemyHealth = _maxEnemyHealth;
            TakeDamage(0);
        }
    }

    private void OnEnable() => _eventManager.onCombatStart += AttackPlayer;
    private void OnDisable() => _eventManager.onCombatStart -= AttackPlayer;
}
