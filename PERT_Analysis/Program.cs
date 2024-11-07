using System;
using System.Collections.Generic;

class Task
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

class Program
{
    static void Main(string[] args)
    {
        var tasks = new Dictionary<string, Task>();

        while (true)
        {
            Console.Write("Enter task name (or 'done' to finish): ");
            var name = Console.ReadLine();
            if (name.ToLower() == "done")
                break;

            Console.Write($"Enter duration for task {name} (e.g., '5 days' or '8 hrs'): ");
            var durationInput = Console.ReadLine();
            double duration = ParseDuration(durationInput);
            var task = new Task(name, duration);

            Console.Write($"Enter names of previous tasks for {name} (comma-separated, or 'none'): ");
            var previousTasks = Console.ReadLine();
            if (previousTasks.ToLower() != "none")
            {
                foreach (var previousTaskName in previousTasks.Split(','))
                {
                    var trimmedName = previousTaskName.Trim();
                    if (tasks.ContainsKey(trimmedName))
                    {
                        task.AddPreviousTask(tasks[trimmedName]);
                    }
                    else
                    {
                        Console.WriteLine($"Task {trimmedName} does not exist.");
                    }
                }
            }

            tasks[name] = task;
        }

        foreach (var task in tasks.Values)
        {
            task.CalculateEarliestTimes();
            Console.WriteLine($"Task {task.Name}: Earliest Start = {task.EarliestStart}, Earliest Finish = {task.EarliestFinish} hrs");
        }
    }

    static double ParseDuration(string input)
    {
        var parts = input.Split(' ');
        double value = 0;
        if (!double.TryParse(parts[0], out value))
        {
            Console.WriteLine("parsing failed!!");
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
}