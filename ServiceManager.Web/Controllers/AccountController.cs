using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Common.Models;
using ServiceManager.Web.ViewModels;

namespace ServiceManager.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ServiceManagerContext _context;
        private UserManager<SystemUser> _userManager;
        private SignInManager<SystemUser> _signInManager;

        public AccountController(ServiceManagerContext context, UserManager<SystemUser> userManager, SignInManager<SystemUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        private List<SelectListItem> PopulateProjectMultiSelect(List<Project> allProjects, List<Project> selectedProjects)
        {
            var selectListItems = new List<SelectListItem>();
            foreach (var project in allProjects)
            {
                bool isSelected = selectedProjects?.Any(x => x.Id == project.Id) ?? false;
                selectListItems.Add(new SelectListItem { Value = project.Id.ToString(), Text = project.Name, Selected = isSelected });
            }
            return selectListItems;
        }

        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _context.Users.Where(x => x.Id == Id).Include(x => x.Projects).FirstOrDefaultAsync();
            if (user == null) return NotFound();
            var projects = await _context.Project.ToListAsync();
            //ViewBag.Projects = PopulateProjectMultiSelect(projects, user.Projects.ToList());
            ViewBag.Projects = new MultiSelectList(projects, "Id", "Name", user.Projects.Select(x => x.Id.ToString()).ToList());
            var userVm = new RegisterViewModel { FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName, Email = user.Email, Status = user.Status, Team = user.Team, Designation = user.Designation };
            return View(userVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegisterViewModel pUser)
        {
            SystemUser user = null;
            List<Project> projects = null;

            ModelState.Remove(nameof(RegisterViewModel.Password));
            if (!ModelState.IsValid)
            {
                user = await _context.Users.Where(x => x.Id == pUser.Id).Include(x => x.Projects).FirstOrDefaultAsync();
                if (user == null) return NotFound();
                projects = await _context.Project.ToListAsync();
                ViewBag.Projects = new MultiSelectList(projects, "Id", "Name", user.Projects.Select(x => x.Id.ToString()).ToList());
                return View(pUser);
            }

            if (!Guid.TryParse(pUser.Id, out var userId))
            {
                ModelState.AddModelError(nameof(RegisterViewModel.Id), "User not found");
                user = await _context.Users.Where(x => x.Id == pUser.Id).Include(x => x.Projects).FirstOrDefaultAsync();
                if (user == null) return NotFound();
                projects = await _context.Project.ToListAsync();
                ViewBag.Projects = new MultiSelectList(projects, "Id", "Name", user.Projects.Select(x => x.Id.ToString()).ToList());
                return View(pUser);
            }

            //var user = await _userManager.FindByIdAsync(pUser.Id);
            user = await _context.Users.Include(x => x.Projects).Where(x => x.Id == pUser.Id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.FirstName = pUser.FirstName;
                user.LastName = pUser.LastName;
                user.Email = pUser.Email;
                user.Designation = pUser.Designation;
                user.Status = pUser.Status;
                user.Team = pUser.Team;

                // validating before saving the user
                //if (pUser.ProjectIds != null && pUser.ProjectIds.Count > 0)
                //{
                //    //bool missingProjects = _context.Project.Any(x => !pUser.ProjectIds.Contains(x.Id.ToString()));
                //    bool missingProjects = _context.Project.Any(x => !pUser.ProjectIds.Contains(x.Id.ToString()));
                //    if (missingProjects)
                //    {
                //        ModelState.AddModelError(nameof(RegisterViewModel.ProjectIds), "Project selected are not found, please refresh and try again.");

                //        ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                //        return View(pUser);
                //    }
                //}

                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    bool isDirty = false;
                    if (pUser.ProjectIds != null)
                    {
                        //var projectIds = string.Join(',', pUser.ProjectIds);
                        //string Ids = string.Empty;
                        //foreach (var projectId in pUser.ProjectIds)
                        //{
                        //    Ids += "'" + projectId + "',";
                        //}
                        //if (Ids.EndsWith(",")) Ids = Ids.Substring(0, Ids.Length - 1);

                        //projects = _context.Project.FromSqlRaw($"SELECT * FROM [Project] WHERE [Id] IN ({Ids})");
                        //var projects = user.Projects.Where(x => pUser.ProjectIds.Contains(x.Id.ToString())).ToList();
                        user.Projects.Clear();

                        projects = await _context.Project.Where(x => pUser.ProjectIds.Contains(x.Id.ToString())).ToListAsync();

                        foreach (var project in projects)
                        {
                            user.Projects.Add(project);
                            isDirty = true;
                        }
                    }

                    if (isDirty)
                    {
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("Index");
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            projects = await _context.Project.ToListAsync();
            ViewBag.Projects = new MultiSelectList(projects, "Id", "Name", user.Projects.Select(x => x.Id.ToString()).ToList());

            return View(pUser);
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string returnUrl, LogInViewModel pLogin)
        {
            if (!ModelState.IsValid) return View(pLogin);
            var user = await _userManager.FindByNameAsync(pLogin.UserName);
            if (user == null)
            {
                ModelState.AddModelError(nameof(LogInViewModel.UserName), "User not found");
                return View(pLogin);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, pLogin.Password, true, true);
            if (signInResult.Succeeded)
            {
                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectPermanent("/");
                return RedirectPermanent(returnUrl);
            }
            return View(pLogin);
        }

        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated) await _signInManager.SignOutAsync();
            return RedirectPermanent("/");
        }


        public IActionResult Register()
        {
            ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string returnUrl, RegisterViewModel pRegister)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                return View(pRegister);
            }

            var identityResult = await _userManager.CreateAsync(new SystemUser { FirstName = pRegister.FirstName, LastName = pRegister.LastName, UserName = pRegister.UserName, Email = pRegister.Email, Designation = pRegister.Designation, Status = pRegister.Status, Team = pRegister.Team }, pRegister.Password);
            if (identityResult.Succeeded)
            {
                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectPermanent("/");
                return RedirectPermanent(returnUrl);
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            ViewBag.Projects = _context.Project.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            return View(pRegister);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return Forbid();
            var profileVm = new ProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Designation = user.Designation,
                Team = user.Team,
                Status = user.Status
            };

            return View(profileVm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel pUser)
        {
            if (pUser == null) return BadRequest();
            if (!ModelState.IsValid) return View(pUser);


            if (!Guid.TryParse(pUser.Id, out var userId))
            {
                ModelState.AddModelError(nameof(ProfileViewModel.Id), "User not found");
                return View(pUser);
            }


            var user = await _userManager.FindByIdAsync(pUser.Id);
            if (user == null) return Forbid();

            user.FirstName = pUser.FirstName;
            user.LastName = pUser.FirstName;
            user.Designation = pUser.Designation;
            user.Status = pUser.Status;
            user.Team = pUser.Team;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Redirect("/");
            }

            foreach(var error in result.Errors)
            {
                // keys are same
                ModelState.AddModelError(error.Code, error.Description);
            }

            return View(pUser);
        }


    }
}
