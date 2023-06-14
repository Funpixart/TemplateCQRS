using TemplateCQRS.Domain.Common;

namespace TemplateCQRS.Application.Enums;

public enum StatusEnum
{
    [MultipleDescription("A new operation has started", Language.EN)]
    [MultipleDescription("Una nueva operación ha empezado", Language.ES)]
    New = 0,

    [MultipleDescription("When an operation is pending to be process", Language.EN)]
    [MultipleDescription("Cuando una operación está pendiente de ser procesada", Language.ES)]
    Pending = 1,

    [MultipleDescription("When an operation or process is in progress", Language.EN)]
    [MultipleDescription("Cuando una operación o proceso está en progreso", Language.ES)]
    InProgress = 2,

    [MultipleDescription("When an operation or process has completed", Language.EN)]
    [MultipleDescription("Cuando una operación o proceso se ha completado", Language.ES)]
    Completed = 3,

    [MultipleDescription("When an operation or process has been cancelled", Language.EN)]
    [MultipleDescription("Cuando una operación o proceso ha sido cancelada", Language.ES)]
    Cancelled = 4,

    [MultipleDescription("When an operation or process has failed", Language.EN)]
    [MultipleDescription("Cuando una operación o proceso ha fallado", Language.ES)]
    Failed = 5,
}