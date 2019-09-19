namespace TaskManager.Service.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class TaskDetailsRepository : ITaskDetailsRepository
    {
        private readonly TaskManagerDbContext _taskManagerDbContext;

        public TaskDetailsRepository(TaskManagerDbContext taskManagerDbContext)
        {
            _taskManagerDbContext = taskManagerDbContext;
        }

        public async Task Delete(Models.TaskDetailModel entity)
        {
            _taskManagerDbContext.Tasks.Remove(entity);

            await _taskManagerDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Models.TaskDetailModel>> GetAllTasks()
        {
            return await _taskManagerDbContext.Tasks.AsNoTracking().ToListAsync();
        }

        public async Task<Models.TaskDetailModel> Get(int id)
        {
            return await _taskManagerDbContext.Tasks.FirstOrDefaultAsync(s => s.Id.Equals(id));
        }

        public async Task<int> Insert(Models.TaskDetailModel entity)
        {
            _taskManagerDbContext.Tasks.Add(entity);

            return await _taskManagerDbContext.SaveChangesAsync();
        }

        public async Task Update(int id, Models.TaskDetailModel entity)
        {
            _taskManagerDbContext.Tasks.Update(entity);

            await _taskManagerDbContext.SaveChangesAsync();
        }
    }
}
