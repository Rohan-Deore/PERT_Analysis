using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PERT_Analyser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, Task> tasks = new Dictionary<string, Task>();

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
                foreach (string previousTaskName in previousTasksInput.Split(','))
                {
                    string trimmedName = previousTaskName.Trim();
                    if (tasks.ContainsKey(trimmedName))
                    {
                        task.AddPreviousTask(tasks[trimmedName]);
                    }
                    else
                    {
                        MessageBox.Show($"Task {trimmedName} does not exist.");
                    }
                }
            }

            tasks[name] = task;
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
            double value = double.Parse(parts[0]);
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
    }

    public class Task
    {
        public string Name { get; }
        public double Duration { get; }
        public List<Task> PreviousTasks { get; }
        public double EarliestStart { get; private set; }
        public double EarliestFinish { get; private set; }

        public Task(string name, double duration)
        {
            Name = name;
            Duration = duration;
            PreviousTasks = new List<Task>();
            EarliestStart = 0;
            EarliestFinish = 0;
        }

        public override string ToString()
        {
            string prevTasks = "[";
            foreach (Task task in PreviousTasks) 
            {
                prevTasks += task.Name;
                prevTasks += ", ";
            }
            
            prevTasks += "]";

            return $"{Name} {Duration} {prevTasks}";
        }

        public void AddPreviousTask(Task task)
        {
            PreviousTasks.Add(task);
        }

        public void CalculateEarliestTimes()
        {
            if (PreviousTasks.Count == 0)
            {
                EarliestStart = 0;
            }
            else
            {
                EarliestStart = Math.Max(EarliestStart, PreviousTasks.Max(task => task.EarliestFinish));
            }
            EarliestFinish = EarliestStart + Duration;
        }
    }
}