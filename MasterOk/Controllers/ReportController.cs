using Microsoft.AspNetCore.Mvc;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MasterOk.Controllers
{
    public class ReportController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _webHost;

        public ReportController(DataBaseContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FirstLastNameClient");
            return View();
        }

        [HttpPost]
        public async Task<VirtualFileResult> Index(int id, DateTime startDate, DateTime endDate)
        {
            //Имя файла для сохранения
            string fileName = "Orders_Clients_" + DateTime.Now.ToShortDateString() + ".xlsx";
            //Новая книга эксель
            var workBook = new XLWorkbook();
            //Рабочая страница
            var wortSheet = workBook.Worksheets.Add("Заказ по клиенту");
            //Вертикальная группировка
            wortSheet.Outline.SummaryVLocation = XLOutlineSummaryVLocation.Bottom;

            Client client = await _context.Clients.FindAsync(1);
            //Указатель строки в эксель
            int currentRow = 1;

            wortSheet.Cell(currentRow, 1).Value = "Отчет";
            wortSheet.Range(currentRow, 1, currentRow, 5).Merge();
            wortSheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            currentRow++;

            wortSheet.Cell(currentRow, 1).Value = "по заказам клиента";
            wortSheet.Range(currentRow, 1, currentRow, 5).Merge();
            wortSheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            currentRow++;

            wortSheet.Cell(currentRow, 1).Value = "c "+startDate.ToShortDateString()+" по " +endDate.ToShortDateString();
            wortSheet.Range(currentRow, 1, currentRow, 5).Merge();
            wortSheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            currentRow++;

            if (client != null)
            {
                wortSheet.Cell(currentRow, 1).Value = "Клиент:";
                wortSheet.Cell(currentRow, 2).Value = client.FirstLastNameClient;
                currentRow++;

                var productChecks = await _context.ProductChecks.
                    Include(p => p.PayMethod).
                    Include(p => p.DeliveryMethod).
                    Include(p => p.StateOrder).
                    Include(p => p.ProductSolds).
                    ThenInclude(p => p.Product).
                    Where(i => i.ClientId == client.Id).
                    Where(p => startDate <= p.DateTimeSale && p.DateTimeSale <= endDate).
                    ToListAsync();

                wortSheet.Cell(currentRow, 1).Value = "Номер заказа";
                wortSheet.Cell(currentRow, 2).Value = "Дата заказа";
                wortSheet.Cell(currentRow, 3).Value = "Способ оплаты";
                wortSheet.Cell(currentRow, 4).Value = "Способ доставки";
                wortSheet.Cell(currentRow, 5).Value = "Статус заказа";

                foreach (var item in productChecks)
                {
                    currentRow++;
                    wortSheet.Cell(currentRow, 1).Value = item.Id;
                    wortSheet.Cell(currentRow, 2).Value = item.DateTimeSale.ToString();
                    wortSheet.Cell(currentRow, 3).Value = item.PayMethod.TitlePayMethod;
                    wortSheet.Cell(currentRow, 4).Value = item.DeliveryMethod.TitleDeliveryMethod;
                    wortSheet.Cell(currentRow, 5).Value = item.StateOrder.TitleState;

                    currentRow++;
                    int first = currentRow;

                    wortSheet.Cell(currentRow, 1).Value = "Код товара";
                    wortSheet.Cell(currentRow, 2).Value = "Название товара";
                    wortSheet.Cell(currentRow, 3).Value = "Стоимость";
                    wortSheet.Cell(currentRow, 4).Value = "Количество";
                    wortSheet.Cell(currentRow, 5).Value = "Общая стоимость";

                    foreach (var items in item.ProductSolds)
                    {
                        currentRow++;
                        wortSheet.Cell(currentRow, 1).Value = items.Product.Id;
                        wortSheet.Cell(currentRow, 2).Value = items.Product.TitleProduct;
                        wortSheet.Cell(currentRow, 3).Value = items.PriceSold;
                        wortSheet.Cell(currentRow, 4).Value = items.CountSold;
                        wortSheet.Cell(currentRow, 5).Value = items.TotalSold;
                    }
                    int last = currentRow;

                    wortSheet.Rows(first, last).Group();
                }
                currentRow++;

                wortSheet.Cell(currentRow, 4).Value = "Итого:";
                wortSheet.Cell(currentRow, 5).Value = productChecks.Sum(s => s.ProductSolds.Sum(s => s.TotalSold));

                wortSheet.Rows(1, currentRow).AdjustToContents();
                wortSheet.Columns(1, 10).AdjustToContents();
            }

            string directory = _webHost.WebRootPath + "/Report/";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string files = directory + fileName;

            using (var fileStream = new FileStream(files, FileMode.Create))
            {
                workBook.SaveAs(fileStream);
                fileStream.Flush();
            }

            return File(Path.Combine("~", "/Report/", fileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}