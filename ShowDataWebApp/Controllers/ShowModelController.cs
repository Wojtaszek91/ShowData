using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShowDataWebApp.Models;
using ShowDataWebApp.Models.ViewModels;
using ShowDataWebApp.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShowDataWebApp.Repository;

namespace ShowDataWebApp.Controllers
{
    public class ShowModelController : Controller
    {
        public readonly IDataOverviewRepository _dataRepo;
        public readonly IShowModelRepository _showModelRepo;

        public ShowModelController(IDataOverviewRepository dataRepo, IShowModelRepository showModelRepo)
        {
            _dataRepo = dataRepo;
            _showModelRepo = showModelRepo;
        }

        public IActionResult Index()
        {
            return View(new ShowModel { });
        }

        public async Task<IActionResult> GetShowModels()
        {
            return Json(new { data = await _showModelRepo.GetAllAsync(StaticUrlBase.ShowModelApiUrl) });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteShowModel(int id)
        {
            var state = await _showModelRepo.DeleteAsync(StaticUrlBase.ShowModelApiUrl, id);
            if (state)
                return Json(new { success = true, message = "Object has been deleted." });
            else return Json(new { success = false, message = "Object has been NOT deleted" });
        }

        public async Task<IActionResult> UpsertShowModel(int? id)
        {
            IEnumerable<DataOverview> DOlist = await _dataRepo.GetAllAsync(StaticUrlBase.DataOverviewApiUrl);

            ShowModelVM showVM = new ShowModelVM()
            {
                DataOverviewList = DOlist.Select(e => new SelectListItem
                {
                    Text = e.Title,
                    Value = e.Id.ToString()
                })
            };
            showVM.ShowModel = new ShowModel();


            if (id == null)
            {
                return View(showVM);
            }

            showVM.ShowModel = await _showModelRepo.GetAsync(StaticUrlBase.ShowModelApiUrl, id.GetValueOrDefault());

            if (showVM == null)
                return NotFound();

            return View(showVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertShowModel(ShowModelVM showModelVM)
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
                    showModelVM.ShowModel.Image = p1;
                }
                else
                {
                    var dbShowModel = await _showModelRepo.GetAsync(StaticUrlBase.DataOverviewApiUrl, showModelVM.ShowModel.Id);
                    showModelVM.ShowModel.Image = dbShowModel.Image;
                }
                bool respon;
                if (showModelVM.ShowModel.Id == 0)
                {
                    respon = await _showModelRepo.CreateAsync(StaticUrlBase.ShowModelApiUrl, showModelVM.ShowModel);
                } else
                {
                    respon = await _showModelRepo.UpdateAsync(StaticUrlBase.ShowModelApiUrl + showModelVM.ShowModel.Id, showModelVM.ShowModel);
                }
                if (respon) {
                    return RedirectToAction(nameof(Index));
                }  else return View(showModelVM);
            }
            else
            {
                IEnumerable<DataOverview> DOlist = await _dataRepo.GetAllAsync(StaticUrlBase.DataOverviewApiUrl);

                showModelVM.DataOverviewList = DOlist.Select(e => new SelectListItem
                {
                    Text = e.Title,
                    Value = e.Id.ToString()
                });
                return View(showModelVM);
            }
        }
    }
}
