namespace Direct.Core.FakeData;

public sealed record FakeData
{
    public string Field1 { get; init; } = string.Empty;
    public string Field2 { get; init; } = string.Empty;
    public decimal Field3 { get; init; }
    public int Field4 { get; init; }
    public bool Field5 { get; init; }

    public override string ToString()
    {
        return $"Field1: {Field1}, Field2: {Field2}, Field3: {Field3}, Field4: {Field4}, Field5: {Field5}";
    }
}
