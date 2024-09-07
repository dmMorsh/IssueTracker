namespace IssueTracker.Dal.Models;

public enum typeOfIssue
{
    story,
    task,
    bug,
    epic
}

public enum statusOfTask
{
    todo,
    inProgress,
    done
}

public enum priorityOfTask
{
    high,
    medium,
    low
}