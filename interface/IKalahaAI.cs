namespace ITAI_Assignemnt_1.game;

public interface IKalahaAI
{
    
    /// <summary>
    /// Given the current KalahaState, choose the next pit.
    /// </summary>
    int GetAiMove(KalahaState state);
    
}