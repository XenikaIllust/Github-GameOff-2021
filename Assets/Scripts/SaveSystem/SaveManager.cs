using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance; // Singleton
    public static SaveData saveData;

    private void Awake()
    {
        // Only one SaveManager should be present at all times
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        saveData = new SaveData();
    }

    private void Start()
    {
        LoadGame("GameOff.sav"); // Placeholder. Saving/Loading should be done from other game objects.
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnSaveGame", SaveGame);
        EventManager.StartListening("OnLoadGame", LoadGame);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnSaveGame", SaveGame);
        EventManager.StopListening("OnLoadGame", LoadGame);
    }

    private void SaveGame(object saveFileName)
    {
        // Find the save file by name and write the SaveData to it
        if (FileManager.WriteToFile((string) saveFileName, saveData.ToJson()))
        {
            Debug.Log("Save Successful.");
        }
        else Debug.Log("Could not save game.");
    }

    private void LoadGame(object saveFileName)
    {
        // Find the save file by name and read the SaveData from it
        if (FileManager.LoadFromFile((string) saveFileName, out string saveFileContents))
        {
            saveData.LoadFromJson(saveFileContents);
            Debug.Log("Load Successful.");
        }
        else Debug.Log("Could not load game.");
    }
}
