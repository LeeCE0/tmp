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

            // 공용 클래스 파일 시작
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine("public static class DataClass");
            sb.AppendLine("{");

            // CSV 파일 처리
            foreach (var file in Directory.GetFiles(csvPath, "*.csv"))
            {
                string className = Path.GetFileNameWithoutExtension(file); // 테이블 이름
                string[] lines = File.ReadAllLines(file);

                // 헤더 읽기
                string[] headers = lines[0].Split(',');

                // 테이블 데이터 클래스 정의
                sb.AppendLine($"    public class {className}Row");
                sb.AppendLine("    {");
                foreach (var header in headers)
                {
                    sb.AppendLine($"        public string {header} {{ get; set; }}");
                }
                sb.AppendLine("    }");

                // 딕셔너리 생성
                sb.AppendLine($"    public static Dictionary<int, {className}Row> {className} = new Dictionary<int, {className}Row>();");

                // 데이터 추가 코드 생성
                sb.AppendLine($"    static DataClass()");
                sb.AppendLine("    {");

                // 데이터 파싱 및 딕셔너리에 추가
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] row = lines[i].Split(',');
                    sb.AppendLine($"        {className}.Add({i}, new {className}Row");
                    sb.AppendLine("        {");
                    for (int j = 0; j < headers.Length; j++)
                    {
                        sb.AppendLine($"            {headers[j]} = \"{row[j]}\"");
                    }
                    sb.AppendLine("        });");
                }

                sb.AppendLine("    }");
                sb.AppendLine();
            }

            // 공용 클래스 파일 끝
            sb.AppendLine("}");

            File.WriteAllText(dataClassPath, sb.ToString());
        }
    }
}
