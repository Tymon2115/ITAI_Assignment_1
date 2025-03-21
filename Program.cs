using ITAI_Assignemnt_1.game;
using ITAI_Assignment_1.Game;
class Program
{
     public static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "tournament")
        {
            // For example, compare Minimax and AlphaBeta
            IKalahaAI ai1 = new MinimaxAi();
            // IKalahaAI ai1 = new AlphaBetaAi();
            IKalahaAI ai2 = new AlphaBetaAi();
            TournamentEngine tournament = new TournamentEngine(ai1, ai2, 2); 
            Console.WriteLine("Running tournament...");
            tournament.RunTournament();
        }
        else
        {
            Engine engine = new Engine();
            engine.Start();
        }
    }
}