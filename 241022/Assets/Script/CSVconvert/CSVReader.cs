using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace DataT
{
    public class CSVReader : MonoBehaviour
    {
        private static CSVReader instance;
        public static CSVReader Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<CSVReader>();
                return instance;
            }
        }

        string fullFolderPath = Path.Combine(Application.dataPath, "TableData"); // CSV 파일들이 저장된 폴더 경로

        public List<UnitData> unitList;
        public List<MapData> mapList;
        //public List<SkillData> skillList;

        void Awake()
        {
            CSVLoader<UnitData> unitLoader = new CSVLoader<UnitData>();
            unitList = unitLoader.LoadCSV(Path.Combine(fullFolderPath, "UnitTable.csv"));

            CSVLoader<MapData> mapLoader = new CSVLoader<MapData>();
            //mapList = mapLoader.LoadCSV(Path.Combine(folderPath, "MapData.csv"));

            //CSVLoader<SkillData> skillLoader = new CSVLoader<SkillData>();
            //skillList = skillLoader.LoadCSV(Path.Combine(folderPath, "SkillData.csv"));
        }
        public class UnitData
        {
            public int UnitID { get; set; }
            public int UnitSpeed { get; set; }
            public string UnitName { get; set; }
            public int ATK { get; set; }
            public int DEF { get; set; }
            public int HP { get; set; }
            public int Cost { get; set; }
            public byte UnitType { get; set; }

            public UnitData() { }
        }

        public class MapData
        {
            public int MapID { get; set; }
            public string MapName { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        //public class SkillData
        //{
        //    public int SkillID { get; set; }
        //    public string SkillName { get; set; }
        //    public int Power { get; set; }
        //    public int Cooldown { get; set; }
        //}

        public class CSVLoader<T> where T : new()
        {
            public List<T> LoadCSV(string filePath)
            {
                List<T> dataList = new List<T>();

                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string[] headers = reader.ReadLine().Split(',');

                        while (!reader.EndOfStream)
                        {
                            string[] values = reader.ReadLine().Split(',');

                            T data = new T();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                PropertyInfo property = typeof(T).GetProperty(headers[i]);
                                if (property != null)
                                {
                                    object value = Convert.ChangeType(values[i], property.PropertyType);
                                    property.SetValue(data, value);
                                }
                            }
                            dataList.Add(data);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error reading CSV file {filePath}: {e.Message}");
                }

                return dataList;
            }
        }
    }
}
