using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KLTN_Team83.Models.ViewModels
{
    public class BlogVM
    {
        public Blog Blog { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TypeBlogList { get; set; }
    }
}
