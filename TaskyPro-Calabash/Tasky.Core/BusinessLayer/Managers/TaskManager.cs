using System.Collections.Generic;

using Tasky.DAL;

namespace Tasky.BL.Managers
{
    public static class TaskManager
    {
        public static Task GetTask(int id)
        {
            return TaskRepository.GetTask(id);
        }

        public static IList<Task> GetTasks()
        {
            return new List<Task>(TaskRepository.GetTasks());
        }

        public static int SaveTask(Task item)
        {
            return TaskRepository.SaveTask(item);
        }

        public static int DeleteTask(int id)
        {
            return TaskRepository.DeleteTask(id);
        }
    }
}
