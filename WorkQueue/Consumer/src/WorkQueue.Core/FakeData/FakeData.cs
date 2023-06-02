namespace WorkQueue.Core.FakeData;

public sealed record FakeData
{
    public string Field1 { get; set; } = string.Empty;
    public int Field2 { get; set; }
    public decimal Field3 { get; set; }
    public bool Field4 { get; set; }

    public override string ToString()
    {
        return $"Field1: {Field1},Field2: {Field2},Field3: {Field3},Field4: {Field4},{DateTime.Now:O}";
    }
}