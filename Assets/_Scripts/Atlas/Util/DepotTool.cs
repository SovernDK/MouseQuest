#if (UNITY_EDITOR) 
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace DB 
{
    public class DepotTool : OdinEditorWindow
    {
        [SerializeField]
        private string _depotFilePath = "depot";
        private string _outputDirectory = "Assets/_Scripts/DepotModels";

        [MenuItem("Tools/Depot")]
        private static void OpenWindow()
        {
            GetWindow<DepotTool>().Show();
        }

        [Button("Generate Model Classes")]
        private void GenerateModelClasses()
        {
            Depot depot = new Depot(_depotFilePath);
            string filePath = Path.Combine(Application.streamingAssetsPath, _depotFilePath + ".dpo");

            if (File.Exists(filePath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(filePath);
                    DepotFile data = JsonUtility.FromJson<DepotFile>(jsonContent);
                    if (data != null)
                    {
                        foreach (Sheet sheet in data.sheets)
                        {
                            GenerateClass(sheet);
                        }
                        AssetDatabase.Refresh();
                        Debug.Log("Model classes generated successfully.");
                    }
                    else
                    {
                        Debug.LogError("Failed to parse Depot file.");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed to read Depot file. Exception: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Depot file not found at path: " + filePath);
            }
        }

        private void GenerateClass(Sheet sheet)
        {
            StringBuilder classBuilder = new StringBuilder();
            classBuilder.AppendLine("using System;");
            classBuilder.AppendLine("using UnityEngine;");
            classBuilder.AppendLine();
            classBuilder.AppendLine("namespace DB.Models");
            classBuilder.AppendLine("{");
            classBuilder.AppendLine("   [Serializable]");
            classBuilder.AppendLine($"   public class {sheet.name}");
            classBuilder.AppendLine("   {");

            classBuilder.AppendLine($"      public int guid;");
            classBuilder.AppendLine($"      public int id;");

            foreach (Column column in sheet.columns)
            {
                string type = GetCSharpType(column.typeStr);
                string name = column.name;
                classBuilder.AppendLine($"      public {type} {name};");
            }

            classBuilder.AppendLine("   }");
            classBuilder.AppendLine("}");

            string directoryPath = _outputDirectory;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, $"{sheet.name}.cs");
            File.WriteAllText(filePath, classBuilder.ToString());
        }

        private string GetCSharpType(string type)
        {
            switch(type)
            {
                case "text":
                    return "string";
                case "int":
                    return "int";
                case "float":
                    return "float";
                case "bool":
                    return "bool";  
                case "image":
                    return "Sprite";  
            }
            return "string";
        }

        [Serializable]
        private class Sheet
        {
            public string name;
            public List<Column> columns;
        }

        [Serializable]
        public class Column
        {
            public string typeStr;
            public string name;
        }

        [Serializable]
        private class DepotFile
        {
            public List<Sheet> sheets;
        }
    }
}
#endif