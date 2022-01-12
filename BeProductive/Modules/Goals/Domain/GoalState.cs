using NpgsqlTypes;

namespace BeProductive.Modules.Goals.Domain;

public enum GoalState
{
    Unknown,
    [PgName(nameof(Success))]
    Success,
    [PgName(nameof(Failure))]
    Failure,
    [PgName(nameof(Emergency))]
    Emergency,
    [PgName(nameof(NotApplicable))]
    NotApplicable,
}