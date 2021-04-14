using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Common.Models;
using ServiceManager.Web.Requests.Services;
using ServiceManager.Web.ViewModels;
using ServiceManager.WindowsService.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManager.Web.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly ServiceManagerContext _context;
        private readonly ISystemServiceManager _systemServiceManager;
        public ServiceController(ServiceManagerContext context, ISystemServiceManager systemServiceManager)
        {
            _context = context;
            _systemServiceManager = systemServiceManager;
        }

        public IActionResult Index()
        {
            var services = _context.SystemService.ToList();
            return View(services);
        }

        public IActionResult Create()
        {
            ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SystemServiceViewModel pService)
        {
            if (pService == null) return BadRequest();
            ModelState.Remove(nameof(SystemServiceViewModel.Id));
            if (!ModelState.IsValid)
            {
                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                return View(pService);
            }

            // validating project guid id
            if (!Guid.TryParse(pService.ProjectId, out var projectId))
            {
                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ModelState.AddModelError(nameof(SystemServiceViewModel.ProjectId), "Project was not found, please refresh and then select project.");
                return View(pService);
            }

            // validating machine guid id
            if (!Guid.TryParse(pService.MachineId, out var machineId))
            {
                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ModelState.AddModelError(nameof(SystemServiceViewModel.MachineId), "Machine was not found, please refresh and then select machine.");
                return View(pService);
            }

            // validate project
            var project = _context.Project.Find(projectId);
            if (project == null)
            {
                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ModelState.AddModelError(nameof(SystemServiceViewModel.ProjectId), "Project was not found, please refresh and then select project.");
                return View(pService);
            }
            
            // validate project
            var machine = _context.Machine.Find(machineId);
            if (machine == null)
            {
                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ModelState.AddModelError(nameof(SystemServiceViewModel.ProjectId), "Machine was not found, please refresh and then select machine.");
                return View(pService);
            }

            int recordsAdded = await _systemServiceManager.AddServiceAsync(new SystemService { Id = Guid.NewGuid(), Name = pService.Name, Description = pService.Description, ProjectId = project.Id, MachineId = machine.Id });
            if (recordsAdded == 0)
            {
                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ModelState.AddModelError(string.Empty, "Request could not be processed, please try again");
                return View(pService);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string Id)
        {
            if (!Guid.TryParse(Id, out var guid)) return NotFound();
            var service = await _context.SystemService.FindAsync(guid);
            if (service == null) return NotFound();
            ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            var sServiceVm = new SystemServiceViewModel { Id = service.Id.ToString(), Name = service.Name, Description = service.Description, ProjectId = service.ProjectId.ToString(), MachineId = service.MachineId.ToString() };
            return View(sServiceVm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(SystemServiceViewModel pService)
        {
            if (pService == null) return BadRequest();

            if (!Guid.TryParse(pService.ProjectId, out var projectId))
            {
                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ModelState.AddModelError(nameof(SystemServiceViewModel.ProjectId), "Project was not found, please refresh and then select project.");
                return View(pService);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                return View(pService);
            }

            // validate project id
            var project = await _context.Project.FindAsync(projectId);
            if (project == null)
            {
                ModelState.AddModelError(nameof(SystemServiceViewModel.ProjectId), "Project was not found, please refresh and then select project.");

                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                return View(pService);
            }

            if (!Guid.TryParse(pService.Id, out var serviceId))
            {
                ModelState.AddModelError(nameof(SystemServiceViewModel.ProjectId), "Project was not found, please refresh and then select project.");

                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                return View(pService);
            }

            var service = await _context.SystemService.FindAsync(serviceId);
            if (service == null)
            {
                ModelState.AddModelError(string.Empty, "Service was not found, please try again.");

                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                return View(pService);
            }

            service.Name = pService.Name;
            service.Description = pService.Description;
            service.ProjectId = projectId;

            _context.SystemService.Update(service);
            int recordsAdded = await _context.SaveChangesAsync();
            if (recordsAdded == 0)
            {
                ModelState.AddModelError(string.Empty, "Request could not be processed, please try again");

                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                ViewBag.Machines = _context.Machine.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                return View(pService);
            }

            return RedirectToAction(nameof(Index));
        }

        #region AJAX Calls

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Start([FromBody] ServiceCommandRequest pRequest)
        {
            if (Guid.TryParse(pRequest.ServiceId, out var guid))
            {
                var service = await _context.SystemService.Where(x => x.Id == guid).Include(x => x.Machine).FirstOrDefaultAsync();
                if (service == null) return BadRequest();
                _context.CommandRequest.Add(new CommandRequest { Id = Guid.NewGuid(), Command = "start", RequestedBy = User.Identity.Name, ServiceId = guid, MachineIdentifier = service.Machine.Identifier, RequestTimeUtc = DateTime.UtcNow });
                await _context.SaveChangesAsync();
                return Json(new { message = "Service has started" });
            }
            return Json(new { message = "Failed to start service" });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Stop([FromBody] ServiceCommandRequest pRequest)
        {
            if (Guid.TryParse(pRequest.ServiceId, out var guid))
            {
                var service = await _context.SystemService.Where(x => x.Id == guid).Include(x => x.Machine).FirstOrDefaultAsync();
                if (service == null) return BadRequest();
                _context.CommandRequest.Add(new CommandRequest { Id = Guid.NewGuid(), Command = "stop", RequestedBy = User.Identity.Name, ServiceId = guid, MachineIdentifier = service.Machine.Identifier, RequestTimeUtc = DateTime.UtcNow });
                await _context.SaveChangesAsync();
                return Json(new { message = "Service has stopped" });
            }
            return Json(new { message = "Failed to stop service" });
        }

        #endregion

    }

}
