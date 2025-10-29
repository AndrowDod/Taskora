using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;


namespace ToDO_List.DAL.Data.Models
{
    public class Project : ModelBase
    {
        public string Name { get; set; }

        public DateTime DueDate { get; set; }

        //navigatioonal property
        public ICollection<Tasks> Tasks { get; set; }


        //navigatioonal property for user
        public string userId { get; set; }
        public IdentityUser User { get; set; }
    }
}
