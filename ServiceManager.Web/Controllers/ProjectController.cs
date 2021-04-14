using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Common.Extensions;
using ServiceManager.Common.Models;
using ServiceManager.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManager.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ServiceManagerContext _context;

        public ProjectController(ServiceManagerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var projects = _context.Project.Include(x => x.Services).ToList();
            return View(projects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProjectViewModel pProject)
        {
            ModelState.Remove(nameof(ProjectViewModel.Id));
            if (!ModelState.IsValid) return View(pProject);
            _context.Project.Add(new Project { Name = pProject.Name, Description = pProject.Description });
            int recordsAdded = _context.SaveChanges();
            if (recordsAdded == 0)
            {
                ModelState.AddModelError(string.Empty, "Request could not be processed, please try again");
                return View(pProject);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string Id)
        {
            if (!Guid.TryParse(Id, out var guid)) return NotFound();
            var project = _context.Project.Find(guid);
            if (project == null) return NotFound();
            var projectVm = new ProjectViewModel { Id = project.Id.ToString(), Name = project.Name, Description = project.Description };
            return View(projectVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectViewModel pProject)
        {
            if (!ModelState.IsValid) return View(pProject);

            // validating guid for project
            if (!Guid.TryParse(pProject.Id, out var projectId))
            {
                ModelState.AddModelError(nameof(ProjectViewModel.Id), "Project not found.");
                return View(pProject);
            }

            var project = await _context.Project.FindAsync(projectId);
            if (project == null)
            {
                ModelState.AddModelError(nameof(ProjectViewModel.Id), "Project not found.");
                return View(pProject);
            }
            project.Name = pProject.Name;
            project.Description = project.Description;
            _context.Project.Update(project);

            int recordsUpdated = await _context.SaveChangesAsync();
            if(recordsUpdated == 0)
            {
                ModelState.AddModelError(nameof(ProjectViewModel.Id), "Project could not be updated. Please try again.");
                return View(pProject);
            }
            return RedirectToAction("Index");
        }
    }
}
