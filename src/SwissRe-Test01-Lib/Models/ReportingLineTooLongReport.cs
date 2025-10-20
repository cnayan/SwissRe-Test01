using System.Diagnostics.CodeAnalysis;

namespace SwissRe_Test01_Lib.Models;

[ExcludeFromCodeCoverage]
public sealed record ReportingLineTooLongReport
{
    public int? EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required int Depth { get; set; }
    public required int ExcessDepth { get; set; }
}
