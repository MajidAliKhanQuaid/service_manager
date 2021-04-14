using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceManager.Common.Models;
using ServiceManager.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManager.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly ServiceManagerContext _context;
        private readonly RoleManager<SystemRole> _roleManager;

        public RoleController(ServiceManagerContext context, RoleManager<SystemRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _context.Roles.Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel pRole)
        {
            ModelState.Remove(nameof(RoleViewModel.Id));
            if (!ModelState.IsValid) return View(pRole);
            if (!await _roleManager.RoleExistsAsync(pRole.Name))
            {
                var result = await _roleManager.CreateAsync(new SystemRole { Name = pRole.Name, Description = pRole.Description });
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                //ModelState.AddModelError(string.Empty, "Role could not be create, please try again");
                return View(pRole);
            }
            ModelState.AddModelError(nameof(SystemRole.Name), "Role name already exists, enter a new one");
            return View(pRole);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null) return RedirectToAction("Index", new { Message = "Role not found" });
            var roleVm = new RoleViewModel { Id = role.Id, Name = role.Name, Description = role.Description };
            return View(roleVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel pRole)
        {
            if (!ModelState.IsValid) return View(pRole);
            if(Guid.TryParse(pRole.Id, out var Id))
            {
                var role = await _roleManager.FindByIdAsync(pRole.Id);
                if(role != null)
                {
                    role.Name = pRole.Name;
                    role.Description = pRole.Description;

                    var identityResult = await _roleManager.UpdateAsync(role);
                    if (identityResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach(var error in identityResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                        return View(pRole);
                    }
                }
            }
            ModelState.AddModelError(nameof(RoleViewModel.Id), "No role find with the identifier.");
            return View(pRole);
        }

    }
}
