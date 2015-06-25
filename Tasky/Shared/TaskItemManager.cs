using System;
using System.Collections.Generic;
using Tasky.BL;

namespace Tasky.BL.Managers
{
	public static class TaskItemManager
	{
		static TaskItemManager ()
		{
		}
		
		public static TaskItem GetTask(int id)
		{
			return DAL.TaskItemRepository.GetTask(id);
		}
		
		public static IList<TaskItem> GetTasks ()
		{
			return new List<TaskItem>(DAL.TaskItemRepository.GetTasks());
		}
		
		public static int SaveTask (TaskItem item)
		{
			return DAL.TaskItemRepository.SaveTask(item);
		}
		
		public static int DeleteTask(int id)
		{
			return DAL.TaskItemRepository.DeleteTask(id);
		}
		
	}
}