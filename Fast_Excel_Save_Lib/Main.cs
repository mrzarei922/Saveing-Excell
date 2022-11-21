using Fast_Excel_Save_Lib.Models;
using IronXL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fast_Excel_Save_Lib
{
    public class Main
    {
        public string Excel_Saving(String Url)
        {
            if (string.IsNullOrEmpty(Url))
            {
                return "file not found";
            }
            try
            {
                //ReadData Excell
                WorkBook workbook = WorkBook.Load(Url);
                WorkSheet sheet = workbook.WorkSheets.First();

                var program = new Main();
                //Saving  data in redis
                program.SaveBigData(sheet.Columns[1].Count(), sheet);

                //saving data in SqlServer
                program.SaveDataSql(sheet.Columns[1].Count());

            }
            catch (Exception ex)
            {
                return (ex.Message.ToString());
                throw;
            }

            return "true";


        }

       
        public void SaveBigData(int count, WorkSheet sheetread)
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            foreach (var cell in sheetread.Columns[1])
            {
                var value = cell.Text;
                cache.StringSetAsync($"Device_Status:{cell.RowIndex}", value);
            }
        }

        public void SaveDataSql(int count)
        {
            testDBContext _context = new testDBContext();
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            List<ExellExport> record = new List<ExellExport>();
            ExellExport t = new ExellExport();
            for (int i = 0; i <= count; i++)
            {
                var value = cache.StringGetAsync($"Device_Status:{i}").Result;
                if (!string.IsNullOrEmpty(value))
                {
                    record.Add(new ExellExport
                    {
                        Export = value
                    });
                }
            }
            _context.ExellExports.AddRange(record);
            _context.SaveChanges();
           
        }

    }
}
