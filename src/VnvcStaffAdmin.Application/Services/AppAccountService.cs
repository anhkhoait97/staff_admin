using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Linq.Expressions;
using VnvcStaffAdmin.Application.Helpers;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.AppAccounts;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Application.Services
{
    public class AppAccountService : IAppAccountService
    {
        private readonly ILogger<AppAccountService> _logger;
        private readonly IVnvcStaffUow _vnvcStaffUow;

        public AppAccountService(
            ILoggerFactory loggerFactory,
            IVnvcStaffUow vnvcStaffUow)
        {
            _logger = loggerFactory.CreateLogger<AppAccountService>();
            _vnvcStaffUow = vnvcStaffUow;
        }

        public async Task<ResponseModel> GetById(string id)
        {
            try
            {
                var result = await _vnvcStaffUow.GetRepository<AppAccount>().SingleAsync(x => x.Id == id);

                return result != null ? ResponseModel.Successed("Thành công", result) : ResponseModel.Failed("Tài khoản không tồn tại");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Failed(ex.Message);
            }
        }

        public async Task<DatasourceResult<AppAccount>> GetLists(QueryGetListAppAccountDto query)
        {
            var result = new DatasourceResult<AppAccount>
            {
                From = query.From,
                Size = query.Size
            };

            try
            {
                Expression<Func<AppAccount, bool>> predicate = x => true;

                if (!string.IsNullOrEmpty(query.SearchText))
                {
                    predicate = x => x.FullName.Contains(query.SearchText.Trim()) || x.Phone.Contains(query.SearchText.Trim()) || x.Email.Contains(query.SearchText.Trim());
                }

                var appAccounts = await _vnvcStaffUow.GetRepository<AppAccount>().FindPagingAsync(predicate, query.From, query.Size, Builders<AppAccount>.Sort.Descending(f => f.CreatedAt));
                result.Data = appAccounts.ToList();
                result.Total = await _vnvcStaffUow.GetRepository<AppAccount>().CountAsync(predicate);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<ResponseModel> Update(UpdateAppAccountDto dto)
        {
            try
            {
                var account = await _vnvcStaffUow.GetRepository<AppAccount>().SingleAsync(x => x.Id == dto.Id);

                if (account == null) return ResponseModel.Failed("Tài khoản không tồn tại");
                
                var filter = Builders<AppAccount>.Filter.Eq("Id", dto.Id);

                var updateBuilder = Builders<AppAccount>.Update;

                var updateDefinition = updateBuilder.Combine(
                    dto.Columns.Select(column => column switch
                    {
                        "FullName" => updateBuilder.Set(s => s.FullName, dto.FullName),
                        "Email" => updateBuilder.Set(s => s.Email, dto.Email),
                        "Province" => updateBuilder.Set(s => s.Province, dto.Province),
                        "District" => updateBuilder.Set(s => s.District, dto.District),
                        "Ward" => updateBuilder.Set(s => s.Ward, dto.Ward),
                        "Address" => updateBuilder.Set(s => s.Address, dto.Address),
                        "AvatarUrl" => updateBuilder.Set(s => s.AvatarUrl, dto.AvatarUrl),
                        "Birthday" => updateBuilder.Set(s => s.Birthday, dto.Birthday),
                        "Gender" => updateBuilder.Set(s => s.Gender, dto.Gender),
                        "IsActive" => updateBuilder.Set(s => s.IsActive, dto.IsActive),
                        "Center" => updateBuilder.Set(s => s.Center, dto.Center),
                        "IdentityNumber" => updateBuilder.Set(s => s.IdentityNumber, dto.IdentityNumber),
                        _ => null
                    }).Where(update => update != null));

                var options = new FindOneAndUpdateOptions<AppAccount>
                {
                    ReturnDocument = ReturnDocument.After
                };

                var data = await _vnvcStaffUow.GetRepository<AppAccount>().UpdateAsync(filter, updateDefinition, options);

                return ResponseModel.Successed("Cập nhật thành công", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ResponseModel.Failed(ex.Message);
            }
        }

        public async Task<string> Export(QueryGetListAppAccountDto query)
        {
            Expression<Func<AppAccount, bool>> predicate = x => true;

            if (!string.IsNullOrEmpty(query.SearchText))
            {
                predicate = x => x.FullName.Contains(query.SearchText.Trim()) || x.Phone.Contains(query.SearchText.Trim()) || x.Email.Contains(query.SearchText.Trim());
            }

            var appAccounts = await _vnvcStaffUow.GetRepository<AppAccount>().FindAsync(predicate);

            string excelName = $"app_account_export__{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Export", excelName);

            FileInfo newFile = new FileInfo(path);
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Data");
            workSheet.TabColor = Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Column(1).Width = 400;

            var i = 1;

            workSheet.Column(i).Width = 10;
            workSheet.Cells[1, i++].Value = "STT";
            workSheet.Column(i).Width = 25;
            workSheet.Cells[1, i++].Value = "Họ tên";
            workSheet.Column(i).Width = 15;
            workSheet.Cells[1, i++].Value = "Số điện thoại";
            workSheet.Column(i).Width = 30;
            workSheet.Cells[1, i++].Value = "Email";
            workSheet.Column(i).Width = 25;
            workSheet.Cells[1, i++].Value = "Tỉnh thành";
            workSheet.Column(i).Width = 25;
            workSheet.Cells[1, i++].Value = "Đăng nhập gần nhất";
            workSheet.Column(i).Width = 15;
            workSheet.Cells[1, i++].Value = "Trạng thái";
            workSheet.Column(i).Width = 10;
            workSheet.Cells[1, i++].Value = "Giới tính";
            workSheet.Column(i).Width = 15;
            workSheet.Cells[1, i++].Value = "Ngày sinh";
            workSheet.Column(i).Width = 25;
            workSheet.Cells[1, i++].Value = "Ngày tạo";
            workSheet.Column(i).Width = 20;
            workSheet.Cells[1, i++].Value = "Người tạo";
            workSheet.Column(i).Width = 25;
            workSheet.Cells[1, i++].Value = "Ngày cập nhật";
            workSheet.Column(i).Width = 20;
            workSheet.Cells[1, i++].Value = "Người cập nhật";

            var row = 2;
            foreach (var item in appAccounts)
            {
                var gender = "";
                switch (item.Gender)
                {
                    case "male":
                        gender = "Nam";
                        break;
                    case "female":
                        gender = "Nữ";
                        break;
                }
                i = 1;
                workSheet.Cells[row, i++].Value = row - 1;
                workSheet.Cells[row, i++].Value = item.FullName;
                workSheet.Cells[row, i++].Value = StringHelper.FormatPhoneNumber(item.Phone);
                workSheet.Cells[row, i++].Value = !string.IsNullOrEmpty(item.Email) ? item.Email : string.Empty;
                workSheet.Cells[row, i++].Value = item.Province;
                workSheet.Cells[row, i++].Value = item.LoginLog.OrderByDescending(x => x.At).FirstOrDefault()?.At.AddHours(7).ToString("dd/MM/yyyy HH:mm:ss");
                workSheet.Cells[row, i++].Value = item.IsActive ? "Hoạt động" : "Ngưng hoạt động";
                workSheet.Cells[row, i++].Value = gender;
                workSheet.Cells[row, i++].Value = item.Birthday?.AddHours(7).ToString("dd/MM/yyyy");
                workSheet.Cells[row, i++].Value = item.CreatedAt.AddHours(7).ToString("dd/MM/yyyy HH:mm:ss");
                workSheet.Cells[row, i++].Value = item.CreatedBy;
                workSheet.Cells[row, i++].Value = item.UpdatedAt?.AddHours(7).ToString("dd/MM/yyyy HH:mm:ss") ?? "";
                workSheet.Cells[row, i++].Value = item.UpdatedBy;
                row++;
            }
            //color header
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#1e88e5");
            workSheet.Cells[1, 1, 1, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 1, 1, 13].Style.Fill.BackgroundColor.SetColor(colFromHex);
            workSheet.Cells[1, 1, 1, 13].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[1, 1, 1, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 1, 1, 13].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //border
            var modelTable = workSheet.Cells[1, 1, --row, 13];
            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            excel.SaveAs(newFile);
            return excelName;
        }
    }
}
