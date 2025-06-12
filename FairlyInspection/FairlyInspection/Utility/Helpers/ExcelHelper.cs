using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace FairlyInspection.Utility
{
    public class ExcelHelper
    {
        /// <summary>讀取excel 到datatable    
        /// 預設第一行為表頭，匯入第一個工作表   
        /// </summary>      
        /// <param name="strFileName">excel文件路徑</param>      
        /// <returns></returns>      
        public static DataTable ExcelToDataTable(string strFileName)
        {
            DataTable dt = new DataTable();
            FileStream file = null;
            IWorkbook Workbook = null;
            try
            {

                using (file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))//C#文件流讀取文件
                {
                    if (strFileName.IndexOf(".xlsx") > 0)
                        //把xlsx文件中的資料寫入Workbook中
                        Workbook = new XSSFWorkbook(file);

                    else if (strFileName.IndexOf(".xls") > 0)
                        //把xls文件中的資料寫入Workbook中
                        Workbook = new HSSFWorkbook(file);

                    if (Workbook != null)
                    {
                        ISheet sheet = Workbook.GetSheetAt(0);//讀取第一個sheet
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        //得到Excel工作表的行 
                        IRow headerRow = sheet.GetRow(0);
                        //得到Excel工作表的總列數  
                        int cellCount = headerRow.LastCellNum;

                        for (int j = 0; j < cellCount; j++)
                        {
                            //得到Excel工作表指定行的單元格  
                            ICell cell = headerRow.GetCell(j);
                            dt.Columns.Add(cell.ToString());
                        }

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            DataRow dataRow = dt.NewRow();

                            if (row != null)
                            {
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (row.GetCell(j) != null)
                                        dataRow[j] = row.GetCell(j).ToString();
                                }
                                dt.Rows.Add(dataRow);
                            }
                        }
                    }
                    return dt;
                }
            }

            catch (Exception ex)
            {
                if (file != null)
                {
                    file.Close();//關閉當前流並釋放資源
                }
                return null;
            }

        }
        /// <summary>   
        /// 從Excel中獲取資料到DataTable   
        /// </summary>   
        /// <param name="strFileName">Excel文件全路徑(伺服器路徑)</param>   
        /// <param name="SheetName">要獲取資料的工作表名稱</param>   
        /// <param name="HeaderRowIndex">工作表標題行所在行號(從0開始)</param>   
        /// <returns></returns>   
        public static DataTable RenderDataTableFromExcel(string strFileName, string SheetName, int HeaderRowIndex)
        {
            IWorkbook Workbook = null;

            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (strFileName.IndexOf(".xlsx") > 0)

                    Workbook = new XSSFWorkbook(file);

                else if (strFileName.IndexOf(".xls") > 0)

                    Workbook = new HSSFWorkbook(file);
                ISheet sheet = Workbook.GetSheet(SheetName);
                return RenderDataTableFromExcel(Workbook, SheetName, HeaderRowIndex);
            }
        }

        /// <summary>   
        /// 從Excel中獲取資料到DataTable   
        /// </summary>   
        /// <param name="workbook">要處理的工作薄</param>   
        /// <param name="SheetName">要獲取資料的工作表名稱</param>   
        /// <param name="HeaderRowIndex">工作表標題行所在行號(從0開始)</param>   
        /// <returns></returns>   
        public static DataTable RenderDataTableFromExcel(IWorkbook workbook, string SheetName, int HeaderRowIndex)
        {
            ISheet sheet = workbook.GetSheet(SheetName);
            DataTable table = new DataTable();
            try
            {
                IRow headerRow = sheet.GetRow(HeaderRowIndex);
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                int rowCount = sheet.LastRowNum;

                #region 迴圈各行各列,寫入資料到DataTable
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell == null)
                        {
                            dataRow[j] = null;
                        }
                        else
                        {
                            //dataRow[j] = cell.ToString();   
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                    dataRow[j] = null;
                                    break;
                                case CellType.Boolean:
                                    dataRow[j] = cell.BooleanCellValue;
                                    break;
                                case CellType.Numeric:
                                    dataRow[j] = cell.ToString();
                                    break;
                                case CellType.String:
                                    dataRow[j] = cell.StringCellValue;
                                    break;
                                case CellType.Error:
                                    dataRow[j] = cell.ErrorCellValue;
                                    break;
                                case CellType.Formula:
                                default:
                                    dataRow[j] = "=" + cell.CellFormula;
                                    break;
                            }
                        }
                    }
                    table.Rows.Add(dataRow);
                    //dataRow[j] = row.GetCell(j).ToString();   
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                table.Clear();
                table.Columns.Clear();
                table.Columns.Add("出錯了");
                DataRow dr = table.NewRow();
                dr[0] = ex.Message;
                table.Rows.Add(dr);
                return table;
            }
            finally
            {
                //sheet.Dispose();   
                workbook = null;
                sheet = null;
            }
            #region 清除最後的空行
            for (int i = table.Rows.Count - 1; i > 0; i--)
            {
                bool isnull = true;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Rows[i][j] != null)
                    {
                        if (table.Rows[i][j].ToString() != "")
                        {
                            isnull = false;
                            break;
                        }
                    }
                }
                if (isnull)
                {
                    table.Rows[i].Delete();
                }
            }
            #endregion
            return table;
        }

        /// Excel匯入成Datable
        /// </summary>
        /// <param name="file">匯入路徑(包含檔名與副檔名)</param>
        /// <returns></returns>
        public static DataTable ExcelToTable(string file)
        {
            DataTable dt = new DataTable();
            IWorkbook workbook;
            string fileExt = Path.GetExtension(file).ToLower();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                //XSSFWorkbook 適用XLSX格式，HSSFWorkbook 適用XLS格式
                if (fileExt == ".xlsx") { workbook = new XSSFWorkbook(fs); } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(fs); } else { workbook = null; }
                if (workbook == null) { return null; }
                ISheet sheet = workbook.GetSheetAt(0);

                //表頭  
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                List<int> columns = new List<int>();
                for (int i = 0; i < header.LastCellNum; i++)
                {
                    object obj = GetValueType(header.GetCell(i));
                    if (obj == null || obj.ToString() == string.Empty)
                    {
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                    }
                    else
                        dt.Columns.Add(new DataColumn(obj.ToString()));
                    columns.Add(i);
                }
                //資料  
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    bool hasValue = false;
                    foreach (int j in columns)
                    {
                        dr[j] = GetValueType(sheet.GetRow(i).GetCell(j));
                        if (dr[j] != null && dr[j].ToString() != string.Empty)
                        {
                            hasValue = true;
                        }
                    }
                    if (hasValue)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Datable匯出成Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file">匯出路徑(包括檔名與副檔名)</param>
        public static void TableToExcel(DataTable dt, string file)
        {
            IWorkbook workbook;
            string fileExt = Path.GetExtension(file).ToLower();
            if (fileExt == ".xlsx") { workbook = new XSSFWorkbook(); } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(); } else { workbook = null; }
            if (workbook == null) { return; }
            ISheet sheet = string.IsNullOrEmpty(dt.TableName) ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet(dt.TableName);

            //表頭  
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var colName = dt.Columns[i].ColumnName;
                ICell cell = row.CreateCell(i);
                switch (colName)
                {
                    case "ID":
                        colName = "編號";
                        break;
                    case "JobID":
                        colName = "工號";
                        break;
                    case "DepartmentTitle":
                        colName = "課別";
                        break;
                    case "Name":
                        colName = "姓名";
                        break;
                    case "EngName":
                        colName = "英文名";
                        break;
                    case "Email":
                        colName = "信箱";
                        break;
                    case "Ext":
                        colName = "分機";
                        break;
                    case "IsForeign":
                        colName = "外籍部門";
                        break;
                    case "IsSign":
                        colName = "簽到狀態";
                        break;
                    case "IsSent":
                        colName = "通知狀態";
                        break;
                    case "SignTime":
                        colName = "簽到時間";
                        break;
                    default:
                        continue;
                }
                cell.SetCellValue(colName);
            }

            //資料  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j >= (dt.Columns.Count-2))
                    {
                        continue;
                    }
                    string data = dt.Rows[i][j].ToString();
                    ICell cell = row1.CreateCell(j);
                    if (data == "True")
                        data = "是";
                    else if (data == "False")
                        data = "否";
                    cell.SetCellValue(data);
                }
            }

            //轉為位元組陣列  
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buf = stream.ToArray();

            //儲存為Excel檔案  
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }

        /// <summary>
        /// 獲取單元格型別
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueType(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:  
                    return null;
                case CellType.Boolean: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:  
                    return cell.NumericCellValue;
                case CellType.String: //STRING:  
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:  
                default:
                    return "=" + cell.CellFormula;
            }
        }
    }
}