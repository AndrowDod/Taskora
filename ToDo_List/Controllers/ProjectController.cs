using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDo_List.BLL.Repositories;
using ToDo_List.PL.ViewModels;
using ToDO_List.DAL.Data.Models;

namespace ToDo_List.PL.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly TasksRepository _tasksRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectController(TasksRepository tasksRepository, ProjectRepository projectRepository, IMapper mapper)
        {
            _tasksRepository = tasksRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        #region Index
        public IActionResult Index()
        {

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var allTasks = _projectRepository.GetAll()
                .Where(p => p.userId != null && p.userId.ToString() == userId);
            
            var projects = _mapper.Map<IEnumerable<ProjectViewModel>>(allTasks);
            return View(projects);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProjectViewModel createdProject)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                var project = _mapper.Map<ProjectViewModel, Project>(createdProject);
                project.userId = userId;

                _projectRepository.Create(project);
                return RedirectToAction("Index", "Project");
                
            }
            return View(createdProject);
        }

        #endregion

        #region Details
        public IActionResult Details(int id)
        {
            var project = _mapper.Map<ProjectViewModel>(_projectRepository.GetById(id));
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
        #endregion

        #region Edit

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var project = _projectRepository.GetById(id.Value);
            project.userId = userId;
            if (project is null)
                return NotFound();

            var existingTasks = project.Tasks;
            var mappedProject = _mapper.Map<ProjectViewModel>(project);
            project.Tasks = existingTasks;
            return View(mappedProject);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, ProjectViewModel editedProject)
        {
            if (!ModelState.IsValid || editedProject is null)
            {
                return View(editedProject);
            }

            var project = _projectRepository.GetById(id);
            if (project is null)
                return NotFound();
            var userId = project.userId;

            try
            {
                var existingTasks = project.Tasks;

                _mapper.Map(editedProject, project);
                project.userId = userId;
                project.Tasks = existingTasks;



                _projectRepository.Update(project);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(editedProject);
            }
        }
            
        

        #endregion

        #region Delete

        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var project = _projectRepository.GetById(id.Value);

            if (project is null)
                return NotFound();

            var mappedProject = _mapper.Map<ProjectViewModel>(project);

            return View(mappedProject);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, ProjectViewModel deletedProject)
        {
            if (id != deletedProject.Id || deletedProject is null)
                return BadRequest();

            var project = _projectRepository.GetById(deletedProject.Id);
            if (project == null)
                return NotFound();

            try
            {
                _projectRepository.Delete(project);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(String.Empty, ex.Message);
                return View(deletedProject);
            }

        }

        #endregion

        #region Search

        public IActionResult Search(string projectName)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var projects = _projectRepository.GetByName(projectName , userId);
            var mappedProjects = _mapper.Map<IEnumerable<ProjectViewModel>>(projects);
            return View("Index", mappedProjects);
        }

        #endregion


    }
}
