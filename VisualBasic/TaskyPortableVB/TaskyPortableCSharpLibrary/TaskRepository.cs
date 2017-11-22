using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Linq.Expressions;

namespace Tasky.Portable {
	/// <summary>
	/// The repository is responsible for providing an abstraction to actual data storage mechanism
	/// whether it be SQLite, XML or some other method
	/// </summary>
	public class TaskRepository {
		static string storeLocation;	
		static List<Task> tasks;
		IXmlStorage storage;

		public TaskRepository (string filename, IXmlStorage xmlStorage)
		{
			// set the db location
			storeLocation = filename;
			storage = xmlStorage;
			// deserialize XML from file at dbLocation
			tasks = storage.ReadXml (filename);
		}

		public Task GetTask(int id)
		{
			for (var t = 0; t< tasks.Count; t++) {
				if (tasks[t].ID == id)
					return tasks[t];
			}
			return new Task() {ID=id};
		}
		
		public IEnumerable<Task> GetTasks ()
		{
			return tasks;
		}

		/// <summary>
		/// Insert or update a task
		/// </summary>
		public int SaveTask (Task item)
		{ 
			var max = 0;
			if (tasks.Count > 0) 
				max = tasks.Max(x => x.ID);

			if (item.ID == 0) {
				item.ID = ++max;
				tasks.Add (item);
			} else {
				//HACK: why isn't Find available in PCL ?
				//var i = tasks.Find (x => x.ID == item.ID); 
				var j = tasks.Where (x => x.ID == item.ID).First();
				j = item; // replaces item in collection with updated value
			}

			storage.WriteXml (tasks, storeLocation);
			return max;
		}
		
		public int DeleteTask(int id)
		{
			for (var t = 0; t< tasks.Count; t++) {
				if (tasks[t].ID == id){
					tasks.RemoveAt (t);
					storage.WriteXml (tasks, storeLocation);
					return 1;
				}
			}
			
			return -1;
		}
	}
}