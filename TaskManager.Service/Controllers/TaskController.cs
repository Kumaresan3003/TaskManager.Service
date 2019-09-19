namespace TaskManager.Service.Controllers
{
    using Business;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using TaskDetail = Models.TaskDetailModel;

    /// <summary>
    /// Task API Controller class.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Task")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly ITaskManager _taskManager;
        private readonly ILogger<TaskController> _logger;

        /// <summary>
        /// Constructor for TaskController.
        /// </summary>
        /// <param name="taskManager">Taskmanager</param>
        /// <param name="logger">logger</param>
        public TaskController(ITaskManager taskManager, ILogger<TaskController> logger)
        {
            _taskManager = taskManager;
            _logger = logger;
        }

        // GET api/Task

        /// <summary>
        /// /Method to get all available task details
        /// </summary>
        /// <returns>Task Details</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Fetching Available Task Details");
                return Ok(await _taskManager.GetAllTasks());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An exception has occured with Service, Please contact service team");
            }
        }

        // GET api/Task/1

        /// <summary>
        /// Method to get the task by taskid
        /// </summary>
        /// <param name="id">task id</param>
        /// <returns>task details</returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching details for task {id}");
                return Ok(await _taskManager.GetTaskDetail(id));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An exception has occured with Service while fetching task details, Please contact service team");
            }
        }

        // POST api/Task

        /// <summary>
        /// Method to add a new task.
        /// </summary>
        /// <param name="task">Task detail</param>
        /// <returns>status</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskDetail task)
        {
            try
            {
                if(task == null)
                {
                    _logger.LogInformation("Invalid task item detail.");
                    return BadRequest();
                }

                await _taskManager.AddTaskDetails(task);

                _logger.LogInformation($"Inserted task to database with id {task.Id}");

                return Ok($"Task with id {task.Id} created successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An exception has occured with Service while inserting task, Please contact service team");
            }
        }

        // PUT api/Task/1

        /// <summary>
        /// Method to update task
        /// </summary>
        /// <param name="id">taskid</param>
        /// <param name="task">taskdetail</param>
        /// <returns>status</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TaskDetail task)
        {
            try
            {
                if (task == null || task.Id != id)
                {
                    _logger.LogInformation("Provide valid task item detail");
                    return BadRequest("Invalid task detail");
                }

                if (!_taskManager.IsTaskValid(task))
                {                    
                    return BadRequest("This task has active child tasks. Active child tasks has to be closed before closing parent task");
                }

                await _taskManager.UpdateTaskDetails(id, task);
                _logger.LogInformation($"Updated task with id  {task.Id}");

                return Ok($"{task.Name} Saved");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An exception has occured with Service, Please contact service team");
            }
        }
    }
}
