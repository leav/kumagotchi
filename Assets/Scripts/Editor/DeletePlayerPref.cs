using UnityEditor;
using UnityEngine;

public class DeletePlayerPref : EditorWindow
{

    [MenuItem("Edit/Reset Playerprefs")]

    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}