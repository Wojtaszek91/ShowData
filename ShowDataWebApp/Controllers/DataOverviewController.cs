using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShowDataWebApp.Models;
using ShowDataWebApp.Repository.IRepository;

namespace ShowDataWebApp.Controllers
{
    public class DataOverviewController : Controller
    {
        public readonly IDataOverviewRepository _dataRepo;

        public DataOverviewController(IDataOverviewRepository dataRepo)
        {
            _dataRepo = dataRepo;
        }

        public IActionResult Index()
        {
            return View(new DataOverview { });
        }

        public async Task<IActionResult> GetDataOverviews()
        {
            return Json(new { data = await _dataRepo.GetAllAsync(StaticUrlBase.DataOverviewApiUrl) });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDataOverview(int id)
        {
            var state = await _dataRepo.DeleteAsync(StaticUrlBase.DataOverviewApiUrl, id);
            if (state)
                return Json(new { success = true, message = "Object has been deleted." });
            else return Json(new { success = false, message = "Object has been NOT deleted" });
        }

        public async Task<IActionResult> UpsertDataOverview(int? id)
        {
            DataOverview obj = new DataOverview();
            if (id == null)
            {
                return View(obj);
            }

            obj = await _dataRepo.GetAsync(StaticUrlBase.DataOverviewApiUrl, id.GetValueOrDefault());

            if (obj == null)
                return NotFound();

            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertDataOverview(DataOverview dataOverview)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    dataOverview.Image = p1;
                } else
                {
                    var dbDataOverview = await _dataRepo.GetAsync(StaticUrlBase.DataOverviewApiUrl, dataOverview.Id);
                    dataOverview.Image = dbDataOverview.Image;
                }
                bool respon;
                if (dataOverview.Id == 0)
                {
                    respon = await _dataRepo.CreateAsync(StaticUrlBase.DataOverviewApiUrl, dataOverview);
                } else
                {
                    respon = await _dataRepo.UpdateAsync(StaticUrlBase.DataOverviewApiUrl + dataOverview.Id, dataOverview);
                }
                if (respon) {
                    return RedirectToAction(nameof(Index));
                }  else return View(dataOverview);
            } else
                return View(dataOverview);
        }
    }
}
