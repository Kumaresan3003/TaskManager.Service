namespace TaskManager.Service.Models
{
    using System;

    /// <summary>
    /// TaskDetailModelClass.
    /// </summary>
    public class TaskDetailModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

        public int? ParentTaskId { get; set; }

        public bool EndTask { get; set; }
    }
}
