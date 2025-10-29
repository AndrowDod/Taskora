using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDo_List.PL.ViewModels
{
    public class CreateEditTaskViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Due Date is required")]
        public DateTime? DueDate { get; set; } 
        public int? ProjectId { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }

        public string UserId { get; set; }
    }
}
