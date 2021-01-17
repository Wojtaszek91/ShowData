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
using Microsoft.AspNetCore.Http;

namespace ShowDataWebApp.Controllers
{
    public class taskController : Controller
    {
        public readonly IProjectRepository _projectRepo;
        public readonly ItaskRepository _taskRepo;

        public taskController(IProjectRepository projectRepo, ItaskRepository taskRepo)
        {
            _projectRepo = projectRepo;
            _taskRepo = taskRepo;
        }

        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return View(new IndexTaskViewModel { PojectId = 0 });
            }
            else
            {
                return View(new IndexTaskViewModel { PojectId = (int)id });
            }
        }

        public async Task<IActionResult> TasksView(int id)
        {
            var token = HttpContext.Session.GetString("ShowDataToken");
            var tasks = await _taskRepo.GetAllProjectTasks(StaticUrlBase.taskApiUrl, id.ToString(), true, token);
            var project = await _projectRepo.GetAsync(StaticUrlBase.ProjectApiUrl, id, token);
            TasksViewVM tasksVm = new TasksViewVM();
            if (tasks != null && project != null)
            {
                tasksVm.TasksList = tasks;
                tasksVm.Project = project;

            }

            return View(tasksVm);
        }

/*        public async Task<IActionResult> GetAllTaskFromProject(string projectId)
        {
            var token = HttpContext.Session.GetString("ShowDataToken");

            return Json(new
            {
                data = await _taskRepo.GetAllProjectTasks(StaticUrlBase.taskApiUrl, projectId, true , token)
            });
        }*/

        [HttpDelete]
        public async Task<IActionResult> Deletetask(int id)
        {
            var state = await _taskRepo.DeleteAsync(StaticUrlBase.taskApiUrl, id,
                HttpContext.Session.GetString("ShowDataToken"));
            if (state)
                return Json(new { success = true, message = "Object has been deleted." });
            else return Json(new { success = false, message = "Object has been NOT deleted" });
        }

        public async Task<IActionResult> Upserttask(int? id)
        {
            IEnumerable<Project> projectsList = await _projectRepo.GetAllAsync(StaticUrlBase.ProjectApiUrl,
                HttpContext.Session.GetString("ShowDataToken"));

            UpsertTaskVM taskVM = new UpsertTaskVM()
            {
                ProjectsList = projectsList.Select(e => new SelectListItem
                {
                    Text = e.Title,
                    Value = e.Id.ToString()
                })
            };
            taskVM.task = new task();


            if (id == null)
            {
                return View(taskVM);
            }

            taskVM.task = await _taskRepo.GetAsync(StaticUrlBase.taskApiUrl,
                id.GetValueOrDefault(),
                HttpContext.Session.GetString("ShowDataToken"));

            if (taskVM == null)
                return NotFound();

            return View(taskVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upserttask(UpsertTaskVM taskVM)
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
                    taskVM.task.Image = p1;
                }
                else if (taskVM.task.Id != 0)
                {
                    var dbtask = await _taskRepo.GetAsync(StaticUrlBase.ProjectApiUrl,
                        taskVM.task.Id,
                        HttpContext.Session.GetString("ShowDataToken"));
                    if (dbtask != null)
                    {
                        taskVM.task.Image = dbtask.Image;
                    }
                }
                bool respon;
                if (taskVM.task.Id == 0)
                {
                    respon = await _taskRepo.CreateAsync(StaticUrlBase.taskApiUrl,
                        taskVM.task,
                        HttpContext.Session.GetString("ShowDataToken"));
                }
                else
                {
                    respon = await _taskRepo.UpdateAsync(StaticUrlBase.taskApiUrl + taskVM.task.Id,
                        taskVM.task,
                        HttpContext.Session.GetString("ShowDataToken"));
                }
                if (respon)
                {
                    return RedirectToAction(nameof(Index));
                }
                else return View(taskVM);
            }
            else
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
                IEnumerable<Project> DOlist = await _projectRepo.GetAllAsync(StaticUrlBase.ProjectApiUrl,
                    HttpContext.Session.GetString("ShowDataToken"));

                taskVM.ProjectsList = DOlist.Select(e => new SelectListItem
                {
                    Text = e.Title,
                    Value = e.Id.ToString()
                });
                return View(taskVM);
            }
        }
    }
}
