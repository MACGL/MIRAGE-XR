    using MirageXR;
using System.IO;
using UnityEngine;

public class DemoSkipMenu : MonoBehaviour
{
    private ActivityManager activityManager => RootObject.Instance.activityManager;

    [SerializeField] private string session;

    // Start is called before the first frame update
    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, session);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            TextAsset activity = Resources.Load(session + "-activity") as TextAsset;
            File.WriteAllText(path + "-activity.json", activity.text);

            TextAsset workplace = Resources.Load(session + "-workplace") as TextAsset;
            File.WriteAllText(path + "-workplace.json", workplace.text);
        }
      
    }
}