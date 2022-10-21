using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace MirageXR
{
    public static class WorkplaceParser
    {
        private const string RESOURCES_PREFIX = "resources://";
        private const string HTTP_PREFIX = "http";
        private const string JSON_EXCEPTION = ".json";
        private const char SLASH_CHAR = '/';

        public static Workplace Parse(string workplaceId, string projectPath = null)
        {
            var json = GetWorkplaceJson(workplaceId, projectPath);
            return  JsonConvert.DeserializeObject<Workplace>(json);
        }

        public static string Serialize(Workplace workplace)
        {
            return JsonConvert.SerializeObject(workplace);
        }

        private static string GetWorkplaceJson(string activityId, string projectPath = null)
        {
            if (activityId.StartsWith(RESOURCES_PREFIX))
            {
                var filePath = activityId.Replace(RESOURCES_PREFIX, string.Empty);
                var textAsset = Resources.Load(filePath) as TextAsset;
                return textAsset ? textAsset.text : null;
            }

            if (activityId.StartsWith(HTTP_PREFIX))
            {
                var fullname = activityId.Split(SLASH_CHAR);
                activityId = fullname[fullname.Length - 1];
            }

            if (!activityId.EndsWith(JSON_EXCEPTION))
            {
                activityId += JSON_EXCEPTION;
            }

            var path = Path.Combine(projectPath == null ? Application.persistentDataPath : projectPath, activityId);
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }
    }
}