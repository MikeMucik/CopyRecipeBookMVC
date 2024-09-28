using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyRecipeBookMVC.Application.ViewModels.ManageUsers
{
    public class ListManageUsersVm
    {
        public List<ManageUsersVm> ListUsers { get; set; } = new List<ManageUsersVm>();
    }
}
