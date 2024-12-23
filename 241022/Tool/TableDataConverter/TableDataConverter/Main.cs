using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ExcelDataReader;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace TableDataConverter
{
    public partial class Main
    {
        Form1 mainForm;
        private DataManager dataManager;
        string csvPath = "";
        string excelPath = "";

        public Main(Form1 form)
        {
            mainForm = form;
            dataManager = new DataManager();
            dataManager.putConsole += new DataManager.SendDataEventHandler(mainForm.PutConsole);
        }

        public async Task Execute()
        {
            DirectoryInfo excelDirectory = new DirectoryInfo(csvPath);
            if (false == excelDirectory.Exists)
            {
                throw new Exception("엑셀 폴더를 찾을 수 없습니다.");
            }

            var excelFiles = excelDirectory.EnumerateFiles("*.*", SearchOption.AllDirectories).Where(f => Config.ExcelFileList.Contains(f.Name));
            if (excelFiles.Count() <= 0)
            {
                throw new Exception("해당 폴더에 엑셀 파일을 찾을 수 없습니다.");
            }

            var file = excelDirectory.GetFiles(excelPath, SearchOption.AllDirectories).First();
            if (null != file)
            {
                excelFiles = excelFiles.Append(file);
            }

            Dictionary<string/*excelFileName*/, DataSet> excelDatas = new Dictionary<string, DataSet>();

            await Task.Run(() =>
            {
                mainForm.PutConsole("Excel 로드중...");
                foreach (FileInfo excelFile in excelFiles)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    using (var stream = File.Open(excelFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        ExcelReaderConfiguration configuration = new ExcelReaderConfiguration();
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream, configuration))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { }
                            });

                            excelDatas.Add(excelFile.Name, result);
                        }
                    }
                    stopwatch.Stop();

                    mainForm.PutConsole(string.Format("Load {0} | {1} ms", excelFile.Name, stopwatch.ElapsedMilliseconds));
                }
                mainForm.PutConsole("Excel 로드 완료.");

                dataManager.Init();

                mainForm.PutConsole("Config CSV 생성중...");
                foreach (var excelData in excelDatas)
                {
                    var retPath = excelData.Key;
                    //dataManager.MakeCSVDataConfig(excelData.Key, excelData.Value);
                }
                mainForm.PutConsole("Config CSV 생성 완료.");

                mainForm.PutConsole("Enum CSV 생성 중...");
                foreach (var excelData in excelDatas)
                {
                    //dataManager.MakeCSVEnumSheet(excelData.Key, excelData.Value);
                }
                mainForm.PutConsole("Enum CSV 생성 완료.");

                //if (null != Config.ConstantTable)
                //{
                //    mainForm.PutConsole(string.Format("ConstantTable({0}) CSV 생성 중...", Config.ConstantTable.Excel));

                //    dataManager.MakeConstantTable(Config.ConstantTable.Excel, Config.ConstantTable.Sheet, excelDatas[Config.ConstantTable.Excel], Config.Zone, Config.Version);

                //    mainForm.PutConsole(string.Format("ConstantTable({0}) CSV 생성 완료.", Config.ConstantTable.Excel));
                //}

                mainForm.PutConsole("Data Sheet 생성 중...");
                foreach (var excelData in excelDatas)
                {
                    //dataManager.MakeCSVDataSheet(excelData.Key, excelData.Value, Config.Zone, Config.Version);
                }
                mainForm.PutConsole("Data CSV 생성 완료.");
            });

            mainForm.PutConsole("DataBuild Execute Success!");
        }
    }
}
