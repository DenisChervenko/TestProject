public interface ISaveable
{
    public string GetSaveKey(); 
    public string ToJson();
    public void FromJson(string jsonData); 
}
