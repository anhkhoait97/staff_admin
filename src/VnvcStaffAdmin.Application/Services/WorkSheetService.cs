using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.WorkSheets;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Application.Services
{
    public class WorkSheetService : IWorkSheetService
    {
        private readonly ILogger<WorkSheetService> _logger;
        private readonly IVnvcStaffUow _vnvcStaffUow;

        public WorkSheetService(
            ILoggerFactory loggerFactory,
            IVnvcStaffUow vnvcStaffUow)
        {
            _logger = loggerFactory.CreateLogger<WorkSheetService>();
            _vnvcStaffUow = vnvcStaffUow;
        }

        public async Task<ResponseModel<WorkSheetHistoryDto>> GetDetailWorkSheetOfUser(QueryGetDetailWorkSheetHistoryDto query)
        {
            var result = new ResponseModel<WorkSheetHistoryDto>();
            try
            {
                var user = await _vnvcStaffUow.GetRepository<AppAccount>().SingleAsync(x => x.Id == query.UserId);
                if (user != null)
                {
                    var userWorkSheet = new WorkSheetHistoryDto
                    {
                        UserId = user.Id,
                        FullName = user.FullName
                    };

                    var firstDayOfMonth = new DateTime(query.Year, query.Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddTicks(-1);

                    var workSheets = await _vnvcStaffUow.GetRepository<WorkSheet>().FindAsync(x => x.UserId == user.Id && firstDayOfMonth <= x.DayShiftDate && x.DayShiftDate <= lastDayOfMonth);
                    if (workSheets.Any())
                    {
                        userWorkSheet.WorkSheets = workSheets.Select(x => new WorkSheetOfUserDto
                        {
                            Id = x.Id,
                            DayShift = x.DayShift,
                            CheckInHour = x.CheckInHour,
                            CheckInMinutes = x.CheckInMinutes,
                            CheckOutHour = x.CheckOutHour,
                            CheckOutMinutes = x.CheckOutMinutes,
                            NumberOfHoursWorked = x.TotalWorkingHour >= 8 ? "1" : "0.5"
                        }).ToList();
                    }

                    result.Data = userWorkSheet;
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<DatasourceResult<WorkSheetHistoryDto>> GetWorkSheets(QueryGetListWorkSheetDto query)
        {
            var result = new DatasourceResult<WorkSheetHistoryDto>
            {
                From = query.From,
                Size = query.Size
            };
            try
            {
                var users = await _vnvcStaffUow.GetRepository<AppAccount>().FindPagingAsync(x => x.IsActive == true, query.From, query.Size, Builders<AppAccount>.Sort.Descending(f => f.CreatedAt));
                
                result.Total = await _vnvcStaffUow.GetRepository<AppAccount>().CountAsync(x => x.IsActive == true);

                var userWorkSheets = new List<WorkSheetHistoryDto>();

                if (users.Any())
                {
                    var firstDayOfMonth = new DateTime(query.Year, query.Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddTicks(-1);

                    for (int i = 0; i < users.Count(); i++)
                    {
                        var user = users.ElementAt(i);

                        var workSheets = await _vnvcStaffUow.GetRepository<WorkSheet>().FindAsync(x => x.UserId == user.Id && firstDayOfMonth <= x.DayShiftDate && x.DayShiftDate <= lastDayOfMonth);

                        userWorkSheets.Add(new WorkSheetHistoryDto
                        {
                            UserId = user.Id,
                            FullName = user.FullName,
                            WorkSheets = workSheets.Select(x => new WorkSheetOfUserDto
                            {
                                Id = x.Id,
                                DayShift = x.DayShift,
                                CheckInHour = x.CheckInHour,
                                CheckInMinutes = x.CheckInMinutes,
                                CheckOutHour = x.CheckOutHour,
                                CheckOutMinutes = x.CheckOutMinutes,
                                NumberOfHoursWorked = x.TotalWorkingHour >= 8 ? "1" : "0.5"
                            }).ToList()
                        });
                    }

                    result.Data = userWorkSheets;
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<string> ExportWorkSheets(QueryGetListWorkSheetDto query)
        {
            string excelName = $"danh_sach_cham_cong_thang_{query.Month}_nam_{query.Year}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Export", excelName);

            FileInfo newFile = new FileInfo(path);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Data");
            workSheet.TabColor = Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Column(1).Width = 400;

            //Build Header
            int headerIndex = 1;

            workSheet.Column(headerIndex).Width = 10;
            workSheet.Cells[1, headerIndex++].Value = "STT";
            workSheet.Column(headerIndex).Width = 20;
            workSheet.Cells[1, headerIndex++].Value = "Mã";
            workSheet.Column(headerIndex).Width = 50;
            workSheet.Cells[1, headerIndex++].Value = "Họ tên";
            workSheet.Column(headerIndex).Width = 40;
            workSheet.Cells[1, headerIndex++].Value = "Vị trí";
            workSheet.Column(headerIndex).Width = 40;
            workSheet.Cells[1, headerIndex++].Value = "Phòng ban";
            workSheet.Column(headerIndex).Width = 10;
            workSheet.Cells[1, headerIndex].Value = "Ngày";

            var firstDayOfMonth = new DateTime(query.Year, query.Month, 1);
            workSheet.Cells[2, headerIndex].Value = GetDate(firstDayOfMonth);
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            int daysInMonth = DateTime.DaysInMonth(query.Year, query.Month);
            for (int dayIndex = 1; dayIndex < daysInMonth; dayIndex++)
            {
                DateTime currentDate = firstDayOfMonth.AddDays(dayIndex);

                workSheet.Cells[2, headerIndex].Value = GetDate(currentDate);
                workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

                headerIndex++;
            }

            workSheet.Cells["A1:A2"].Merge = true;
            workSheet.Cells["B1:B2"].Merge = true;
            workSheet.Cells["C1:C2"].Merge = true;
            workSheet.Cells["D1:D2"].Merge = true;
            workSheet.Cells["E1:E2"].Merge = true;
            workSheet.Cells[1, 6, 1, headerIndex - 1].Merge = true;

            int indexDiMuon = headerIndex;
            workSheet.Column(headerIndex).Width = 10;
            workSheet.Cells[1, headerIndex].Value = "Đi muộn";

            workSheet.Cells[2, headerIndex].Value = "Số phút";

            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Tiền phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Công phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            workSheet.Cells[1, indexDiMuon, 1, headerIndex].Merge = true;

            headerIndex++;

            int indexVeSom = headerIndex;
            workSheet.Column(headerIndex).Width = 10;
            workSheet.Cells[1, headerIndex].Value = "Về sớm";

            workSheet.Cells[2, headerIndex].Value = "Số phút";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Tiền phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Công phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            workSheet.Cells[1, indexVeSom, 1, headerIndex].Merge = true;

            headerIndex++;

            int indexQuenCheckInOut = headerIndex;
            workSheet.Column(headerIndex).Width = 10;
            workSheet.Cells[1, headerIndex].Value = "Quên checkin/checkout";

            workSheet.Cells[2, headerIndex].Value = "Số phút";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Tiền phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Công phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            workSheet.Cells[1, indexQuenCheckInOut, 1, headerIndex].Merge = true;

            //add style
            Color colFromHex = ColorTranslator.FromHtml("#1e88e5");
            workSheet.Cells[1, 1, 1, headerIndex - 1].Style.Font.Bold = true;
            workSheet.Cells[1, 1, 1, headerIndex - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 1, 1, headerIndex - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //
            var row = 3;

            var users = await _vnvcStaffUow.GetRepository<AppAccount>().FindAsync(x => x.IsActive == true);

            if (users.Any())
            {
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddTicks(-1);

                for (int i = 0; i < users.Count(); i++)
                {
                    var user = users.ElementAt(i);

                    if (user == null) continue;

                    var workSheetOfUsers = await _vnvcStaffUow.GetRepository<WorkSheet>().FindAsync(x => x.UserId == user.Id && firstDayOfMonth <= x.DayShiftDate && x.DayShiftDate <= lastDayOfMonth);

                    int dataIndex = 1;

                    workSheet.Cells[row, dataIndex++].Value = row - 2;
                    workSheet.Cells[row, dataIndex++].Value = user.FullName;
                    workSheet.Cells[row, dataIndex++].Value = "";
                    workSheet.Cells[row, dataIndex++].Value = "";
                    workSheet.Cells[row, dataIndex++].Value = "";

                    for (int dayIndex = 0; dayIndex < daysInMonth; dayIndex++)
                    {
                        DateTime currentDate = firstDayOfMonth.AddDays(dayIndex);

                        var workSheetOfUser = workSheetOfUsers.Where(x => x.DayShift == GetDate(currentDate, false)).FirstOrDefault();

                        string numberOfHoursWorked = currentDate.DayOfWeek == DayOfWeek.Sunday ? "N" : "-";

                        if (workSheetOfUser != null)
                        {
                            numberOfHoursWorked = workSheetOfUser.TotalWorkingHour >= 8 ? "1" : "0.5";
                        }

                        workSheet.Cells[row, dataIndex].Value = numberOfHoursWorked;

                        workSheet.Cells[row, dataIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[row, dataIndex].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        dataIndex++;
                    }

                    workSheet.Cells[row, dataIndex++].Value = 0;
                    workSheet.Cells[row, dataIndex++].Value = 0;
                    workSheet.Cells[row, dataIndex++].Value = 0;
                    workSheet.Cells[row, dataIndex++].Value = 0;
                    workSheet.Cells[row, dataIndex++].Value = 0;
                    workSheet.Cells[row, dataIndex++].Value = 0;
                    workSheet.Cells[row, dataIndex++].Value = 0;
                    workSheet.Cells[row, dataIndex++].Value = 0;
                    workSheet.Cells[row, dataIndex++].Value = 0;

                    row++;
                }
            }

            //border
            var modelTable = workSheet.Cells[1, 1, --row, headerIndex];
            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            excel.SaveAs(newFile);
            return excelName;
        }

        public async Task<string> ExportDetailWorkSheetOfUser(QueryGetDetailWorkSheetHistoryDto query)
        {
            var user = await _vnvcStaffUow.GetRepository<AppAccount>().SingleAsync(x => x.Id == query.UserId);

            string excelName = $"danh_sach_cham_cong_{user.FullName}_thang_{query.Month}_nam_{query.Year}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Export", excelName);

            FileInfo newFile = new FileInfo(path);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Data");
            workSheet.TabColor = Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Column(1).Width = 400;

            //Build Header
            int headerIndex = 1;

            workSheet.Column(headerIndex).Width = 10;
            workSheet.Cells[1, headerIndex++].Value = "STT";
            workSheet.Column(headerIndex).Width = 20;
            workSheet.Cells[1, headerIndex++].Value = "Mã";
            workSheet.Column(headerIndex).Width = 50;
            workSheet.Cells[1, headerIndex++].Value = "Họ tên";
            workSheet.Column(headerIndex).Width = 40;
            workSheet.Cells[1, headerIndex++].Value = "Vị trí";
            workSheet.Column(headerIndex).Width = 40;
            workSheet.Cells[1, headerIndex++].Value = "Phòng ban";
            workSheet.Column(headerIndex).Width = 10;
            workSheet.Cells[1, headerIndex].Value = "Ngày";

            var firstDayOfMonth = new DateTime(query.Year, query.Month, 1);
            workSheet.Cells[2, headerIndex].Value = GetDate(firstDayOfMonth);
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            int daysInMonth = DateTime.DaysInMonth(query.Year, query.Month);
            for (int dayIndex = 1; dayIndex < daysInMonth; dayIndex++)
            {
                DateTime currentDate = firstDayOfMonth.AddDays(dayIndex);

                workSheet.Cells[2, headerIndex].Value = GetDate(currentDate);
                workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

                headerIndex++;
            }

            workSheet.Cells["A1:A2"].Merge = true;
            workSheet.Cells["B1:B2"].Merge = true;
            workSheet.Cells["C1:C2"].Merge = true;
            workSheet.Cells["D1:D2"].Merge = true;
            workSheet.Cells["E1:E2"].Merge = true;
            workSheet.Cells[1, 6, 1, headerIndex - 1].Merge = true;

            int indexDiMuon = headerIndex;
            workSheet.Column(headerIndex).Width = 10;
            workSheet.Cells[1, headerIndex].Value = "Đi muộn";

            workSheet.Cells[2, headerIndex].Value = "Số phút";

            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Tiền phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Công phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            workSheet.Cells[1, indexDiMuon, 1, headerIndex].Merge = true;

            headerIndex++;

            int indexVeSom = headerIndex;
            workSheet.Column(headerIndex).Width = 10;
            workSheet.Cells[1, headerIndex].Value = "Về sớm";

            workSheet.Cells[2, headerIndex].Value = "Số phút";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Tiền phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Công phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            workSheet.Cells[1, indexVeSom, 1, headerIndex].Merge = true;

            headerIndex++;

            int indexQuenCheckInOut = headerIndex;
            workSheet.Column(headerIndex).Width = 10;
            workSheet.Cells[1, headerIndex].Value = "Quên checkin/checkout";

            workSheet.Cells[2, headerIndex].Value = "Số phút";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Tiền phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            headerIndex++;

            workSheet.Cells[2, headerIndex].Value = "Công phạt";
            workSheet.Cells[2, headerIndex].Style.Font.Bold = true;

            workSheet.Cells[1, indexQuenCheckInOut, 1, headerIndex].Merge = true;

            //add style
            Color colFromHex = ColorTranslator.FromHtml("#1e88e5");
            workSheet.Cells[1, 1, 1, headerIndex - 1].Style.Font.Bold = true;
            workSheet.Cells[1, 1, 1, headerIndex - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 1, 1, headerIndex - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //
            var row = 3;
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddTicks(-1);

            var workSheetOfUsers = await _vnvcStaffUow.GetRepository<WorkSheet>().FindAsync(x => x.UserId == user.Id && firstDayOfMonth <= x.DayShiftDate && x.DayShiftDate <= lastDayOfMonth);

            int dataIndex = 1;

            workSheet.Cells[row, dataIndex++].Value = row - 2;
            workSheet.Cells[row, dataIndex++].Value = user.FullName;
            workSheet.Cells[row, dataIndex++].Value = "";
            workSheet.Cells[row, dataIndex++].Value = "";
            workSheet.Cells[row, dataIndex++].Value = "";

            for (int dayIndex = 0; dayIndex < daysInMonth; dayIndex++)
            {
                DateTime currentDate = firstDayOfMonth.AddDays(dayIndex);

                var workSheetOfUser = workSheetOfUsers.Where(x => x.DayShift == GetDate(currentDate, false)).FirstOrDefault();

                string numberOfHoursWorked = currentDate.DayOfWeek == DayOfWeek.Sunday ? "N" : "-";

                if (workSheetOfUser != null)
                {
                    numberOfHoursWorked = workSheetOfUser.TotalWorkingHour >= 8 ? "1" : "0.5";
                }

                workSheet.Cells[row, dataIndex].Value = numberOfHoursWorked;

                workSheet.Cells[row, dataIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[row, dataIndex].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                dataIndex++;
            }

            workSheet.Cells[row, dataIndex++].Value = 0;
            workSheet.Cells[row, dataIndex++].Value = 0;
            workSheet.Cells[row, dataIndex++].Value = 0;
            workSheet.Cells[row, dataIndex++].Value = 0;
            workSheet.Cells[row, dataIndex++].Value = 0;
            workSheet.Cells[row, dataIndex++].Value = 0;
            workSheet.Cells[row, dataIndex++].Value = 0;
            workSheet.Cells[row, dataIndex++].Value = 0;
            workSheet.Cells[row, dataIndex++].Value = 0;

            row++;

            //border
            var modelTable = workSheet.Cells[1, 1, --row, headerIndex];
            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            excel.SaveAs(newFile);
            return excelName;
        }

        private string GetDate(DateTime date, bool withoutYear = true)
        {
            string day = date.Day.ToString().Length == 1 ? $"0{date.Day}" : date.Day.ToString();
            string month = date.Month.ToString().Length == 1 ? $"0{date.Month}" : date.Day.ToString();

            return withoutYear ? $"{day}/{month}" : $"{day}/{month}/{date.Year}";
        }
    }
}
