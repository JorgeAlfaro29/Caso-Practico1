using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PAW3CP1.Models.DTO;
using PAW3CP1.Mvc.ServiceApi;

namespace PAW3CP1.Mvc.Controllers
{
    public class TasksController : Controller
    {

        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        // GET: TasksController
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetDataAsync<TaskDTO>();
            return View(tasks);
        }

        // GET: TasksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TasksController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TasksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskDTO tasks)
        {
            if (!ModelState.IsValid) return View(tasks);
            await _taskService.CreateAsync(tasks);
            return RedirectToAction(nameof(Index));
        }

        // GET: TasksController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tasks = await _taskService.GetDataAsync<TaskDTO>();
            var task = tasks.FirstOrDefault(c => c.Id == id);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // POST: TasksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskDTO tasks)
        {
            if (!ModelState.IsValid) return View(tasks);
            await _taskService.UpdateAsync(id, tasks);
            return RedirectToAction(nameof(Index));
        }

        // GET: TasksController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tasks = await _taskService.GetDataAsync<TaskDTO>();
            var task = tasks.FirstOrDefault(c => c.Id == id);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // POST: TasksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, TaskDTO tasks)
        {
            await _taskService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
