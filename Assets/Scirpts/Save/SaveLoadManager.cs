using UnityEngine;
using System.IO;
using Zenject;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private Transform _parentCellDataSave;
    private ISaveable[] _cellData;
    private IResetable[] _resetCellData;

    [SerializeField] private Transform _parentDataSave;
    private ISaveable[] _saveData;
    private IResetable[] _resetData;
    
    [Inject] private EventManager _eventManager;

    private void Awake()
    {
        _saveData = _parentDataSave.GetComponentsInChildren<ISaveable>();
        _cellData = _parentCellDataSave.GetComponentsInChildren<ISaveable>();

        _resetCellData = _parentCellDataSave.GetComponentsInChildren<IResetable>();
        _resetData = _parentDataSave.GetComponentsInChildren<IResetable>();

        LoadData();
    }   

    private void SaveData()
    {
        foreach(var saveable in _saveData)
            SaveSingleData(saveable);

        foreach(var saveable in _cellData)
            SaveSingleData(saveable);
    }

    private void LoadData()
    {
        foreach (var saveable in _saveData)
            LoadSingleData(saveable);

        foreach (var saveable in _cellData)
            LoadSingleData(saveable);
    }

    private void SaveSingleData(ISaveable saveable)
    {
        string jsonData = saveable.ToJson();
        string saveKey = saveable.GetSaveKey();
        File.WriteAllText(Application.persistentDataPath + "/" + saveKey + ".json", jsonData);
    }

    private void LoadSingleData(ISaveable saveable)
    {
        string saveKey = saveable.GetSaveKey();
        string filePath = Application.persistentDataPath + "/" + saveKey + ".json";

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            saveable.FromJson(jsonData);
        }
    }

    public void ClearSavedData()
    {
        foreach (var resetable in _resetData)
            DeleteSingleSave(resetable);

        foreach (var resetable in _resetCellData)
            DeleteSingleSave(resetable);

        _eventManager.onCellIsEmpty.Invoke();
    }

    private void DeleteSingleSave(IResetable resetable) => resetable.Reset();

    private void OnApplicationQuit() => SaveData();

    private void OnEnable()
    {
        _eventManager.onSaveData += SaveData;
        _eventManager.onLoadData += LoadData;
        _eventManager.onRestartLevel += ClearSavedData;
    }
    private void OnDisable()
    {
        _eventManager.onSaveData -= SaveData;
        _eventManager.onLoadData -= LoadData;
        _eventManager.onRestartLevel -= ClearSavedData;
    }
}