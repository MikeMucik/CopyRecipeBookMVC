using System.Diagnostics;
using CopyRecipeBook.Web.Models;
using CopyRecipeBookMVC.Application.ViewModels.ManageUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CopyRecipeBook.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<IdentityUser> _userManager;    
        public HomeController(ILogger<HomeController> logger,
			UserManager<IdentityUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Privacy()
		{
			return View();
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}		
		[HttpGet]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
		{      
            var users = await _userManager.Users.ToListAsync();
			var model = new ListManageUsersVm()
			{
				ListUsers = new List<ManageUsersVm>()
			};
            foreach (var user in users)
            {
				var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
					Console.WriteLine("Jest Adminem");
                }
                model.ListUsers.Add(new ManageUsersVm
				{
					UserId = user.Id,
					UserName = user.UserName,
					IsUser = roles.Contains("User"),
					IsSuperUser = roles.Contains("SuperUser"),
					IsAdmin = roles.Contains("Admin")
				});
            }
            return View(model);
		}
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ManageUsers(ListManageUsersVm model)
        {
            foreach (var userVm in model.ListUsers)
            {
                var user = await _userManager.FindByIdAsync(userVm.UserId);
                if (user != null)
                {                   
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);                    
                    if (userVm.IsAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else if (userVm.IsSuperUser)
                    {
                        await _userManager.AddToRoleAsync(user, "SuperUser");
                    }
                    else if (userVm.IsUser)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                }
            }
            return RedirectToAction(nameof(ManageUsers));
        }
    }
}
