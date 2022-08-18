using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;

public static class DataHandler
{
    public static T GetData<T>()
        where T : new()
    {
        Permission.RequestUserPermissions(new string[] { Permission.ExternalStorageWrite, Permission.ExternalStorageRead });
        if (!Directory.Exists(Application.persistentDataPath))
            Directory.CreateDirectory(Application.persistentDataPath);
        if (!File.Exists(GetFileFromType<T>()))
            File.WriteAllText(GetFileFromType<T>(), JsonUtility.ToJson(new T()));
        return SerializeJson<T>(File.ReadAllText(GetFileFromType<T>()));
    }

    private static string GetFileFromType<T>()
    {
        return Path.Combine(Application.persistentDataPath, typeof(T).Name);
    }

    public static void SetData<T>(T data)
    {
        Debug.Log($"[Nick] {typeof(T).Name} saved");

        File.WriteAllText(GetFileFromType<T>(), JsonUtility.ToJson(data));
    }

    public static T SerializeJson<T>(string data)
    {
        return JsonUtility.FromJson<T>(data);
    }
}

public enum NetworkType { Server, Client }
public class NetworkData
{
    public NetworkType Type = NetworkType.Server;
    public string Ip;
    public ushort Port = 19191;

    public NetworkData(){ }

    public NetworkData(NetworkType type, string ip, ushort port)
    {
        Type = type;
        Ip = ip;
        Port = port;
    }

    public NetworkData(string ip)
    {
        Ip = ip;
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}

[Serializable]
public class TourData
{
    public List<ModelData> models = new List<ModelData>();
}

[Serializable]
public class ModelData
{
    public string name;
    public GameObject model;
    public List<float> floorHeights = new List<float>();

    public ModelData() { }

    public ModelData(string name)
    {
        this.name = name;
    }
}