using System;
using System.Collections.Generic;

class Task
{
    public string Name { get; }
    public int Duration { get; }
    public List<Task> PreviousTasks { get; }
    public int EarliestStart { get; private set; }
    public int EarliestFinish { get; private set; }

    public Task(string name, int duration)
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

            Console.Write($"Enter duration for task {name}: ");
            int duration = int.Parse(Console.ReadLine());
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
            Console.WriteLine($"Task {task.Name}: Earliest Start = {task.EarliestStart}, Earliest Finish = {task.EarliestFinish}");
        }

        Console.ReadKey();
    }
}
