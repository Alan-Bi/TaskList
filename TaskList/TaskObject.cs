using System;
using SQLite;

namespace TaskList
{
	[Table("tasks")]
	public class TaskObject
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string Text { get; set; }
		public Boolean isDeleted { get; set; }
		public DateTime date { get; set; }

		public TaskObject (string text)
		{
			Text = text;
			isDeleted = false;
			date = DateTime.Now.ToLocalTime();
		}
		public TaskObject ()
		{
			
		}
	}
}

