using ITAI_Assignment_1.Game;

namespace ITAI_Assignemnt_1.game;

public class Engine
{

    private KalahaState _state = new KalahaState();
    private TerminalInterface _terminal = new TerminalInterface();
    private IKalahaAI _kalahaAi = new MinimaxAi();
    
    public void Start()
    {
        
        while (!_state.IsTerminal()){
            _terminal.DisplayBoard(_state);
            _state.ApplyMove(_terminal.GetUserMove(_state));
            _terminal.DisplayBoard(_state);
            _state.ApplyMove(_kalahaAi.GetAiMove(_state));
            }
           
        
    }
}