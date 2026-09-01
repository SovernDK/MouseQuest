using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DB 
{
    public class Depot
    {
        private string _depotFileName = "depot.dpo";

        [Serializable]
        private class Sheet<T>
        {
            public string name;
            public List<T> lines;
        }

        [Serializable]
        private class DepotFile<T>
        {
            public List<Sheet<T>> sheets;
        }

        public Depot(string path)
        {
            _depotFileName = path + ".dpo";
        }

        public List<T> Load<T>(string sheetName)
        {
            List<T> _data = new List<T>();
            string filePath = Path.Combine(Application.streamingAssetsPath, _depotFileName);   
            if (File.Exists(filePath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(filePath);
                    DepotFile<T> data = JsonUtility.FromJson<DepotFile<T>>(jsonContent);
                    if (data != null)
                    {
                        foreach (Sheet<T> sheet in data.sheets)
                        {
                            if(sheet.name.Equals(sheetName))
                            {
                                foreach (T line in sheet.lines) _data.Add(line);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to parse Depot file.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to read Depot file. Exception: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Depot file not found at path: " + filePath);
            }

            return _data;
        }
    }
}