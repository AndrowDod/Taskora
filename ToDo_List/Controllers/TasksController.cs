using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDo_List.BLL.Repositories;
using ToDo_List.PL.ViewModels;
using ToDO_List.DAL.Data.Models;

namespace ToDo_List.PL.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {

        private readonly TasksRepository _tasksRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public TasksController(TasksRepository tasksRepository, ProjectRepository projectRepository, IMapper mapper)
        {
            _tasksRepository = tasksRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        #region Index
        public IActionResult Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var allTasks = _tasksRepository.GetAll()
                .Where(t => t.userId != null && t.userId.ToString() == userId);

            var tasks = _mapper.Map<IEnumerable<TaskViewModel>>(allTasks);
            return View(tasks);

        } 
        #endregion

        #region ToggleTaskStatus

        [HttpPost]
        [ValidateAntiForgeryToken] // يحمي ضد CSRF
        public IActionResult ToggleTaskStatus([FromBody] ToggleDtoViewModel dto)
        {
            if (dto == null) return BadRequest(new { success = false, message = "Invalid payload" });

            var task = _tasksRepository.GetById(dto.Id);
            if (task == null) return NotFound(new { success = false, message = "Task not found" });

            task.IsDone = dto.IsDone;

            _tasksRepository.Update(task);

            return Ok(new { success = true, isDone = task.IsDone });
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var task = new CreateEditTaskViewModel()
            {
                Projects = _projectRepository.GetAll()
                .Where(p => p.userId != null && p.userId.ToString() == userId)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList(),

                DueDate = null

            };
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEditTaskViewModel createdTask)
        {
            if (!ModelState.IsValid)
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                createdTask.Projects = _projectRepository.GetAll()
                   .Where(p => p.userId != null && p.userId.ToString() == userId)
                   .Select(p => new SelectListItem
                   {
                       Value = p.Id.ToString(),
                       Text = p.Name
                   })
                   .ToList();

                return View(createdTask);
            }

            var task = new Tasks()
            {
                Title = createdTask.Title,
                DueDate = (DateTime)createdTask.DueDate,
                projectId = createdTask.ProjectId,
                IsDone = false,
                userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value

            };
            _tasksRepository.Create(task);
            return RedirectToAction("Index" , "Tasks");
        }


        #endregion

        #region Delete

        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var task = _tasksRepository.GetById(id.Value);

            if (task is null)
                return NotFound();

            var mappedTask = _mapper.Map<TaskViewModel>(task);

            return View(mappedTask);
         
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, TaskViewModel deletedTask)
        {
            if (id != deletedTask.Id || deletedTask is null)
                return BadRequest();

                var task = _tasksRepository.GetById(deletedTask.Id);
                if (task == null)
                return NotFound();

            try
            {
                _tasksRepository.Delete(task);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(String.Empty, ex.Message);
                return View(task);
            }

        }

        #endregion

        #region Edit

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var task = _tasksRepository.GetById(id.Value);

            if (task is null)
                return NotFound();

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var mappedTask = _mapper.Map<CreateEditTaskViewModel>(task);
            mappedTask.Projects = _projectRepository.GetAll()
                .Where(p => p.userId != null && p.userId.ToString() == userId)
                .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
            return View(mappedTask);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id, CreateEditTaskViewModel editedTask)
        {
            if (ModelState.IsValid || editedTask is not null)
            {

                var task = _tasksRepository.GetById(id);
                if (task is null)
                    return NotFound();

                try
                {
                    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    var mappedtask = _mapper.Map(editedTask, task);
                    mappedtask.userId = userId;
                    _tasksRepository.Update(mappedtask);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(editedTask);
                }
            }
            return View(editedTask);
        }

        #endregion

        #region Search

        public IActionResult Search(string taskTitle)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var tasks = _tasksRepository.GetByTitle(taskTitle , userId);
            var mappedTasks = _mapper.Map<IEnumerable<TaskViewModel>>(tasks);
            return View("Index", mappedTasks);
        }

        #endregion
    }
}
