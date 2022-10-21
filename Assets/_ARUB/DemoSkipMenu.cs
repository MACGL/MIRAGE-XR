using MirageXR;
using System.IO;
using UnityEngine;

public class DemoSkipMenu : MonoBehaviour
{
    [SerializeField] private string activityId;
    private ActivityManager activityManager => RootObject.Instance.activityManager;

    // Start is called before the first frame update
    void Start()
    {
        LoadActivity();
    }

    private async void LoadActivity()
    {
        await RootObject.Instance.editorSceneService.LoadEditorAsync();
        await activityManager.LoadActivity(activityId, Path.Combine(Application.dataPath, "_ARUB", "Demo"));
    }
}