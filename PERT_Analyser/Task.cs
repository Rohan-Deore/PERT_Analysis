using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERT_Analyser
{
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
