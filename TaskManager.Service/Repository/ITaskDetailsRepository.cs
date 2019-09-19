namespace TaskManager.Service.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TaskManager.Service.Models;

    /// <summary>
    /// Interface for TaskDetailsRepository.
    /// </summary>
    public interface ITaskDetailsRepository
    {
        Task<IEnumerable<TaskDetailModel>> GetAllTasks();

        Task<TaskDetailModel> Get(int id);

        Task<int> Insert(TaskDetailModel entity);

        Task Update(int id, TaskDetailModel entity);

        Task Delete(TaskDetailModel entity);
    }
}
