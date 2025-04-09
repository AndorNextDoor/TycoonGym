using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    private void Awake()
    {
        Init();
    }

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveString)
    {
        int saveNumber = 1;

        while(File.Exists(SAVE_FOLDER + "save_" + saveNumber + ".txt"))
        {
            saveNumber++;
        }
        File.WriteAllText(SAVE_FOLDER + "/save_" + saveNumber + ".txt", saveString);
    }

    public static string LoadLastSave()
    {
        FileInfo[] saveFiles = GetAllSaveFiles();
        FileInfo mostRecentFile = null;
        
        foreach(FileInfo fileInfo in saveFiles)
        {
            if(mostRecentFile == null)
            {
                mostRecentFile = fileInfo;
            }
            else if(fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
            {
                mostRecentFile = fileInfo;
            } 
        }

        if(mostRecentFile != null)
        {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            return saveString;
        }
        else
        {
            return null;
        }
    }

    public static FileInfo[] GetAllSaveFiles()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        return directoryInfo.GetFiles("*txt");
    }

    public static bool HaveSaveFiles()
    {
        FileInfo[] saveFiles = GetAllSaveFiles();
        return saveFiles.Length >= 0;
    }
}
