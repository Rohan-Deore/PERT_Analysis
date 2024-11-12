﻿using NLog;
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
            double duration = 0;
            if (!ParseDuration(TaskDuration.Text, out duration))
            {
                MessageBox.Show("Duration is not valid.");
                return;
            }

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
                        var message = $"Task {taskId} does not exist.";
                        logger.Error(message);
                        MessageBox.Show(message);
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

        private bool ParseDuration(string input, out double duration)
        {
            bool status = false;

            string[] parts = input.Split(' ');

            if (!double.TryParse(parts[0], out duration))
            {
                logger.Warn("Invalid duration");
                return status;
            }

            string unit = parts[1].ToLower();

            if (unit == "days" || unit == "day")
            {
                duration = duration * 24;
                status = true;
                return status; // Convert days to hours
            }
            else if (unit == "hrs" || unit == "hr" || unit == "hours" || unit == "hour")
            {
                status = true;
                return status;
            }
            else
            {
                logger.Error("Invalid duration format");
                throw new ArgumentException("Invalid duration format");
            }

            return status;
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