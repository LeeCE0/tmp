using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace TableDataConverter
{
    public class DataManager
    {
        string csvPath = @"C:\Users\user\Documents\tmp\241022\Assets\TableData";
        //델리게이트
        public delegate void SendDataEventHandler(string data);

        //이벤트 생성
        public event SendDataEventHandler putConsole;
        public void Init()
        {
        }

        public void MakeCSVDataSheet(string excelName, DataSet excelData)
        {
            for (int sheetIndex = 0; sheetIndex < excelData.Tables.Count; sheetIndex++)
            {
                DataTable sheet = excelData.Tables[sheetIndex];

                var retPath = SplitPath(excelName);
                var reName = csvPath + Path.DirectorySeparatorChar + retPath[2] + $"_{sheet.TableName}.csv";
                SaveCsvFileSheet(reName, sheet, "UTF-8", "\r\n");

                putConsole(string.Format("{0} table :{1}...", excelName, sheet.TableName));
            }
        }

        public string[] SplitPath(string path)
        {
            string[] ret = new string[] { "", "", "", "" };

            ret[1] = Path.GetDirectoryName(path);
            ret[2] = Path.GetFileNameWithoutExtension(path);
            ret[3] = Path.GetExtension(path);

            return ret;
        }
        public int SaveCsvFileSheet(string outFile, DataTable dataTablet, string charCode, string lfCode)
        {
            int err = -1;

            string delimiter = ",";
            FileStream fs = null;
            try
            {
                fs = new FileStream(outFile, FileMode.Create, FileAccess.Write);
                Encoding enc = Encoding.GetEncoding(charCode);

                int rowCnt, colCnt;

                rowCnt = dataTablet.Rows.Count;
              
                for (int row = 1; row < rowCnt; row++)
                {
                    string line = "";
                    colCnt = dataTablet.Columns.Count;

                    string cell;
                    for (int col = 1; col < colCnt; col++)
                    {
                        cell = dataTablet.Rows[row][col].ToString().Trim();
                        if (cell.Contains(","))
                            cell = $"\"{cell}\"";
                        cell = cell.Replace("\n", "\\n");
                        line += cell;
                        if (col < colCnt - 1) line += delimiter;
                    }
                    if (line.Replace(delimiter, "") == string.Empty)
                        continue;

                    line += lfCode;
                    //  byte[] bytes = Encoding.Unicode.GetBytes(s);
                    byte[] bytes = enc.GetBytes(line);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception e)
            {
                err = -1;
                throw new Exception(e.ToString());
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return err;
        }

        public void BuildTables()
        {
            string outputPath = @"C:\Users\user\Documents\tmp\241022\Assets\TableData"; // CS 파일 저장 폴더
            string dataClassPath = Path.Combine(outputPath, "DataClass.cs"); // 공용 CS 파일

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine("public static class DataClass");
            sb.AppendLine("{");
            
            foreach (var file in Directory.GetFiles(csvPath, "*.csv"))
            {
                string className = Path.GetFileNameWithoutExtension(file);
                string[] lines = File.ReadAllLines(file);

                string[] headers = lines[0].Split(',');
                var types = lines[1].Split(',');

                sb.AppendLine($"    public class {className}");
                sb.AppendLine("    {");
                for (int i = 0; i < headers.Length; i++)
                {
                    string header = headers[i].Trim(); 
                    string type = types[i].Trim();
                    sb.AppendLine($"        public {type} {header} {{ get; set; }}");
                }
                sb.AppendLine("    }");
                sb.AppendLine($"    public static Dictionary<int, {className}> {className + "Data"} = new Dictionary<int, {className}>();");
                sb.AppendLine($"    static DataClass()");
                sb.AppendLine("    {");

                string[] dataTypes = lines[1].Split(',');

                for (int i = 2; i < lines.Length; i++)
                {
                    string[] row = lines[i].Split(',');

                    sb.AppendLine($"       {className + "Data"}.Add({i - 1}, new {className}");
                    sb.AppendLine("        {");
                    for (int j = 0; j < headers.Length; j++)
                    {
                        string value = row[j].Trim();

                        switch (dataTypes[j].Trim().ToLower()) 
                        {
                            case "int":
                                sb.AppendLine($"            {headers[j]} = {value},");
                                break;
                            case "float":
                                sb.AppendLine($"            {headers[j]} = {value}f,");
                                break;
                            case "string":
                                sb.AppendLine($"            {headers[j]} = \"{value}\",");
                                break;
                            default:
                                sb.AppendLine($"    {headers[j].Trim()} = ({dataTypes[j].Trim()})System.Enum.Parse(typeof({dataTypes[j].Trim()}), \"{value}\"),");
                                break;
                        }
                    }
                    sb.AppendLine("        });");
                }

                sb.AppendLine("    }");
                sb.AppendLine();
            }
            sb.AppendLine("}");

            File.WriteAllText(dataClassPath, sb.ToString());
        }
    }
}
