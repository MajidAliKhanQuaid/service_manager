using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Common.Models;
using ServiceManager.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManager.Web.Controllers
{
    public class MachineController : Controller
    {
        private readonly ServiceManagerContext _context;

        public MachineController(ServiceManagerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var machines = _context.Machine.Include(x => x.Services)
                .Select(x => new MachineViewModel
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Specs = x.Specs,
                    Identifier = x.Identifier,
                    Services = x.Services.Select(y => new SelectListItem
                    {
                        Value = y.Id.ToString(),
                        Text = y.Name.ToString()
                    }).ToList()
                })
                .ToList();
            return View(machines);
        }

        public async Task<IActionResult> Create()
        {
            var services = await _context.SystemService.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToListAsync();
            ViewBag.Services = services;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MachineViewModel pMachine)
        {
            if (pMachine == null) return BadRequest();
            ModelState.Remove(nameof(MachineViewModel.Id));
            if (!ModelState.IsValid) return View(pMachine);
            var machine = new Machine { Name = pMachine.Name, Specs = pMachine.Specs, Identifier = pMachine.Identifier };
            _context.Machine.Add(machine);

            //var selectedServices = await _context.SystemService.Where(x => pMachine.ServiceIds.Contains(x.Id.ToString())).ToListAsync();
            //foreach (var selectedService in selectedServices)
            //{
            //    machine.Services.Add(selectedService);
            //}
            //_context.Machine.Update(machine);

            int recordsAdded = await _context.SaveChangesAsync();
            if (recordsAdded == 0)
            {
                ModelState.AddModelError(string.Empty, "Request could not be processed, please try again");
                return View(pMachine);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string Id)
        {
            if (!Guid.TryParse(Id, out var guid)) return NotFound();
            var machine = await _context.Machine.Where(x => x.Id == guid).FirstOrDefaultAsync();
            //var services = await _context.SystemService.Select(x => new SelectListItem
            //{
            //    Value = x.Id.ToString(),
            //    Text = x.Name
            //}).ToListAsync();
            //var selectedServiceIds = machine.Services.Select(x => x.Id.ToString()).ToList();
            if (machine == null) return NotFound();
            //var projectVm = new MachineViewModel { Id = machine.Id.ToString(), Name = machine.Name, Specs = machine.Specs, Identifier = machine.Identifier, Services = services, ServiceIds = selectedServiceIds };
            var projectVm = new MachineViewModel { Id = machine.Id.ToString(), Name = machine.Name, Specs = machine.Specs, Identifier = machine.Identifier };
            return View(projectVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MachineViewModel pMachine)
        {
            if (pMachine == null) return BadRequest();

            Machine machine = null;
            List<SelectListItem> serviceListItems = null;

            // validating guid for project
            if (!Guid.TryParse(pMachine.Id, out var projectId)) return NotFound();

            if (!ModelState.IsValid)
            {
                machine = await _context.Machine.Where(x => x.Id == projectId).FirstOrDefaultAsync();
                //serviceListItems = await _context.SystemService.Select(x => new SelectListItem
                //{
                //    Value = x.Id.ToString(),
                //    Text = x.Name
                //}).ToListAsync();
                //var selectedServiceIds = machine.Services.Select(x => x.Id.ToString()).ToList();
                if (machine == null) return NotFound();
                //var projectVm = new MachineViewModel { Id = machine.Id.ToString(), Name = machine.Name, Specs = machine.Specs, Identifier = machine.Identifier, Services = serviceListItems, ServiceIds = selectedServiceIds };
                var projectVm = new MachineViewModel { Id = machine.Id.ToString(), Name = machine.Name, Specs = machine.Specs, Identifier = machine.Identifier };
                return View(projectVm);
            }

            machine = await _context.Machine.FindAsync(projectId);
            if (machine == null)
            {
                ModelState.AddModelError(nameof(MachineViewModel.Id), "Machine not found.");

                machine = await _context.Machine.Where(x => x.Id == projectId).FirstOrDefaultAsync();
                //serviceListItems = await _context.SystemService.Select(x => new SelectListItem
                //{
                //    Value = x.Id.ToString(),
                //    Text = x.Name
                //}).ToListAsync();
                //var selectedServiceIds = machine.Services.Select(x => x.Id.ToString()).ToList();
                if (machine == null) return NotFound();
                //var projectVm = new MachineViewModel { Id = machine.Id.ToString(), Name = machine.Name, Specs = machine.Specs, Identifier = machine.Identifier, Services = serviceListItems, ServiceIds = selectedServiceIds };
                var projectVm = new MachineViewModel { Id = machine.Id.ToString(), Name = machine.Name, Specs = machine.Specs, Identifier = machine.Identifier };
                return View(projectVm);
            }

            machine.Name = pMachine.Name;
            machine.Specs = machine.Specs;
            machine.Identifier = machine.Identifier;

            _context.Machine.Update(machine);

            int recordsUpdated = await _context.SaveChangesAsync();
            if (recordsUpdated == 0)
            {
                ModelState.AddModelError(nameof(MachineViewModel.Id), "Machine could not be updated. Please try again.");

                machine = await _context.Machine.Where(x => x.Id == projectId).FirstOrDefaultAsync();
                //serviceListItems = await _context.SystemService.Select(x => new SelectListItem
                //{
                //    Value = x.Id.ToString(),
                //    Text = x.Name
                //}).ToListAsync();
                //var selectedServiceIds = machine.Services.Select(x => x.Id.ToString()).ToList();
                if (machine == null) return NotFound();
                //var projectVm = new MachineViewModel { Id = machine.Id.ToString(), Name = machine.Name, Specs = machine.Specs, Identifier = machine.Identifier, Services = serviceListItems, ServiceIds = selectedServiceIds };
                var projectVm = new MachineViewModel { Id = machine.Id.ToString(), Name = machine.Name, Specs = machine.Specs, Identifier = machine.Identifier};
                return View(projectVm);
            }
            return RedirectToAction("Index");
        }
    }
}
