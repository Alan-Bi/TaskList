using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using TaskList;

public class TableSource : UITableViewSource
{
	

	List<TaskObject> TableItems;
	string CellIdentifier = "TableCell";

	public TableSource(List<TaskObject> tasks)
	{
		TableItems = tasks;
	}

	public override nint RowsInSection(UITableView tableview, nint section)
	{
		return TableItems.Count;
	}

	public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
	{
		UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
		string taskDescription = TableItems[indexPath.Row].Text.Substring("Current Task \n \n".Length);
		string temp = TableItems[indexPath.Row].date.ToString();
		string taskDate = temp.Substring(0, temp.IndexOf(' '));

		if (cell == null)
		{
			cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier);
		}

		cell.DetailTextLabel.Text = taskDate;
		cell.DetailTextLabel.TextColor = UIColor.DarkGray;

		//DateTime createdDate;

		/*if (taskDate.Length == 10)
		{
			createdDate = DateTime.ParseExact(taskDate, "MM/dd/yyyy", null);
		}
		else if (taskDate.Length == 8)
		{
			createdDate = DateTime.ParseExact(taskDate, "M/d/yyyy", null);
		}
		else
		{
			if (taskDate[1] == '/')
			{
				createdDate = DateTime.ParseExact(taskDate, "M/dd/yyyy", null);
			}
			else
			{
				createdDate = DateTime.ParseExact(taskDate, "MM/d/yyyy", null);
			}
		}*/

		int diff = (int) (DateTime.Today - TableItems[indexPath.Row].date).TotalDays;

		if (diff >= 30)
		{
			cell.DetailTextLabel.TextColor = UIColor.Red;
		}
		else if (diff >= 10)
		{
			cell.DetailTextLabel.TextColor = UIColor.Orange;
		}


		cell.TextLabel.Text = taskDescription;
		cell.TextLabel.Font = UIFont.SystemFontOfSize(22);
		cell.TextLabel.TextColor = UIColor.FromRGB(45, 145, 213);
		cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
		cell.TextLabel.Lines = 5;
		cell.SelectionStyle = UITableViewCellSelectionStyle.None;

		return cell;
	}

}