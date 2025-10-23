using Microsoft.AspNetCore.Mvc;
using PAW3CP1.Core.BusinessLogic;
using PAW3CP1.Data.Models;
using PAW3CP1.Models.DTO;
using PAW3CP1.Data.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PAW3CP1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksApiController(ITaskBusiness taskBusiness) : ControllerBase
    {
        
        // GET: api/<TasksApiController>
        [HttpGet]
        public async Task<IEnumerable<Tasks>> Get()
        {
            return await taskBusiness.GetTask(id: null);
        }

        // GET api/<TasksApiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TasksApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskDTO tasks)
        {
            var result = await taskBusiness.SaveTaskAsync(TaskDTOExtensions.ToTasks(tasks));
            return result ? Ok(tasks) : BadRequest("No se pudo guardar la tarea.");
        }

        // PUT api/<TasksApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TaskDTO tasks)
        {
            if (id != tasks.Id)
                return BadRequest("El ID no coincide.");

            var result = await taskBusiness.SaveTaskAsync(TaskDTOExtensions.ToTasks(tasks));
            return result ? Ok(tasks) : BadRequest("No se pudo actualizar la tarea.");
        }

        // DELETE api/<TasksApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await taskBusiness.DeleteTaskAsync(id);
            return result ? Ok($"Tarea {id} eliminada correctamente.") : BadRequest("No se pudo eliminar la tarea.");
        }
    }
}
