using ITAI_Assignemnt_1.game;
using ITAI_Assignment_1.Game;

public static class TournamentRunner
{
    public static void RunAllTournaments(int tournamentGames = 4)
    {
        var aiList = new List<(string name, IKalahaAI ai)>
        {
            ("Minimax", new MinimaxAi()),
            ("AlphaBeta", new AlphaBetaAi()),
            ("AlphaBetaOldEval", new AlphaBetaAiOldEval())
        };

        Console.WriteLine("Running tournaments between all AI pairs...\n");

        for (int i = 0; i < aiList.Count; i++)
        {
            for (int j = i + 1; j < aiList.Count; j++)
            {
                Console.WriteLine("Tournament: {0} (Player 0) vs {1} (Player 1)", aiList[i].name, aiList[j].name);
                TournamentEngine tournament1 = new TournamentEngine(aiList[i].ai, aiList[j].ai, tournamentGames);
                tournament1.RunTournament();
                Console.WriteLine();

                // Second matchup: aiList[j] as Player 0, aiList[i] as Player 1
                Console.WriteLine("Tournament: {0} (Player 0) vs {1} (Player 1)", aiList[j].name, aiList[i].name);
                TournamentEngine tournament2 = new TournamentEngine(aiList[j].ai, aiList[i].ai, tournamentGames);
                tournament2.RunTournament();
                Console.WriteLine(new string('-', 50) + "\n");
            }
        }
    }
}
