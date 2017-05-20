using System;
using CoreGraphics;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using SQLite;
using System.IO;
using Foundation;

using UIKit;

namespace TaskList
{
	public partial class ViewController : UIViewController
	{
		public LinkedList<UITextView> taskList = new LinkedList<UITextView>();

		public UITextView noTasks = new UITextView(new RectangleF(5, 130 + 60, (float) UIScreen.MainScreen.Bounds.Width - 10, 
			(float) UIScreen.MainScreen.Bounds.Height - 195)){
			Editable = false
		};

		public UIButton deleteTask = new UIButton (UIButtonType.System);

		public UITextField newTaskText = new UITextField(new RectangleF(0, 50, (float) UIScreen.MainScreen.Bounds.Width - 75, 35 + 5));

		public UIButton addTaskButton = UIButton.FromType (UIButtonType.System);

		public UIButton undoButton = UIButton.FromType (UIButtonType.System);

		public UITextView belowTaskText = new UITextView (new RectangleF(0, 85 + 5, (float) UIScreen.MainScreen.Bounds.Width - 75, 40));

		public TaskObject lastTask;

		public UITextView totalTasks = new UITextView (new RectangleF ((float)UIScreen.MainScreen.Bounds.Width - 67, 
			90, 67, 22));

		public UITextView dateText = new UITextView (new RectangleF ((float)UIScreen.MainScreen.Bounds.Width - 89,
			(float) UIScreen.MainScreen.Bounds.Height - 31, 83, 22));

		public UITextView lines = new UITextView(new RectangleF(60, 240, (float) UIScreen.MainScreen.Bounds.Width - 120, 1));

		public Timer t;// = new Timer (2000);


		//COLOR CONTROL
		public UIColor colorOutline = UIColor.FromRGB(52, 152, 220);
		public UIColor colorText = UIColor.FromRGB(45, 145, 213);
		public UIColor colorFill = UIColor.White;//FromRGB(240, 240, 240);
		public UIColor colorBackground = UIColor.White;//FromRGB(210, 210,  210);
		public UIColor colorBottomBack = UIColor.FromRGB (210, 210, 210);

		public UIColor colorHeaderBar = UIColor.White;//FromRGB(230, 230, 230);



		public int taskCount;
		public UITextView textView;
		public int firstID;


		public int undoButtonCount;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//Database location?
			var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "taskListDB");
			//Create database?
			var db = new SQLiteConnection(dbPath);
			
			//Create table
			db.CreateTable<TaskObject> ();
			Console.WriteLine("REACHED");
			undoButtonCount = 0;


			//Customize all displays and stuff 
			t = new Timer (5000);

			UITextView bottomBack = new UITextView(new CGRect(0, 200 - 20, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - 200 + 20));
			bottomBack.Editable = false;
			bottomBack.Selectable = false;
			bottomBack.BackgroundColor = colorBottomBack;
			View.AddSubview (bottomBack);


			UITextView background = new UITextView(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height));//200 + 50));//UIScreen.MainScreen.Bounds.Height));
			background.Editable = false;
			background.Selectable = false;
			//background.Layer.BorderWidth = 1;
			//background.Layer.CornerRadius = 4;
			//background.Layer.BorderColor = colorOutline.CGColor;
			background.BackgroundColor = colorBackground;
			View.AddSubview (background);

			UINavigationBar bar = new UINavigationBar (new CGRect (0, 0, UIScreen.MainScreen.Bounds.Width, 50));
			View.AddSubview (bar);
			this.NavigationItem.Title = "TaskList";
			bar.BarTintColor = colorHeaderBar;


			UITextView barText = new UITextView(new RectangleF(0, 13, (float) UIScreen.MainScreen.Bounds.Width, 37));
			barText.Text = "TaskList";
			barText.TextAlignment = UITextAlignment.Center;
			barText.Font = UIFont.SystemFontOfSize(18);
			barText.TextColor = UIColor.FromRGB(32, 132, 200);
			barText.Editable = false;
			barText.Selectable = false;
			View.AddSubview (barText);



			//noTasks
			noTasks.Text = "No Tasks";
			noTasks.TextAlignment = UITextAlignment.Center;
			noTasks.Font = UIFont.SystemFontOfSize (24);
			//noTasks.Hidden = true;
			noTasks.Layer.BorderWidth = 1;
			noTasks.Layer.CornerRadius = 5;
			noTasks.Layer.BorderColor = colorOutline.CGColor;
			noTasks.TextColor = colorText;
			noTasks.BackgroundColor = UIColor.White;//FromRGB(245, 245, 245);
			View.AddSubview (noTasks);


			//newTaskText
			newTaskText.BorderStyle = UITextBorderStyle.RoundedRect;
			newTaskText.Placeholder = "What is your new Task?";
			newTaskText.Layer.BorderWidth = 1;
			newTaskText.Layer.CornerRadius = 5;
			newTaskText.Layer.BorderColor = colorOutline.CGColor;
			newTaskText.BackgroundColor = UIColor.White;//colorFill;
			View.AddSubview (newTaskText);

			//totalTask
			totalTasks.Font = UIFont.SystemFontOfSize(13);
			totalTasks.Editable = false;
			totalTasks.TextAlignment = UITextAlignment.Right;
			totalTasks.TextColor = colorText;//UIColor.FromRGB(0,128,255);
			totalTasks.BackgroundColor = colorBackground;
			View.AddSubview(totalTasks);

			//addTaskButton
			addTaskButton.SetTitle ("Add", UIControlState.Normal);
			addTaskButton.Frame = new RectangleF ((float) UIScreen.MainScreen.Bounds.Width - 67 + 2, 50, 59 + 6, 40);
			addTaskButton.Font = UIFont.SystemFontOfSize(16);
			addTaskButton.Layer.BorderWidth = 1;
			addTaskButton.Layer.CornerRadius = 5;
			addTaskButton.Layer.BorderColor = colorOutline.CGColor;
			addTaskButton.SetTitleColor(UIColor.White, UIControlState.Normal);//colorText, UIControlState.Normal);
			addTaskButton.BackgroundColor = UIColor.FromRGB(52 + 10, 152 + 10, 220 + 10);//colorFill;
			View.AddSubview (addTaskButton);


			//deleteTask
			deleteTask.SetTitle ("Next Task", UIControlState.Normal);
			deleteTask.Frame = (new RectangleF ((float)UIScreen.MainScreen.Bounds.Width - 140, 150, 
				/*(float) UIScreen.MainScreen.Bounds.Width -*/ 135, 35));
			deleteTask.Font = UIFont.SystemFontOfSize (18);
			deleteTask.Layer.BorderWidth = 1;
			deleteTask.Layer.CornerRadius = 5;
			deleteTask.Layer.BorderColor = colorOutline.CGColor;
			deleteTask.SetTitleColor(UIColor.White, UIControlState.Normal);
			deleteTask.BackgroundColor = UIColor.FromRGB(52 + 10, 152 + 10, 220 + 10);//colorFill;
			View.AddSubview (deleteTask);


			//belowTaskText
			belowTaskText.Font = UIFont.SystemFontOfSize (12);
			belowTaskText.Editable = false;
			belowTaskText.Hidden = true;
			belowTaskText.TextAlignment = UITextAlignment.Center;
			belowTaskText.BackgroundColor = colorBackground;
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

			//dateText
			dateText.Font = UIFont.SystemFontOfSize(13);
			dateText.Editable = false;
			dateText.TextAlignment = UITextAlignment.Right;
			dateText.TextColor = colorText;//UIColor.FromRGB(0,128,255);
			dateText.BackgroundColor = colorBackground;
			View.AddSubview(dateText);


			UpdateScreen ();


			//Code for interaction with user

			//Code/visuals for one task
			textView = new UITextView(new RectangleF(5, 130 + 60, (float) UIScreen.MainScreen.Bounds.Width - 10, 
				(float) UIScreen.MainScreen.Bounds.Height - 195));
			textView.Text = "";
			textView.TextAlignment = UITextAlignment.Center;
			textView.Font = UIFont.SystemFontOfSize(24);
			textView.Editable = false;
			textView.Layer.BorderWidth = 1;
			textView.Layer.CornerRadius = 5;
			textView.Layer.BorderColor = colorOutline.CGColor;
			textView.TextColor = colorText;
			textView.BackgroundColor = UIColor.White;//FromRGB(245, 245, 245);//colorFill;
			//View.AddSubview (textView); 



			//Find database size
			var firstRow = db.Query<TaskObject> ("SELECT ID, isDeleted, Text FROM tasks WHERE NOT isDeleted ORDER BY ID ASC LIMIT 1");// ORDER BY ID ASC LIMIT 1");
			var lastRow = db.Query<TaskObject> ("SELECT ID FROM tasks WHERE NOT isDeleted ORDER BY ID DESC LIMIT 1");// ORDER BY ID ASC LIMIT 1");
			firstID = 0;
			int lastID = -1;

			//db.DeleteAll<TaskObject> ();

			foreach (var a in firstRow) {
				//if (!a.isDeleted) {
					Console.WriteLine ("Data: " + a.ID + a.Text);
					firstID = a.ID;
					//break;
				//}
			}
			//if (firstID != 0) {
				foreach (var a in lastRow) {
					Console.WriteLine ("Data: " + a.ID + a.Text);
					lastID = a.ID;
				}
			//}


			taskCount = lastID - firstID + 1;
			//db.Delete<TaskObject> (1);


			if (taskCount > 0) {
				textView.Text = db.Get<TaskObject> (firstID).Text;
				View.AddSubview (textView);
				dateText.Text = db.Get<TaskObject>(firstID).date.ToString();
				dateText.Text.Remove(dateText.Text.IndexOf(' '));
			}
			totalTasks.Text = "Tasks: " + taskCount;
			UpdateScreen();
			View.AddSubview(dateText);

			addTaskButton.TouchUpInside += async (sender, e) => {
				if(newTaskText.Text != "" && taskCount < 15)
				{
					db.Insert(new TaskObject("Current Task \n \n" + newTaskText.Text));
					if(taskCount == 0)
					{
						firstRow = db.Query<TaskObject> ("SELECT ID FROM tasks WHERE NOT isDeleted ORDER BY ID ASC LIMIT 1");// ORDER BY ID ASC LIMIT 1");
						foreach (var a in firstRow) {
							Console.WriteLine ("Data: " + a.ID + a.Text);
							firstID = a.ID;
							textView.Text = db.Get<TaskObject> (firstID).Text;
						}
						dateText.Text = db.Get<TaskObject>(firstID).date.ToString();
						dateText.Text.Remove(dateText.Text.IndexOf(' '));
						View.AddSubview(dateText);
					}


					Console.WriteLine(db.Get<TaskObject>(firstID).date);

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
			//Below event might not be necessary
			newTaskText.EditingDidBegin += delegate {
				newTaskText.ResignFirstResponder();
			};

			//Remaining views cause keyboard to go down when touched
			UITapGestureRecognizer tapGesture = new UITapGestureRecognizer (TextViewTap);
			UITapGestureRecognizer tapGesture1 = new UITapGestureRecognizer (TextViewTap);
			UITapGestureRecognizer tapGesture2 = new UITapGestureRecognizer (TextViewTap);

			textView.AddGestureRecognizer (tapGesture);
			noTasks.AddGestureRecognizer (tapGesture1);
			background.AddGestureRecognizer (tapGesture2);




			t.Elapsed += (sender, e) => //TimedMessage; 
			{
				Console.WriteLine("Reached");

				InvokeOnMainThread(() => {
					belowTaskText.Hidden = true;
					belowTaskText.TextColor = UIColor.Black;
					t.Stop();
				});
			};


			deleteTask.TouchUpInside += async (sender, e) => {
				if(taskCount > 0)
				{
					//db.Delete<TaskObject>(firstID);
					//db.Get<TaskObject>(firstID).isDeleted = true;
					db.Execute("UPDATE tasks SET isDeleted = 1 WHERE ID = " + firstID);
					//db.Execute("UPDATE tasks SET isDeleted = 1 WHERE ID = 'firstID'");

					Console.WriteLine("deleted: " + db.Get<TaskObject>(firstID).isDeleted);
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

			undoButton.TouchUpInside += async (sender, e) => {
				//taskList.AddFirst (lastTask);
				if(taskCount < 15 && firstID > 1 && undoButtonCount > 0)
				{
					firstID--;
					//db.Get<TaskObject>(firstID).isDeleted = false;
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
				

		}

		public void UpdateScreen()
		{
			//Display on screen
			totalTasks.Text = "Tasks: " + taskCount;
			View.AddSubview (totalTasks);

			if (taskCount > 0) {
				//textView.Text =
				View.AddSubview (textView);
				View.BringSubviewToFront(dateText);
			}

			else {
				//noTasks.Hidden = false;
				View.BringSubviewToFront(noTasks);
			}


			if (taskCount >= 15)
				undoButton.Hidden = true;

			View.BringSubviewToFront(lines);
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			newTaskText.ResignFirstResponder();
		}

		public void TextViewTap()
		{
			newTaskText.ResignFirstResponder ();
		}
		/*public void TimedMessage (Object o, EventArgs e)
		{
			//Task.Delay(2000).Wait();
			//belowTaskText.Text = "";
			//View.AddSubview (belowTaskText);
			Console.WriteLine("REACHED HERE");
			belowTaskText.Hidden = true;
			t.Stop ();

		}*/

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}

}

