namespace TaskManager.Service.Business
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::TaskManager.Service.Models;
    using Repository;

    /// <summary>
    /// Task Manager Class.
    /// </summary>
    public class TaskManager : ITaskManager
    {
        private readonly ITaskDetailsRepository _taskDetailsRepository;        

        /// <summary>
        /// Constructot for TaskManager
        /// </summary>
        /// <param name="taskManagerRepository">taskManagerRepository</param>        
        public TaskManager(ITaskDetailsRepository taskManagerRepository)
        {
            _taskDetailsRepository = taskManagerRepository;            
        }

        /// <summary>
        /// Method to get all the tasks.
        /// </summary>
        /// <returns>tasks</returns>
        public async Task<IEnumerable<TaskDetailModel>> GetAllTasks()
        {
            return await _taskDetailsRepository.GetAllTasks();
        }

        /// <summary>
        /// Method to get the atsk detail.
        /// </summary>
        /// <param name="id">task id</param>
        /// <returns>task detail</returns>
        public async Task<TaskDetailModel> GetTaskDetail(int id)
        {
            return await _taskDetailsRepository.Get(id);
        }

        /// <summary>
        /// Method to add task details
        /// </summary>
        /// <param name="taskItem">task detail</param>
        /// <returns></returns>
        public async Task<int> AddTaskDetails(TaskDetailModel taskItem)
        {
            return await _taskDetailsRepository.Insert(taskItem);
        }

        /// <summary>
        /// Method to update the task details
        /// </summary>
        /// <param name="id">taskid</param>
        /// <param name="taskItem">taskdetail</param>
        /// <returns></returns>
        public async Task UpdateTaskDetails(int id, TaskDetailModel taskItem)
        {
            await _taskDetailsRepository.Update(id, taskItem);
        } 

        /// <summary>
        /// method to check whether the task is valid to close.
        /// </summary>
        /// <param name="taskItem"></param>
        /// <returns></returns>
        public bool IsTaskValid(TaskDetailModel taskItem)
        {            
            var taskItems = _taskDetailsRepository.GetAllTasks().Result;
            var isValid = !taskItems.Any(t => t.ParentTaskId == taskItem.Id && t.EndTask == false);                    
            return isValid;
        }
    }
}
