namespace SpaceBattle;

public interface ISagaStep
{
    void Execute();       
    void Compensate();    
}