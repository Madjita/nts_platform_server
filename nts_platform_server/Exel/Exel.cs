using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using nts_platform_server.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;

using System.IO.Compression;



namespace nts_platform_server
{

    public class HPM
    {
        public string SYSTEMENTITY;
        public string TYPE;
        public string PARAMETERS;
        public string VALUE;
    }

    public class CreateExelType
    {
        public byte[] bytes;
        public string fileName;
    }

    public class Exel
    {
        IFormFile _file;
        public MemoryStream stream;
        public byte[] bytes;

        Dictionary<int, HPM> ListAll;

        Dictionary<int, HPM> List_ANINNIM;
        Dictionary<int, HPM> List_ANOUTNIM;
        Dictionary<int, HPM> List_ARRAY;
        Dictionary<int, HPM> List_DIINNIM;
        Dictionary<int, HPM> List_FLAGNIM;
        Dictionary<int, HPM> List_LOGICNIM;
        Dictionary<int, HPM> List_NUMERNIM;
        Dictionary<int, HPM> List_PRMODNIM;
        Dictionary<int, HPM> List_REGCLNIM;
        Dictionary<int, HPM> List_REGPVNIM;


        public List<string> List_ANINNIM_COLUMN;
        public List<string> List_ANOUTNIM_COLUMN;
        public List<string> List_ARRAY_COLUMN;
        public List<string> List_DIINNIM_COLUMN;
        public List<string> List_FLAGNIM_COLUMN;
        public List<string> List_LOGICNIM_COLUMN;
        public List<string> List_NUMERNIM_COLUMN;
        public List<string> List_PRMODNIM_COLUMN;
        public List<string> List_REGCLNIM_COLUMN;
        public List<string> List_REGPVNIM_COLUMN;

        public string title;

        public List<string> titles = new List<string>
        {
            "ANINNIM",
            "ANOUTNIM",
            "ARRAY",
            "DIINNIM",
            "FLAGNIM",
            "LOGICNIM",
            "NUMERNIM",
            "PRMODNIM",
            "REGCLNIM",
            "REGPVNIM",
        };



        List<string> Columns = new List<string>
        {
            "A","B","C","D","E","F","G",
        };



        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }


        public byte[] CreateZipArchive()
        {
            var stream = new MemoryStream();

            string sourceFolder = @"zip"; // исходная папка
            string zipFile = "test.zip"; // сжатый файл
            // string targetFolder = "newtest"; // папка, куда распаковывается файл


           
            ZipFile.CreateFromDirectory(sourceFolder, zipFile);

            //ZipFile.ExtractToDirectory(zipFile, targetFolder);


            using (FileStream file = new FileStream("test.zip", FileMode.Open, FileAccess.Read))
            {
                file.CopyTo(stream);
            }

            //Очистить папку
            DirectoryInfo dir = new DirectoryInfo(sourceFolder);
            foreach (FileInfo f in dir.GetFiles())
            {
                f.Delete();
            }

            FileInfo fileInf = new FileInfo("test.zip");
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }

            return stream.ToArray();
        }


        public Exel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }


        public CreateExelType createExelHour(string template,Week week, UserProject userProject,bool zipMode)
        {
            //Создание даты на неделю по номеру недели

            int year = week.Year % 100;
            string fileName = "cw" + year + week.NumberWeek + "_" + userProject.Project.Code + "_" + userProject.User.FirstName + "_" + userProject.User.SecondName + "_Hour_report";
            fileName += ".xlsx";


            byte[] bytes = null;

            var stream = new MemoryStream();
            using (FileStream file = new FileStream(template, FileMode.Open, FileAccess.Read))
                file.CopyTo(stream);


            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet _sheet = package.Workbook.Worksheets[0];
                _sheet.Cells["A8"].Value = userProject.User.FirstName + " " + userProject.User.SecondName;
                //_sheet.Cells[2, 3].Value = "NTS";


                DateTime dataStartWeek = FirstDateOfWeekISO8601(week.Year, week.NumberWeek);

                var data = dataStartWeek;
                int startCell = 14;
                string stringCell = "A" + startCell.ToString();
                _sheet.Cells[stringCell].Value = data.ToString("dd.MM.yyyy");
                for (int i = 1; i < 7; i++)
                {
                    data = data.AddDays(1);
                    startCell++;
                    stringCell = "A" + startCell.ToString();
                    _sheet.Cells[stringCell].Value = data.ToString("dd.MM.yyyy");
                }


                _sheet.Cells["C14"].Value = userProject.Project.Code;
                _sheet.Cells["C15"].Value = userProject.Project.Code;
                _sheet.Cells["C16"].Value = userProject.Project.Code;
                _sheet.Cells["C17"].Value = userProject.Project.Code;
                _sheet.Cells["C18"].Value = userProject.Project.Code;
                _sheet.Cells["C19"].Value = userProject.Project.Code;
                _sheet.Cells["C20"].Value = userProject.Project.Code;

                _sheet.Cells["D14"].Value = week.MoHour.ActivityCode;
                _sheet.Cells["D15"].Value = week.TuHour.ActivityCode;
                _sheet.Cells["D16"].Value = week.WeHour.ActivityCode;
                _sheet.Cells["D17"].Value = week.ThHour.ActivityCode;
                _sheet.Cells["D18"].Value = week.FrHour.ActivityCode;
                _sheet.Cells["D19"].Value = week.SaHour.ActivityCode;
                _sheet.Cells["D20"].Value = week.SuHour.ActivityCode;


                _sheet.Cells["I14"].Value = week.MoHour.WTHour;
                _sheet.Cells["I15"].Value = week.TuHour.WTHour;
                _sheet.Cells["I16"].Value = week.WeHour.WTHour;
                _sheet.Cells["I17"].Value = week.ThHour.WTHour;
                _sheet.Cells["I18"].Value = week.FrHour.WTHour;
                _sheet.Cells["I19"].Value = week.SaHour.WTHour;
                _sheet.Cells["I20"].Value = week.SuHour.WTHour;

                // do work here

                if(zipMode)
                {
                    package.SaveAs(new FileInfo("zip/"+fileName));
                }

                bytes = package.GetAsByteArray();
            }

            return new CreateExelType
            {
                bytes = bytes,
                fileName = fileName
            };
        }

        /*public Exel(string fileName, string template,UserProject userProject)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            stream = new MemoryStream();
            using (FileStream file = new FileStream(template, FileMode.Open, FileAccess.Read))
                file.CopyTo(stream);


            using (ExcelPackage package = new ExcelPackage(stream))
            {
                //"Hour report file"
                //ExcelWorksheet _sheet = package.Workbook.Worksheets.Add("New");

                ExcelWorksheet _sheet = package.Workbook.Worksheets[0];
                _sheet.Cells["A8"].Value = userProject.User.FirstName + " " + userProject.User.SecondName;
                //_sheet.Cells[2, 3].Value = "NTS";



                //Создание даты на неделю по номеру недели
                var week = userProject.Weeks.FirstOrDefault();

                DateTime dataStartWeek = FirstDateOfWeekISO8601(week.Year, week.NumberWeek);

                var data = dataStartWeek;
                int startCell = 14;
                string stringCell = "A" + startCell.ToString();
                _sheet.Cells[stringCell].Value = data.ToString("dd.MM.yyyy");
                for (int i=1; i < 7;i++)
                {
                    data = data.AddDays(1);
                    startCell++;
                    stringCell = "A" + startCell.ToString();
                    _sheet.Cells[stringCell].Value = data.ToString("dd.MM.yyyy");
                }


                _sheet.Cells["C14"].Value = userProject.Project.Code;
                _sheet.Cells["C15"].Value = userProject.Project.Code;
                _sheet.Cells["C16"].Value = userProject.Project.Code;
                _sheet.Cells["C17"].Value = userProject.Project.Code;
                _sheet.Cells["C18"].Value = userProject.Project.Code;
                _sheet.Cells["C19"].Value = userProject.Project.Code;
                _sheet.Cells["C20"].Value = userProject.Project.Code;

                _sheet.Cells["D14"].Value = week.MoHour.ActivityCode;
                _sheet.Cells["D15"].Value = week.TuHour.ActivityCode;
                _sheet.Cells["D16"].Value = week.WeHour.ActivityCode;
                _sheet.Cells["D17"].Value = week.ThHour.ActivityCode;
                _sheet.Cells["D18"].Value = week.FrHour.ActivityCode;
                _sheet.Cells["D19"].Value = week.SaHour.ActivityCode;
                _sheet.Cells["D20"].Value = week.SuHour.ActivityCode;


                _sheet.Cells["I14"].Value = week.MoHour.WTHour;
                _sheet.Cells["I15"].Value = week.TuHour.WTHour;
                _sheet.Cells["I16"].Value = week.WeHour.WTHour;
                _sheet.Cells["I17"].Value = week.ThHour.WTHour;
                _sheet.Cells["I18"].Value = week.FrHour.WTHour;
                _sheet.Cells["I19"].Value = week.SaHour.WTHour;
                _sheet.Cells["I20"].Value = week.SuHour.WTHour;

                // do work here                            
                //package.SaveAs(new FileInfo(fileName));

                bytes = package.GetAsByteArray();
            }
        }*/



        public Exel([FromForm] IFormFile file)
        {
            _file = file;

            //string end = "";
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public void CreateExel()
        {
            using (var package = new ExcelPackage())
            {

                foreach (var name_Worksheets in titles)
                {
                    string name_HPM = title;



                    ExcelWorksheet _sheet = package.Workbook.Worksheets.Add(name_HPM + " " + name_Worksheets);

                    string tag = null;
                    int id_value = 0;
                    int id = 2;
                    int col = 3;


                    _sheet.Cells[1, 1].Value = "Row";
                    _sheet.Cells[1, 2].Value = "SYSTEM ENTITY";


                    List<string> list_column = new List<string>();

                    Dictionary<int, HPM> List_dict = new Dictionary<int, HPM>();

                    //
                    if (name_Worksheets == "ANINNIM")
                    {
                        List_dict = List_ANINNIM;
                        list_column = List_ANINNIM_COLUMN;
                    }

                    if (name_Worksheets == "ANOUTNIM")
                    {
                        List_dict = List_ANOUTNIM;
                        list_column = List_ANOUTNIM_COLUMN;
                    }

                    if (name_Worksheets == "ARRAY")
                    {
                        List_dict = List_ARRAY;
                        list_column = List_ARRAY_COLUMN;
                    }

                    if (name_Worksheets == "DIINNIM")
                    {
                        List_dict = List_DIINNIM;
                        list_column = List_DIINNIM_COLUMN;
                    }

                    if (name_Worksheets == "FLAGNIM")
                    {
                        List_dict = List_FLAGNIM;
                        list_column = List_FLAGNIM_COLUMN;
                    }

                    if (name_Worksheets == "LOGICNIM")
                    {
                        List_dict = List_LOGICNIM;
                        list_column = List_LOGICNIM_COLUMN;
                    }

                    if (name_Worksheets == "NUMERNIM")
                    {
                        List_dict = List_NUMERNIM;
                        list_column = List_NUMERNIM_COLUMN;
                    }
                    if (name_Worksheets == "PRMODNIM")
                    {
                        List_dict = List_PRMODNIM;
                        list_column = List_PRMODNIM_COLUMN;
                    }

                    if (name_Worksheets == "REGCLNIM")
                    {
                        List_dict = List_REGCLNIM;
                        list_column = List_REGCLNIM_COLUMN;
                    }

                    if (name_Worksheets == "REGPVNIM")
                    {
                        List_dict = List_REGPVNIM;
                        list_column = List_REGPVNIM_COLUMN;
                    }
                    //


                    for (int i = 0; i < list_column.Count; i++)
                    {
                        _sheet.Cells[1, col + i].Value = list_column[i];
                    }



                    foreach (KeyValuePair<int, HPM> entry in List_dict)
                    {
                        // do something with entry.Value or entry.Key

                        if (tag == null)
                        {
                            tag = entry.Value.SYSTEMENTITY;
                            id_value = entry.Key;
                            id = 2;

                            _sheet.Cells[id, 1].Value = id_value;

                            _sheet.Cells[id, 2].Value = tag;
                        }

                        if (tag != entry.Value.SYSTEMENTITY)
                        {
                            tag = entry.Value.SYSTEMENTITY;
                            id_value = entry.Key;
                            id++;

                            _sheet.Cells[id, 1].Value = id_value;
                            _sheet.Cells[id, 2].Value = tag;
                        }

                        col = list_column.FindIndex(x => x == entry.Value.PARAMETERS);
                        col += 3;

                        _sheet.Cells[id, col].Value = entry.Value.VALUE;

                    }

                    _sheet.Cells.AutoFitColumns();

                }


                package.SaveAs(new FileInfo(title+"_"+ @"basicUsage.xlsx"));
            }
        }


        public Dictionary<int, HPM> GetRowHPM(int Worksheets)
        {
            ListAll = new Dictionary<int, HPM>();

            List_ANINNIM = new Dictionary<int, HPM>();
            List_ANOUTNIM = new Dictionary<int, HPM>();
            List_ARRAY = new Dictionary<int, HPM>();
            List_DIINNIM = new Dictionary<int, HPM>();
            List_FLAGNIM = new Dictionary<int, HPM>();
            List_LOGICNIM = new Dictionary<int, HPM>();
            List_NUMERNIM = new Dictionary<int, HPM>();
            List_PRMODNIM = new Dictionary<int, HPM>();
            List_REGCLNIM = new Dictionary<int, HPM>();
            List_REGPVNIM = new Dictionary<int, HPM>();


            //

            List_ANINNIM_COLUMN = new List<string>();
            List_ANOUTNIM_COLUMN = new List<string>();
            List_ARRAY_COLUMN = new List<string>();
            List_DIINNIM_COLUMN = new List<string>();
            List_FLAGNIM_COLUMN = new List<string>();
            List_LOGICNIM_COLUMN = new List<string>();
            List_NUMERNIM_COLUMN = new List<string>();
            List_PRMODNIM_COLUMN = new List<string>();
            List_REGCLNIM_COLUMN = new List<string>();
            List_REGPVNIM_COLUMN = new List<string>();

            //

            using (ExcelPackage _package = new ExcelPackage(_file.OpenReadStream()))
            {

                ExcelWorksheet _sheet = _package.Workbook.Worksheets[Worksheets];


                title = _sheet.Name;

                var start = _sheet.Dimension.Start;
                var end = _sheet.Dimension.End;

                for (int row = start.Row; row <= end.Row; row++)
                { // Row by row...
                    if (row == 1)
                        continue;

                    var hpm = new HPM
                    {
                        SYSTEMENTITY = "",
                        TYPE = "",
                        PARAMETERS = "",
                        VALUE = "",
                    };


                    for (int col = start.Column; col <= end.Column; col++)
                    { // ... Cell by cell...

                        if (col == 1)
                            continue;

                        object cellValue = _sheet.Cells[row, col].Text; // This got me the actual value I needed.
                        string item = cellValue.ToString();


                        switch (col)
                        {
                            //SYSTEMENTITY
                            case 2:
                                if (item != "" && row != 1)
                                {
                                    hpm.SYSTEMENTITY = item;
                                }
                                break;
                            //TYPE
                            case 3:
                                if (item != "" && row != 1)
                                {
                                    hpm.TYPE = item;
                                }
                                break;
                            //PARAMETERS
                            case 4:
                                if (item != "" && row != 1)
                                {
                                    hpm.PARAMETERS = item;
                                }
                                break;
                            //VALUE
                            case 5:
                                if (item != "" && row != 1)
                                {
                                    hpm.VALUE = item;
                                }
                                break;
                        }

                    }

                    if (hpm.SYSTEMENTITY != "")
                    {
                        ListAll.Add(row, hpm);


                        if(hpm.TYPE == "ANINNIM")
                        {
                            List_ANINNIM.Add(row, hpm);

                            if(List_ANINNIM_COLUMN.Contains(hpm.PARAMETERS) == false)
                            {
                                List_ANINNIM_COLUMN.Add(hpm.PARAMETERS);
                            }
                        }

                        if (hpm.TYPE == "ANOUTNIM")
                        {
                            List_ANOUTNIM.Add(row, hpm);

                            if (List_ANOUTNIM_COLUMN.Contains(hpm.PARAMETERS) == false)
                            {
                                List_ANOUTNIM_COLUMN.Add(hpm.PARAMETERS);
                            }
                        }

                        if (hpm.TYPE == "ARRAY")
                        {
                            List_ARRAY.Add(row, hpm);

                            if (List_ARRAY_COLUMN.Contains(hpm.PARAMETERS) == false)
                            {
                                List_ARRAY_COLUMN.Add(hpm.PARAMETERS);
                            }
                        }

                        if (hpm.TYPE == "DIINNIM")
                        {
                            List_DIINNIM.Add(row, hpm);

                            if (List_DIINNIM_COLUMN.Contains(hpm.PARAMETERS) == false)
                            {
                                List_DIINNIM_COLUMN.Add(hpm.PARAMETERS);
                            }
                        }

                        if (hpm.TYPE == "FLAGNIM")
                        {
                            List_FLAGNIM.Add(row, hpm);

                            if (List_FLAGNIM_COLUMN.Contains(hpm.PARAMETERS) == false)
                            {
                                List_FLAGNIM_COLUMN.Add(hpm.PARAMETERS);
                            }
                        }

                        if (hpm.TYPE == "LOGICNIM")
                        {
                            List_LOGICNIM.Add(row, hpm);

                            if (List_LOGICNIM_COLUMN.Contains(hpm.PARAMETERS) == false)
                            {
                                List_LOGICNIM_COLUMN.Add(hpm.PARAMETERS);
                            }
                        }

                        if (hpm.TYPE == "NUMERNIM")
                        {
                            List_NUMERNIM.Add(row, hpm);

                            if (List_NUMERNIM_COLUMN.Contains(hpm.PARAMETERS) == false)
                            {
                                List_NUMERNIM_COLUMN.Add(hpm.PARAMETERS);
                            }
                        }
                        if (hpm.TYPE == "PRMODNIM")
                        {
                            List_PRMODNIM.Add(row, hpm);

                            if (List_PRMODNIM_COLUMN.Contains(hpm.PARAMETERS) == false)
                            {
                                List_PRMODNIM_COLUMN.Add(hpm.PARAMETERS);
                            }
                        }

                        if (hpm.TYPE == "REGCLNIM")
                        {
                            List_REGCLNIM.Add(row, hpm);

                            if (List_REGCLNIM_COLUMN.Contains(hpm.PARAMETERS) == false)
                            {
                                List_REGCLNIM_COLUMN.Add(hpm.PARAMETERS);
                            }
                        }

                        if (hpm.TYPE == "REGPVNIM")
                        {
                            List_REGPVNIM.Add(row, hpm);

                            if (List_REGPVNIM_COLUMN.Contains(hpm.PARAMETERS) == false)
                            {
                                List_REGPVNIM_COLUMN.Add(hpm.PARAMETERS);
                            }
                        }

                    }
                }

                _package.Dispose();

            }

            return ListAll;
        }




        /*
        public Dictionary<int, User> GetRowDataUsers()
        {
            if (_file == null)
                return null;

            var list = new Dictionary<int, User>();
            using (ExcelPackage _package = new ExcelPackage(_file.OpenReadStream()))
            {
                ExcelWorksheet _sheet = _package.Workbook.Worksheets[0];

                var start = _sheet.Dimension.Start;
                var end = _sheet.Dimension.End;
                for (int row = start.Row; row <= end.Row; row++)
                { // Row by row...

                    if (row == 1)
                        continue;

                    var user = new User
                    {
                        Name = "",
                        Email = "",
                        Password = ""
                    };

                    for (int col = start.Column; col <= end.Column; col++)
                    { // ... Cell by cell...
                        object cellValue = _sheet.Cells[row, col].Text; // This got me the actual value I needed.
                        string item = cellValue.ToString();


                        switch (col)
                        {
                            //Name
                            case 2:
                                if (item != "" && row != 1)
                                {
                                    user.Name = item;
                                }
                                break;

                            //Почта
                            case 7:
                                if (item != "" && row != 1)
                                {
                                    user.Email = item;
                                }
                                break;

                            //Пароль
                            case 8:
                                if (item != "" && row != 1)
                                {
                                    //user.Password = BCrypt.Net.BCrypt.HashPassword(item);
                                }
                                break;
                        }
                    }

                    if (user.Name != "")
                    {
                        list.Add(row, user);
                    }
                }


                _package.Dispose();
            }

            return list;
        }


        public Dictionary<int, Productions> GetRowDataProductions()
        {
            if (_file == null)
                return null;

            var list = new Dictionary<int, Productions>();
            using (ExcelPackage _package = new ExcelPackage(_file.OpenReadStream()))
            {
                ExcelWorksheet _sheet = _package.Workbook.Worksheets["Production"];

                var start = _sheet.Dimension.Start;
                var end = _sheet.Dimension.End;
                for (int row = start.Row; row <= end.Row; row++)
                { // Row by row...

                    if (row == 1)
                        continue;

                    var productions = new Productions
                    {
                        Name = "",
                    };

                    for (int col = start.Column; col <= end.Column; col++)
                    { // ... Cell by cell...
                        object cellValue = _sheet.Cells[row, col].Text; // This got me the actual value I needed.
                        string item = cellValue.ToString();


                        switch (col)
                        {
                            //Name
                            case 2:
                                if (item != "" && row != 1)
                                {
                                    productions.Name = item;
                                }
                                break;
                        }
                    }

                    if (productions.Name != "")
                    {
                        list.Add(row, productions);
                    }
                }

                _package.Dispose();
            }

            return list;
        }


        public Dictionary<int, BufferVSM> GetRowDataBufferVSM()
        {
            if (_file == null)
                return null;

            var list = new Dictionary<int, BufferVSM>();

            using (ExcelPackage _package = new ExcelPackage(_file.OpenReadStream()))
            {
                ExcelWorksheet _sheet = _package.Workbook.Worksheets[2];

                var start = _sheet.Dimension.Start;
                var end = _sheet.Dimension.End;
                for (int row = start.Row; row <= end.Row; row++)
                { // Row by row...

                    if (row == 1)
                        continue;

                    var bufferSVSM = new BufferVSM
                    {
                        Name = "",
                        Type = "",
                        MinHold = 0,
                        Max = 0,
                        Value = 0,
                        ValueDefault = 0,
                        ReplenishmentSec = 0,
                        ReplenishmentCount = 0
                    };

                    for (int col = start.Column; col <= end.Column; col++)
                    { // ... Cell by cell...
                        object cellValue = _sheet.Cells[row, col].Text; // This got me the actual value I needed.
                        string item = cellValue.ToString();


                        switch (col)
                        {
                            //Name
                            case 2:
                                if (item != "" && row != 1)
                                {
                                    bufferSVSM.Name = item;
                                }
                                break;


                            //Type
                            case 3:
                                if (item != "" && row != 1)
                                {
                                    bufferSVSM.Type = item;
                                }
                                break;

                            //MinHold
                            case 4:
                                if (item != "" && row != 1)
                                {
                                    bufferSVSM.MinHold = int.Parse(item);
                                }
                                break;

                            //Max
                            case 5:
                                if (item != "" && row != 1)
                                {
                                    bufferSVSM.Max = int.Parse(item);
                                }
                                break;

                            //Value
                            case 6:
                                if (item != "" && row != 1)
                                {
                                    bufferSVSM.Value = int.Parse(item);
                                }
                                break;

                            //ValueDefault
                            case 7:
                                if (item != "" && row != 1)
                                {
                                    bufferSVSM.ValueDefault = int.Parse(item);
                                }
                                break;

                            //ReplenishmentSec
                            case 8:
                                if (item != "" && row != 1)
                                {
                                    bufferSVSM.ReplenishmentSec = int.Parse(item);
                                }
                                break;

                            //ReplenishmentCount
                            case 9:
                                if (item != "" && row != 1)
                                {
                                    bufferSVSM.ReplenishmentCount = int.Parse(item);
                                }
                                break;
                        }
                    }

                    if (bufferSVSM.Name != "")
                    {
                        list.Add(row, bufferSVSM);
                    }
                }


                _package.Dispose();
            }

            return list;
        }

        public Dictionary<int, EtapVSM> GetRowDataEtapVSM()
        {
            if (_file == null)
                return null;

            var list = new Dictionary<int, EtapVSM>();

            using (ExcelPackage _package = new ExcelPackage(_file.OpenReadStream()))
            {
                ExcelWorksheet _sheet = _package.Workbook.Worksheets[3];

                var start = _sheet.Dimension.Start;
                var end = _sheet.Dimension.End;
                for (int row = start.Row; row <= end.Row; row++)
                { // Row by row...

                    if (row == 1)
                        continue;

                    var etapVSM = new EtapVSM
                    {
                        Name = "",
                        Description = "",
                        DefaultTimeCircle = 0,
                        DefaultTimePreporation = 0,
                        DefaultAvailability = 0,
                        Parallel = false,
                    };

                    for (int col = start.Column; col <= end.Column; col++)
                    { // ... Cell by cell...
                        object cellValue = _sheet.Cells[row, col].Text; // This got me the actual value I needed.
                        string item = cellValue.ToString();


                        switch (col)
                        {
                            //Name
                            case 2:
                                if (item != "" && row != 1)
                                {
                                    etapVSM.Name = item;
                                }
                                break;


                            //Description
                            case 3:
                                if (item != "" && row != 1)
                                {
                                    etapVSM.Description = item;
                                }
                                break;

                            //DefaultTimeCircle
                            case 4:
                                if (item != "" && row != 1)
                                {
                                    etapVSM.DefaultTimeCircle = int.Parse(item);
                                }
                                break;

                            //DefaultTimePreporation
                            case 5:
                                if (item != "" && row != 1)
                                {
                                    etapVSM.DefaultTimePreporation = int.Parse(item);
                                }
                                break;

                            //Value
                            case 6:
                                if (item != "" && row != 1)
                                {
                                    etapVSM.DefaultAvailability = int.Parse(item);
                                }
                                break;

                            //ValueDefault
                            case 7:
                                if (item != "" && row != 1)
                                {
                                    etapVSM.Parallel = int.Parse(item) > 0 ? true : false;
                                }
                                break;
                        }
                    }

                    if (etapVSM.Name != "")
                    {
                        list.Add(row, etapVSM);
                    }
                }


                _package.Dispose();
            }

            return list;
        }


        public Dictionary<int, JObject> GetRowDataEtapSections()
        {
            if (_file == null)
                return null;

            var list = new Dictionary<int, JObject>();

            using (ExcelPackage _package = new ExcelPackage(_file.OpenReadStream()))
            {
                ExcelWorksheet _sheet = _package.Workbook.Worksheets[4];

                var start = _sheet.Dimension.Start;
                var end = _sheet.Dimension.End;
                for (int row = start.Row; row <= end.Row; row++)
                { // Row by row...

                    if (row == 1)
                        continue;

                    string etap = "";
                    string worker_name = "";

                    for (int col = start.Column; col <= end.Column; col++)
                    { // ... Cell by cell...
                        object cellValue = _sheet.Cells[row, col].Text; // This got me the actual value I needed.
                        string item = cellValue.ToString();


                        switch (col)
                        {
                            //Etap
                            case 2:
                                if (item != "" && row != 1)
                                {
                                    etap = item;
                                }
                                break;


                            //Worker name
                            case 3:
                                if (item != "" && row != 1)
                                {
                                    worker_name = item;
                                }
                                break;
                        }
                    }

                    if (etap != "" && worker_name != "")
                    {
                        var obj = new JObject();
                        obj.Add(new JProperty("etap", etap));
                        obj.Add(new JProperty("worker", worker_name));
                        list.Add(row, obj);
                    }
                }


                _package.Dispose();
            }

            return list;
        }



        public Dictionary<int, JObject> GetRowDataCardVSM()
        {
            if (_file == null)
                return null;

            var list = new Dictionary<int, JObject>();

            using (ExcelPackage _package = new ExcelPackage(_file.OpenReadStream()))
            {
                ExcelWorksheet _sheet = _package.Workbook.Worksheets[1];

                var start = _sheet.Dimension.Start;
                var end = _sheet.Dimension.End;


                string production = "";

                for (int row = start.Row; row <= end.Row; row++)
                { // Row by row...

                    if (row == 1)
                        continue;

                    string bufer = "";
                    string etap = "";
                    string sections = "";


                    for (int col = start.Column; col <= end.Column; col++)
                    { // ... Cell by cell...
                        object cellValue = _sheet.Cells[row, col].Text; // This got me the actual value I needed.
                        string item = cellValue.ToString();


                        switch (col)
                        {

                            //Production
                            case 1:
                                if (item != "" && row != 1)
                                {
                                    production = item;
                                }
                                break;

                            //Buffer
                            case 2:
                                if (item != "" && row != 1)
                                {
                                    bufer = item;
                                }
                                break;


                            //Etap
                            case 3:
                                if (item != "" && row != 1)
                                {
                                    etap = item;
                                }
                                break;

                            //Sections
                            case 4:
                                if (item != "" && row != 1)
                                {
                                    sections = item;
                                }
                                break;
                        }
                    }

                    if (production != "" && bufer != "" && etap != "" && sections != "")
                    {
                        var obj = new JObject();
                        obj.Add(new JProperty("production", production));
                        obj.Add(new JProperty("bufer", bufer));
                        obj.Add(new JProperty("etap", etap));
                        obj.Add(new JProperty("sections", sections));
                        list.Add(row, obj);
                    }
                }


                _package.Dispose();
            }

            return list;
        }
        */

    }
}
