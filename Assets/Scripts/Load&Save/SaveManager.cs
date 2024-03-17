using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //dataPatch devuelve la dirección donde está el juego
    private const string FINAL_PATH = "/Saves/";
    private static readonly string _saveFolder = Application.dataPath + FINAL_PATH;
    private const string SAVE_EXTENSION = ".txt";
    private const string SAVE_NAME = "save_";

    public static void Init()
    {

        if (!Directory.Exists(_saveFolder))
        {
            Directory.CreateDirectory(_saveFolder);
        }
    }

    public static void Save(string saveString)
    {
        int saveNumber = 1;
        while (File.Exists(_saveFolder + SAVE_NAME + saveNumber + SAVE_EXTENSION))
            saveNumber++;

        File.WriteAllText(_saveFolder + SAVE_NAME + saveNumber + SAVE_EXTENSION, saveString);
    }

    public static string Load()
    {
        //Regex
        var directoryInfo = new DirectoryInfo(_saveFolder);
        var saveFiles = directoryInfo.GetFiles("*" + SAVE_EXTENSION);

        FileInfo mostRecentFile = null;

        foreach (var fileInfo in saveFiles)
        {
            mostRecentFile ??= fileInfo;
            //mostRecentFile = mostRecentFile == null ? fileInfo : mostRecentFile;
            if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                mostRecentFile = fileInfo;
        }

        return mostRecentFile == null ? null : File.ReadAllText(mostRecentFile.FullName);

    }

    public static string[] SelectLoad()
    {
        var directoryInfo = new DirectoryInfo(_saveFolder);
        var saveFiles = directoryInfo.GetFiles("*" + SAVE_EXTENSION);

        var listOfLoads = new string[saveFiles.Length];

        for (int i = 0; i < saveFiles.Length; i++)
            listOfLoads[i] = File.ReadAllText(saveFiles[i].FullName);

        return listOfLoads ?? null;
    }

}