using System;
using CoreGraphics;
using System.Drawing;
using System.Timers;
using SQLite;
using System.IO;
using Foundation;

using UIKit;

namespace TaskList
{
	public partial class ViewController : UIViewController
	{
		public UITextView noTasks = new UITextView(new RectangleF(5, 190, (float) UIScreen.MainScreen.Bounds.Width - 10, 
			(float) UIScreen.MainScreen.Bounds.Height - 195)){
			Editable = false
		};

		public UIButton deleteTask = new UIButton (UIButtonType.System);

		public UITextField newTaskText = new UITextField(new RectangleF(0, 50, (float) UIScreen.MainScreen.Bounds.Width - 75, 40));

		public UIButton addTaskButton = UIButton.FromType (UIButtonType.System);

		public UIButton undoButton = UIButton.FromType (UIButtonType.System);

		public UITextView belowTaskText = new UITextView (new RectangleF(0, 90, (float) UIScreen.MainScreen.Bounds.Width - 75, 40));

		public TaskObject lastTask;

		public UITextView totalTasks = new UITextView (new RectangleF ((float)UIScreen.MainScreen.Bounds.Width - 67, 
			90, 67, 22));

		public UITextView dateText = new UITextView (new RectangleF ((float)UIScreen.MainScreen.Bounds.Width - 89,
			(float) UIScreen.MainScreen.Bounds.Height - 31, 83, 22));

		public UITextView lines = new UITextView(new RectangleF(60, 240, (float) UIScreen.MainScreen.Bounds.Width - 120, 1));

		UITextView lines1 = new UITextView(new RectangleF(0, 50, (float)UIScreen.MainScreen.Bounds.Width, 1));

		UITableView taskTableView = new UITableView(new RectangleF(0, 51, (float)UIScreen.MainScreen.Bounds.Width,
			(float)UIScreen.MainScreen.Bounds.Height - 50));

		UIButton exitTableView = UIButton.FromType(UIButtonType.System);

		UIButton loadAllTasks = UIButton.FromType(UIButtonType.System);

		UINavigationBar bar = new UINavigationBar(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 50));

		UITextView barText = new UITextView(new RectangleF(0, 13, (float)UIScreen.MainScreen.Bounds.Width, 37));


		//COLOR CONTROL
		public UIColor colorOutline = UIColor.FromRGB(52, 152, 220);
		public UIColor colorText = UIColor.FromRGB(45, 145, 213);


		public int taskCount;
		public int firstID;
		public int undoButtonCount;

		public Timer t;

		public UITextView textView;

		public UIViewController tableViewController;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "taskListDB");
			var db = new SQLiteConnection(dbPath);
			
			//Create table
			db.CreateTable<TaskObject> ();
			undoButtonCount = 0;


			t = new Timer (5000);


			exitTableView.SetTitle ("Close", UIControlState.Normal);
			exitTableView.Frame = new RectangleF((float) UIScreen.MainScreen.Bounds.Width - 55, 17, 50, 30);
			exitTableView.Font = UIFont.SystemFontOfSize(16);
			exitTableView.SetTitleColor(colorText, UIControlState.Normal);

			loadAllTasks.SetTitle ("Show All", UIControlState.Normal);
			loadAllTasks.Frame = new RectangleF((float) UIScreen.MainScreen.Bounds.Width / 2 - 50,
                (float) UIScreen.MainScreen.Bounds.Height - 35, 100, 30);
			loadAllTasks.Font = UIFont.SystemFontOfSize(16);
			loadAllTasks.SetTitleColor(colorText, UIControlState.Normal);
			View.AddSubview (loadAllTasks);


			barText.Text = "TaskList";
			barText.TextAlignment = UITextAlignment.Center;
			barText.Font = UIFont.SystemFontOfSize(18);
			barText.TextColor = UIColor.FromRGB(32, 132, 200);
			barText.Editable = false;
			barText.Selectable = false;
			View.AddSubview (bar);
			View.AddSubview (barText);


			//noTasks
			noTasks.Text = "No Tasks";
			noTasks.TextAlignment = UITextAlignment.Center;
			noTasks.Font = UIFont.SystemFontOfSize (24);
			noTasks.Layer.BorderWidth = 1;
			noTasks.Layer.CornerRadius = 5;
			noTasks.Layer.BorderColor = colorOutline.CGColor;
			noTasks.TextColor = colorText;
			noTasks.BackgroundColor = UIColor.White;
			View.AddSubview (noTasks);

			//newTaskText
			newTaskText.BorderStyle = UITextBorderStyle.RoundedRect;
			newTaskText.Placeholder = "What is your new Task?";
			newTaskText.Layer.BorderWidth = 1;
			newTaskText.Layer.CornerRadius = 5;
			newTaskText.Layer.BorderColor = colorOutline.CGColor;
			newTaskText.BackgroundColor = UIColor.White;
			View.AddSubview (newTaskText);

			//totalTask
			totalTasks.Font = UIFont.SystemFontOfSize(13);
			totalTasks.Editable = false;
			totalTasks.TextAlignment = UITextAlignment.Right;
			totalTasks.TextColor = colorText;
			View.AddSubview(totalTasks);

			//addTaskButton
			addTaskButton.SetTitle ("Add", UIControlState.Normal);
			addTaskButton.Frame = new RectangleF ((float) UIScreen.MainScreen.Bounds.Width - 67 + 2, 50, 59 + 6, 40);
			addTaskButton.Font = UIFont.SystemFontOfSize(16);
			addTaskButton.Layer.BorderWidth = 1;
			addTaskButton.Layer.CornerRadius = 5;
			addTaskButton.Layer.BorderColor = colorOutline.CGColor;
			addTaskButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			addTaskButton.BackgroundColor = UIColor.FromRGB(52 + 10, 152 + 10, 220 + 10);
			View.AddSubview (addTaskButton);

			//deleteTask
			deleteTask.SetTitle ("Next Task", UIControlState.Normal);
			deleteTask.Frame = (new RectangleF ((float)UIScreen.MainScreen.Bounds.Width - 140, 150, 135, 35));
			deleteTask.Font = UIFont.SystemFontOfSize (18);
			deleteTask.Layer.BorderWidth = 1;
			deleteTask.Layer.CornerRadius = 5;
			deleteTask.Layer.BorderColor = colorOutline.CGColor;
			deleteTask.SetTitleColor(UIColor.White, UIControlState.Normal);
			deleteTask.BackgroundColor = UIColor.FromRGB(52 + 10, 152 + 10, 220 + 10);
			View.AddSubview (deleteTask);

			//belowTaskText
			belowTaskText.Font = UIFont.SystemFontOfSize (12);
			belowTaskText.Editable = false;
			belowTaskText.Hidden = true;
			belowTaskText.TextAlignment = UITextAlignment.Center;
			View.AddSubview (belowTaskText);

			//undoButton
			undoButton.SetTitle("Undo", UIControlState.Normal);
			undoButton.Frame = new RectangleF (0, 103 + 60, 65, 30);
			undoButton.Font = UIFont.SystemFontOfSize (16);
			undoButton.Hidden = true;
			undoButton.SetTitleColor (colorText, UIControlState.Normal);
			View.AddSubview (undoButton);

			//lines
			lines.Editable = false;
			lines.Selectable = false;
			lines.Layer.BorderWidth = 1;
			lines.Layer.CornerRadius = 1;
			lines.Layer.BorderColor = colorText.CGColor;
			View.AddSubview(lines);

			lines1.Editable = false;
			lines1.Selectable = false;
			lines1.Layer.BorderWidth = 1;
			lines1.Layer.CornerRadius = 1;
			lines1.Layer.BorderColor = colorText.CGColor;


			//dateText
			dateText.Font = UIFont.SystemFontOfSize(13);
			dateText.Editable = false;
			dateText.TextAlignment = UITextAlignment.Right;
			dateText.TextColor = colorText;
			View.AddSubview(dateText);


			//UpdateScreen ();


			//Code/visuals for one task
			textView = new UITextView(new RectangleF(5, 190, (float) UIScreen.MainScreen.Bounds.Width - 10, 
				(float) UIScreen.MainScreen.Bounds.Height - 195));
			textView.Text = "";
			textView.TextAlignment = UITextAlignment.Center;
			textView.Font = UIFont.SystemFontOfSize(24);
			textView.Editable = false;
			textView.Layer.BorderWidth = 1;
			textView.Layer.CornerRadius = 5;
			textView.Layer.BorderColor = colorOutline.CGColor;
			textView.TextColor = colorText;
			textView.BackgroundColor = UIColor.White;



			//Find database size
			var firstRow = db.Query<TaskObject> ("SELECT ID, isDeleted, Text FROM tasks WHERE NOT isDeleted ORDER BY ID ASC LIMIT 1");
			var lastRow = db.Query<TaskObject> ("SELECT ID FROM tasks WHERE NOT isDeleted ORDER BY ID DESC LIMIT 1");
			firstID = 0;
			int lastID = -1;


			foreach (var a in firstRow) 
			{
				firstID = a.ID;
			}
			foreach (var a in lastRow) 
			{
				lastID = a.ID;
			}

			taskCount = lastID - firstID + 1;


			if (taskCount > 0) 
			{
				textView.Text = db.Get<TaskObject> (firstID).Text;
				View.AddSubview (textView);
				dateText.Text = db.Get<TaskObject>(firstID).date.ToString();
				dateText.Text.Remove(dateText.Text.IndexOf(' '));
			}

			totalTasks.Text = "Tasks: " + taskCount;

			UpdateScreen();

			View.AddSubview(dateText);
			View.AddSubview (loadAllTasks);


			addTaskButton.TouchUpInside += async (sender, e) => 
			{
				if(newTaskText.Text != "" && taskCount < 15)
				{
					string text = newTaskText.Text;
					if (text.Length > 185)
					{
						text = text.Substring(0, 180) + "...";
					}

					db.Insert(new TaskObject("Current Task \n \n" + text));
					if(taskCount == 0)
					{
						firstRow = db.Query<TaskObject> ("SELECT ID FROM tasks WHERE NOT isDeleted ORDER BY ID ASC LIMIT 1");// ORDER BY ID ASC LIMIT 1");
						foreach (var a in firstRow) 
						{
							Console.WriteLine ("Data: " + a.ID + a.Text);
							firstID = a.ID;
							textView.Text = db.Get<TaskObject> (firstID).Text;
						}
						dateText.Text = db.Get<TaskObject>(firstID).date.ToString();
						dateText.Text.Remove(dateText.Text.IndexOf(' '));
						View.AddSubview(dateText);
					}

					taskCount++;

					if(firstID > 15)
					{
						db.Delete<TaskObject>(firstID - 15);
					}

					newTaskText.Text = "";
					newTaskText.ResignFirstResponder();

					UpdateScreen();
					View.AddSubview(dateText);

					belowTaskText.Text = "Successfully added.";
					belowTaskText.TextColor = UIColor.Black;
					belowTaskText.Hidden = false;

					t.Start();


				}
				else if(newTaskText.Text == "")
				{
					belowTaskText.Text = "Please enter your task above.";
					belowTaskText.TextColor = UIColor.Red;
					belowTaskText.Hidden = false;

					t.Start();
				}
				else
				{
					belowTaskText.Text = "You already have 15 tasks. Please finish a few before adding more.";
					belowTaskText.TextColor = UIColor.Red;
					belowTaskText.Hidden = false;

					t.Start();
				}



			};


			//When return key is tapped, keyboard goes down
			newTaskText.ShouldReturn = (sender) =>
			{
				sender.ResignFirstResponder();
				return false;
			};


			//Remaining views cause keyboard to go down when touched
			UITapGestureRecognizer tapGesture = new UITapGestureRecognizer (TextViewTap);
			UITapGestureRecognizer tapGesture1 = new UITapGestureRecognizer (TextViewTap);

			textView.AddGestureRecognizer (tapGesture);
			noTasks.AddGestureRecognizer (tapGesture1);


			t.Elapsed += (sender, e) => 
			{
				InvokeOnMainThread(() => {
					belowTaskText.Hidden = true;
					belowTaskText.TextColor = UIColor.Black;
					t.Stop();
				});
			};


			deleteTask.TouchUpInside += async (sender, e) => 
			{
				if(taskCount > 0)
				{
					db.Execute("UPDATE tasks SET isDeleted = 1 WHERE ID = " + firstID);

					firstID++;
					taskCount--;

					if (taskCount > 0)
					{
						textView.Text = db.Get<TaskObject>(firstID).Text;
						dateText.Text = db.Get<TaskObject>(firstID).date.ToString();
						dateText.Text.Remove(dateText.Text.IndexOf(' '));
					}

					undoButtonCount = Math.Min(undoButtonCount + 1, 15);
					undoButton.Hidden = false;
					View.AddSubview(dateText);
					UpdateScreen();
				}
			};

			undoButton.TouchUpInside += async (sender, e) => 
			{
				if(taskCount < 15 && firstID > 1 && undoButtonCount > 0)
				{
					firstID--;
					db.Execute("UPDATE tasks SET isDeleted = 0 WHERE ID = " + firstID);
					taskCount++;
					undoButtonCount--;
					textView.Text = db.Get<TaskObject>(firstID).Text;

				}
				if(undoButtonCount <= 0)
					undoButton.Hidden = true;

				dateText.Text = db.Get<TaskObject>(firstID).date.ToString();
				dateText.Text.Remove(dateText.Text.IndexOf(' '));

				UpdateScreen(); 
				View.AddSubview(dateText);
			};

			loadAllTasks.TouchUpInside += async(sender, e) =>
			{
				taskTableView.Source = new TableSource(db.Query<TaskObject>("SELECT Text, date FROM tasks WHERE NOT isDeleted"));
				taskTableView.RowHeight = UITableView.AutomaticDimension;
				taskTableView.EstimatedRowHeight = 75;

				View.AddSubview(taskTableView);
				View.AddSubview(exitTableView);

				barText.Text = taskCount + " Tasks";
				View.AddSubview(lines1);
			};

			exitTableView.TouchUpInside += async(sender, e) =>
			{
				taskTableView.RemoveFromSuperview();
				exitTableView.RemoveFromSuperview();
				lines1.RemoveFromSuperview();
				barText.Text = "TaskList";
			};

				

		}

		//Ensuring that the display gets properly updated every time something changes
		public void UpdateScreen()
		{
			//Display on screen
			totalTasks.Text = "Tasks: " + taskCount;
			View.AddSubview (totalTasks);

			if (taskCount > 0) 
			{
				View.AddSubview (textView);
				View.BringSubviewToFront(dateText);
			}

			else 
			{
				View.BringSubviewToFront(noTasks);
			}


			if (taskCount >= 15)
				undoButton.Hidden = true;

			View.BringSubviewToFront(lines);
			View.BringSubviewToFront(loadAllTasks);
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			newTaskText.ResignFirstResponder();
		}

		public void TextViewTap()
		{
			newTaskText.ResignFirstResponder ();
		}


		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}

}

