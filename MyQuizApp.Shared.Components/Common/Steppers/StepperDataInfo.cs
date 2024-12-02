namespace MyQuizApp.Shared.Components.Common.Steppers;

public class StepperDataInfo<TType>
{
    public TType Data { get; set; } 
    public int Index { get; set; } 
    public bool Completed { get; set; }
}