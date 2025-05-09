using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KLTN_Team83.Models.ViewModels
{
    public class RoleManagmentVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IQueryable<SelectListItem> CompanyList { get; set; }
    }
}
