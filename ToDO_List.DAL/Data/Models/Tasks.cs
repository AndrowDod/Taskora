using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDO_List.DAL.Data.Models
{
    public class Tasks : ModelBase
    {
        public String Title { get; set; }
        public bool IsDone { get; set; }
        public DateTime DueDate { get; set; }

        //navigatioonal property for project
        public int? projectId { get; set; }
        public Project Project { get; set; }

        //navigatioonal property for user
        public string userId { get; set; }
        public IdentityUser User { get; set; }
    }
}
