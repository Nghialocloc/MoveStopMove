using UnityEngine;
using UnityEditor;
using System.IO;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;
using static System.IO.Directory;
using static System.IO.Path;


namespace Gametest
{
    public static class ToolMenu
    {
        [MenuItem("Tool/Setup/Create Starting Folders")]
       public static void CreateDefaultSetup()
       {
            //Tao ra file Project cho du an
            NewSetup("Project","Scripts","Art","Animation","Prefabs","Material");
            Refresh();
        }


        public static void NewSetup(string root, params string[] dir)
        {
            var fullpath = Combine(dataPath, root);
            foreach(var newDirectory in dir)
            {
                CreateDirectory(Combine(fullpath, newDirectory));
            }
        }
    }
}
