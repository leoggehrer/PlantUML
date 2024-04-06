namespace PlantUML.ConApp
{
    /// <summary>
    /// Represents the type of diagram.
    /// </summary>
    public enum DiagramBuilderType
    {
        All = Activity + Class + Sequence,
        Activity = 1,
        Class = 2,
        Sequence = 4,
    }
}
