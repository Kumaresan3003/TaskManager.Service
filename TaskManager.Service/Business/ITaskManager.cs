namespace TaskManager.Service.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::TaskManager.Service.Models;

    /// <summary>
    /// Interface for TaskManager.
    /// </summary>
    public interface ITaskManager
    {   
        Task<IEnumerable<TaskDetailModel>> GetAllTasks();

        Task<TaskDetailModel> GetTaskDetail(int id);

        Task<int> AddTaskDetails(TaskDetailModel task);

        Task UpdateTaskDetails(int id, TaskDetailModel task);

        bool IsTaskValid(TaskDetailModel task);
    }
}
