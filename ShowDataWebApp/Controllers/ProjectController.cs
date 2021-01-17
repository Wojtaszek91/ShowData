using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShowDataWebApp.Models;
using ShowDataWebApp.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShowDataWebApp.Models.ViewModels;

namespace ShowDataWebApp.Controllers
{
    public class ProjectController : Controller
    {
        public readonly IProjectRepository _projectRepo;

        public ProjectController(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public async Task<IActionResult> Index()
        {
            var projectsList = await _projectRepo.GetAllAsync(StaticUrlBase.ProjectApiUrl,
                HttpContext.Session.GetString("ShowDataToken"));
            ProjectsVM projectsVM = new ProjectsVM()
            {
                ProjectsList = projectsList
            };

                return View(projectsVM);


        }

        public async Task<IActionResult> GetProjects()
        {
            
            return Json(new { data = await _projectRepo.GetAllAsync(StaticUrlBase.ProjectApiUrl,
                HttpContext.Session.GetString("ShowDataToken")) });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var state = await _projectRepo.DeleteAsync(StaticUrlBase.ProjectApiUrl, id,
                HttpContext.Session.GetString("ShowDataToken"));
            if (state)
                return Json(new { success = true, message = "Object has been deleted." });
            else return Json(new { success = false, message = "Object has been NOT deleted" });
        }

        public async Task<IActionResult> UpsertProject(int? id)
        {
            Project obj = new Project();
            if (id == null)
            {
                return View(obj);
            }

            obj = await _projectRepo.GetAsync(StaticUrlBase.ProjectApiUrl,
                id.GetValueOrDefault(),
                HttpContext.Session.GetString("ShowDataToken"));

            if (obj == null)
                return NotFound();

            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertProject(Project project)
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
                    project.Image = p1;
                }
                else if (project.Id != 0)
                {
                    var dbDataOverview = await _projectRepo.GetAsync(StaticUrlBase.ProjectApiUrl,
                        project.Id,
                        HttpContext.Session.GetString("ShowDataToken"));
                    project.Image = dbDataOverview.Image;
                }
                bool respon;
                if (project.Id == 0)
                {
                    respon = await _projectRepo.CreateAsync(StaticUrlBase.ProjectApiUrl,
                        project,
                        HttpContext.Session.GetString("ShowDataToken"));
                }
                else
                {
                    respon = await _projectRepo.UpdateAsync(StaticUrlBase.ProjectApiUrl + project.Id,
                        project,
                        HttpContext.Session.GetString("ShowDataToken"));
                }
                if (respon)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(project);
                }
            }
            else
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
                // model state invalid
                return View(project);
            }
        }
    }
}
