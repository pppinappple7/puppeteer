using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private const int CurrentSaveVersion = 1;
    private const string SaveFileName = "save.json";

    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, SaveFileName);

    private static string BackupPath => SavePath + ".bak";
    private static string TemporaryPath => SavePath + ".tmp";

   
    public static bool HasSave()
    {
        return File.Exists(SavePath) || File.Exists(BackupPath);
    }

    
    public static bool TrySave(SaveData data, out string error)
    {
        error = null;

        if (data == null)
        {
            error = "Нельзя сохранить игру: SaveData равен null.";
            return false;
        }

        if (data.version > CurrentSaveVersion)
        {
            error = "Версия данных сохранения новее версии SaveSystem.";
            return false;
        }

        data.version = CurrentSaveVersion;
        data.EnsureStructure();

        try
        {
            string directory = Application.persistentDataPath;
            Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);
            if (string.IsNullOrWhiteSpace(json))
            {
                error = "Не удалось преобразовать SaveData в JSON.";
                return false;
            }

            if (File.Exists(SavePath) && TryReadFile(SavePath, out _, out _))
            {
                File.Copy(SavePath, BackupPath, true);
            }

            File.WriteAllText(TemporaryPath, json);
            File.Copy(TemporaryPath, SavePath, true);
            File.Delete(TemporaryPath);

            return true;
        }
        catch (Exception exception)
        {
            error = "Не удалось сохранить игру: " + exception.Message;


            try
            {
                if (File.Exists(TemporaryPath))
                    File.Delete(TemporaryPath);
            }
            catch
            {

            }

            return false;
        }
    }


    public static bool TryLoad(out SaveData data, out string error)
    {
        data = null;
        error = null;

        if (File.Exists(SavePath))
        {
            if (TryReadFile(SavePath, out data, out string mainError))
                return true;

            string backupError = "Резервная копия не найдена.";
            if (File.Exists(BackupPath) && TryReadFile(BackupPath, out data, out backupError))
            {
                Debug.LogWarning("Основное сохранение не удалось прочитать. Загружена резервная копия. Причина: " + mainError);
                return true;
            }

            data = null;
            error = "Не удалось загрузить основное сохранение: " + mainError;

            if (File.Exists(BackupPath))
                error += " Резервная копия также повреждена или несовместима: " + backupError;

            return false;
        }

        if (File.Exists(BackupPath))
        {
            if (TryReadFile(BackupPath, out data, out string backupError))
            {
                Debug.LogWarning("Основной файл сохранения отсутствует. Загружена резервная копия.");
                return true;
            }

            error = "Не удалось загрузить резервную копию: " + backupError;
            return false;
        }

        error = "Сохранение не найдено. Можно начать новую игру.";
        return false;
    }

    public static bool DeleteSave(out string error)
    {
        error = null;

        try
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);

            if (File.Exists(BackupPath))
                File.Delete(BackupPath);

            if (File.Exists(TemporaryPath))
                File.Delete(TemporaryPath);

            return true;
        }
        catch (Exception exception)
        {
            error = "Не удалось удалить сохранение: " + exception.Message;
            return false;
        }
    }

    public static string GetSavePath()
    {
        return SavePath;
    }

    private static bool TryReadFile(string path, out SaveData data, out string error)
    {
        data = null;
        error = null;

        try
        {
            if (!File.Exists(path))
            {
                error = "Файл не существует.";
                return false;
            }

            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
            {
                error = "Файл сохранения пуст.";
                return false;
            }

            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);
            if (loadedData == null)
            {
                error = "Не удалось разобрать JSON.";
                return false;
            }

            if (loadedData.version <= 0)
            {
                error = "В файле отсутствует корректная версия сохранения.";
                return false;
            }

            if (loadedData.version > CurrentSaveVersion)
            {
                error = "Файл создан более новой версией игры.";
                return false;
            }

            loadedData.EnsureStructure();
            data = loadedData;
            return true;
        }
        catch (Exception exception)
        {
            error = exception.Message;
            return false;
        }
    }
}
