namespace WorkQueue.Core.Data;

public sealed record FakeData
{
    public string Field1 { get; set; } = string.Empty;
    public int Field2 { get; set; }
    public decimal Field3 { get; set; }
    public bool Field4 { get; set; }
}