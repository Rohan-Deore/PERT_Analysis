using NLog;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PERT_Analyser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<int, Task> tasks = new Dictionary<int, Task>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string name = TaskName.Text;
            double duration = ParseDuration(TaskDuration.Text);
            Task task = new Task(name, duration);

            string previousTasksInput = PreviousTasks.Text;
            if (previousTasksInput.ToLower() != "none" || !string.IsNullOrEmpty(previousTasksInput))
            {
                foreach (string previousTaskId in previousTasksInput.Split(','))
                {
                    string taskIdStr = previousTaskId.Trim();
                    
                    if (string.IsNullOrEmpty(taskIdStr))
                    {
                        logger.Warn("Task ID list is empty.");
                        continue;
                    }

                    int taskId = 0;
                    if (!int.TryParse(taskIdStr, out taskId))
                    {
                        logger.Warn("Task ID is invalid.");
                        continue;
                    }

                    if (tasks.ContainsKey(taskId))
                    {
                        task.AddPreviousTask(tasks[taskId]);
                    }
                    else
                    {
                        MessageBox.Show($"Task {taskId} does not exist.");
                    }
                }
            }

            tasks[task.Id] = task;
            Data.Items.Add(task.ToString());
            TaskName.Clear();
            TaskDuration.Clear();
            PreviousTasks.Clear();
        }

        private void CalculatePERT_Click(object sender, RoutedEventArgs e)
        {
            Results.Items.Clear();

            foreach (Task task in tasks.Values)
            {
                task.CalculateEarliestTimes();
                Results.Items.Add($"Task {task.Name}: Earliest Start = {task.EarliestStart}, Earliest Finish = {task.EarliestFinish} hrs");
            }
        }

        private double ParseDuration(string input)
        {
            string[] parts = input.Split(' ');
            double value = 0;

            if (!double.TryParse(parts[0], out value))
            {
                Console.WriteLine("Invalid duration");
                return 0;
            }

            string unit = parts[1].ToLower();

            if (unit == "days" || unit == "day")
            {
                return value * 24; // Convert days to hours
            }
            else if (unit == "hrs" || unit == "hr" || unit == "hours" || unit == "hour")
            {
                return value;
            }
            else
            {
                throw new ArgumentException("Invalid duration format");
            }
        }

        private void Data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskName.Clear();
            TaskDuration.Clear();
            PreviousTasks.Clear();
        }

        private void Data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var data = ((sender as ListBox).SelectedValue as string);
            var splitData = data.Split(' ');
            TaskName.Text = splitData[0];
            TaskDuration.Text = splitData[1];
            PreviousTasks.Text = splitData[2].Trim('[', ']');
        }

        private void TaskDuration_LostFocus(object sender, RoutedEventArgs e)
        {
            string[] parts = TaskDuration.Text.Split(' ');
            double value = 0;

            if (double.TryParse(parts[0], out value))
            {
                TaskDuration.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            else
            { 
                TaskDuration.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Data.Items.Add("Id\tName\tDuration\tPrevious task Ids");
        }
    }
}