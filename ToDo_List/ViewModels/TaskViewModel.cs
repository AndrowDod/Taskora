using Microsoft.CodeAnalysis;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ToDo_List.PL.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public bool IsDone { get; set; }

        [Required(ErrorMessage = "Due Date is required")]
        public DateTime DueDate { get; set; }

        [AllowNull]
        public int? ProjectId { get; set; }
        public String ProjectName { get; set; }

        public String UserId { get; set; }
    }
}
