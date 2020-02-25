using CrowdFundingTest.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingTest.Services
{
    public class ExceLIO
    {
        public List<User> ReadExcel(string filename)
        {
            XSSFWorkbook hssfwb;
            using (FileStream file = new FileStream(filename,
                FileMode.Open, FileAccess.Read))
            {
                hssfwb = new XSSFWorkbook(file);
            }
            ISheet sheet = hssfwb.GetSheet("Person");
            List<User> persons = new List<User>();
            for (int row = 0; row <= sheet.LastRowNum; row++)
            {
                if (sheet.GetRow(row) != null)
                //null is when the row only contains empty cells
                {
                    User c = new User
                    {
                        Name = sheet.GetRow(row).GetCell(0).StringCellValue,
                        Email = sheet.GetRow(row).GetCell(2).StringCellValue
                    };
                    persons.Add(c);
                }
            }
            return persons;
        }
    }
}



