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

            if (_state.CurrentPlayer ==0)
            {
                _terminal.DisplayBoard(_state);
                _state.ApplyMove(_terminal.GetUserMove(_state));
            }

            if (_state.CurrentPlayer == 1)
            {
                _terminal.DisplayBoard(_state);
                _state.ApplyMove(_kalahaAi.GetAiMove(_state));
            }
            }
        _terminal.DisplayBoard(_state);
           
        
    }
}