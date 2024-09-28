using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Application.ViewModels.ManageUsers
{
    public class ManageUsersVm
    {
        public  string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsUser { get; set; }
        public bool IsSuperUser { get; set; }
        public bool IsAdmin { get; set; }
    }
}
