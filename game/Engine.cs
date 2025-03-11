using ITAI_Assignment_1.Game;

namespace ITAI_Assignemnt_1.game;

public class Engine
{

    private KalahaState _state = new KalahaState();
    private TerminalInterface _terminal = new TerminalInterface();
    
    public void Start()
    {
        
        while (!_state.IsTerminal()){
            _terminal.DisplayBoard(_state);
            _state.ApplyMove(_terminal.GetUserMove(_state));
            }
           
        
    }

    
    //this should be done using strategy pattern so we can swap out different AIs easily
    public int getAiMove(KalahaState state)
    {
        return 1;
    }
    




}