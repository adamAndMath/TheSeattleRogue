using UnityEngine;
using UnityEditor;

public class EditorAssetMenu
{
    private const string NewAsset = "New {0}.asset";

    [MenuItem("Assets/Create/Room")]
    public static void CreateConversation()
    {
        CreateNewAsset<Room>();
    }

    [MenuItem("Assets/Create/Item")]
    public static void CreateItem()
    {
        CreateNewAsset<Item>();
    }

    private static T CreateNewAsset<T>() where T : ScriptableObject
    {
        return CreateNewAsset<T>(string.Format(NewAsset, typeof(T).Name));
    }

    private static T CreateNewAsset<T>(string defName) where T : ScriptableObject
    {
        var asset = ScriptableObject.CreateInstance<T>();

        AssetDatabase.CreateAsset(asset, GetUniqueAssetPathNameOrFallback(defName));
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        return asset;
    }

    private static string GetUniqueAssetPathNameOrFallback(string filename)
    {
        string path;

        try
        {
            var assetdatabase = typeof(AssetDatabase);
            path = (string)assetdatabase.GetMethod("GetUniquePathNameAtSelectedPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(assetdatabase, new object[] { filename });
        }
        catch
        {
            path = AssetDatabase.GenerateUniqueAssetPath("Assets/" + filename);
        }

        return path;
    }
}
