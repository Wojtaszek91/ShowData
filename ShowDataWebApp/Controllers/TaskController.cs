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

        public async Task<IActionResult> Deletetask(int id)
        {
            var token = HttpContext.Session.GetString("ShowDataToken");
            var task = await _taskRepo.GetAsync(StaticUrlBase.taskApiUrl, id, token);
            var taskDeleteState = await _taskRepo.DeleteAsync(StaticUrlBase.taskApiUrl, id,
                HttpContext.Session.GetString("ShowDataToken"));
            if (taskDeleteState)
            {
                var project = await _projectRepo.GetAsync(StaticUrlBase.ProjectApiUrl, task.ProjectId, token);
                project.TasksIncluded--;
                var projectUpdateState = await _projectRepo.UpdateAsync(StaticUrlBase.ProjectApiUrl + task.ProjectId, project, token);
                if (projectUpdateState)
                {
                return RedirectToAction(nameof(TasksView), new { id = task.ProjectId });
                }
                else
                {
                    return RedirectToAction(nameof(TasksView), new { id = task.ProjectId });
                }

            }
            else
            {
                return RedirectToAction(nameof(TasksView), new { id = task.ProjectId });
            }
        }

        /// <summary>
        /// Returns view with model to create or update task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Upserttask(int? id)
        {
            var token = HttpContext.Session.GetString("ShowDataToken");
            IEnumerable<Project> projectsList = await _projectRepo.GetAllAsync(StaticUrlBase.ProjectApiUrl, token);

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
                id.GetValueOrDefault(), token);

            if (taskVM == null)
                return NotFound();

            return View(taskVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upserttask(UpsertTaskVM taskVM)
        {
            var token = HttpContext.Session.GetString("ShowDataToken");

            if (ModelState.IsValid)
            {
                bool didUpdateProject = true; // true because no need to update project if task didnt change parent.
                bool taskUpsertResponse;

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
                    var dbtask = await _taskRepo.GetAsync(StaticUrlBase.taskApiUrl, taskVM.task.Id, token);
                    if (dbtask != null)
                    {
                        taskVM.task.Image = dbtask.Image;
                    }
                    if(dbtask.ProjectId != taskVM.task.ProjectId)
                    {
                        var oldDbProject = await _projectRepo.GetAsync(StaticUrlBase.ProjectApiUrl, dbtask.ProjectId, token);
                        var newDbProject = await _projectRepo.GetAsync(StaticUrlBase.ProjectApiUrl, taskVM.task.ProjectId, token);
                        if (newDbProject != null && oldDbProject != null)
                        {
                            newDbProject.TasksIncluded++;
                            oldDbProject.TasksIncluded--;
                            didUpdateProject = await _projectRepo.UpdateAsync(StaticUrlBase.ProjectApiUrl + dbtask.ProjectId, oldDbProject, token);
                            didUpdateProject = await _projectRepo.UpdateAsync(StaticUrlBase.ProjectApiUrl + taskVM.task.ProjectId, newDbProject, token);
                        }
                        else
                        {
                            didUpdateProject = false;
                        }
                    }
                }
                if (taskVM.task.Id == 0)
                {
                    taskVM.task.isAvailsable = true;
                    var dbProject = await _projectRepo.GetAsync(StaticUrlBase.ProjectApiUrl, taskVM.task.ProjectId, token);
                    if (dbProject != null)
                    {
                        dbProject.TasksIncluded++;
                        didUpdateProject = await _projectRepo.UpdateAsync(StaticUrlBase.ProjectApiUrl + taskVM.task.ProjectId, dbProject, token);
                    }
                    else
                    {
                        didUpdateProject = false;
                    }
                    taskUpsertResponse = await _taskRepo.CreateAsync(StaticUrlBase.taskApiUrl, taskVM.task, token);
                }
                else
                {
                    taskUpsertResponse = await _taskRepo.UpdateAsync(StaticUrlBase.taskApiUrl + taskVM.task.Id, taskVM.task, token);
                }
                if (taskUpsertResponse && didUpdateProject)
                {
                    return RedirectToAction(nameof(Index));
                }
                else return View(taskVM);
            }
            else
            {
/*                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }*/
                IEnumerable<Project> projectsList = await _projectRepo.GetAllAsync(StaticUrlBase.ProjectApiUrl, token);

                taskVM.ProjectsList = projectsList.Select(e => new SelectListItem
                {
                    Text = e.Title,
                    Value = e.Id.ToString()
                });
                return View(taskVM);
            }
        }
    }
}
