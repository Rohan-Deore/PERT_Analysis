class Task:
    def __init__(self, name, duration):
        self.name = name
        self.duration = duration
        self.previous_tasks = []
        self.earliest_start = 0
        self.earliest_finish = 0

    def add_previous_task(self, task):
        self.previous_tasks.append(task)

    def calculate_earliest_times(self):
        if not self.previous_tasks:
            self.earliest_start = 0
        else:
            self.earliest_start = max(task.earliest_finish for task in self.previous_tasks)
        
        self.earliest_finish = self.earliest_start + self.duration

def calculate_pert(tasks):
    for task in tasks:
        task.calculate_earliest_times()
    return tasks

def main():
    tasks = {}

    while True:
        name = input("Enter task name (or 'done' to finish): ")
        if name.lower() == 'done':
            break

        duration = int(input(f"Enter duration for task {name}: "))
        task = Task(name, duration)

        previous_tasks = input(f"Enter names of previous tasks for {name} (comma-separated, or 'none'): ")
        if previous_tasks.lower() != 'none':
            for previous_task in previous_tasks.split(','):
                previous_task_name = previous_task.strip()
                if previous_task_name in tasks:
                    task.add_previous_task(tasks[previous_task_name])
                else:
                    print(f"Task {previous_task_name} does not exist.")

        tasks[name] = task

    tasks_list = list(tasks.values())
    tasks_list = calculate_pert(tasks_list)

    for task in tasks_list:
        print(f"Task {task.name}: Earliest Start = {task.earliest_start}, Earliest Finish = {task.earliest_finish}")

if __name__ == "__main__":
    main()
