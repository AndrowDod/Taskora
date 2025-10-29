using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDO_List.DAL.Data.Models;

namespace ToDo_List.PL.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Due Date is required")]
        public DateTime DueDate { get; set; }

        //navigatioonal property
        public ICollection<Tasks> Tasks { get; set; }
        public String UserId { get; set; }


    }
}
