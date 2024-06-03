using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;

namespace TiemKietAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public IActionResult Test(IFormFile uploadFile)
        {
            Console.WriteLine($"FileName Start: {uploadFile.FileName}");

            ConvertToIFormFile(uploadFile);
            return Ok();
        }

        private IFormFile ConvertToIFormFile(IFormFile file)
        {
            // Tạo một tên tập tin tạm thời cho tệp .xls
            var tempFilePath = Path.GetTempFileName();

            Console.WriteLine($"Path: {tempFilePath}");
            // Lưu tệp IFormFile vào đĩa
            using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            // Mở tệp .xls và chuyển đổi sang .xlsx
            var app = new Microsoft.Office.Interop.Excel.Application();
            var wb = app.Workbooks.Open(tempFilePath);
            var newFilePath = Path.ChangeExtension(tempFilePath, ".xlsx");
            wb.SaveAs(newFilePath, XlFileFormat.xlOpenXMLWorkbook);
            wb.Close();
            app.Quit();

            // Đọc nội dung của tệp .xlsx mới chuyển đổi
            byte[] fileBytes = System.IO.File.ReadAllBytes(newFilePath);

            // Xóa tệp .xls tạm thời
            System.IO.File.Delete(tempFilePath);
            System.IO.File.Delete(newFilePath);

            // Tạo một thể hiện mới của IFormFile từ dữ liệu đã chuyển đổi
            var convertedFile = new FormFile(new MemoryStream(fileBytes), 0, file.Length, file.Name, file.FileName);
            Console.WriteLine($"FileName END: {convertedFile.FileName}");

            // Trả về tệp IFormFile mới
            return convertedFile;
        }
    }
}
