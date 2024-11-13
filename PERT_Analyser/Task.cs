using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERT_Analyser
{
    public class Task
    {
        private static int taskCounter = 1;
        public int Id { get; }
        public string Name { get; }
        public double Duration { get; }
        public List<int> PreviousTasks { get; }
        public double EarliestStart { get; private set; }
        public double EarliestFinish { get; private set; }

        public Task(string name, double duration)
        {
            Id = taskCounter++;
            Name = name;
            Duration = duration;
            PreviousTasks = new List<int>();
            EarliestStart = 0;
            EarliestFinish = 0;
        }

        public override string ToString()
        {
            string prevTasks = "[";
            foreach (int taskID in PreviousTasks)
            {
                prevTasks += taskID;
                prevTasks += ", ";
            }

            prevTasks += "]";

            return $"{Id}\t{Name}\t{Duration}\t{prevTasks}";
        }

        public void AddPreviousTask(Task task)
        {
            PreviousTasks.Add(task.Id);
        }

        public void CalculateEarliestTimes(List<Task> prevTasks)
        {
            if (PreviousTasks.Count == 0)
            {
                EarliestStart = 0;
            }
            else
            {
                EarliestStart = Math.Max(EarliestStart, prevTasks.Max(task => task.EarliestFinish));
            }

            EarliestFinish = EarliestStart + Duration;
        }
    }
}
