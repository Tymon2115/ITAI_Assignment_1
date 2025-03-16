using System.Diagnostics;
using ITAI_Assignemnt_1.game;

public class TournamentEngine
{
    private readonly IKalahaAI player0;
    private readonly IKalahaAI player1;
    private readonly int numGames;

    public TournamentEngine(IKalahaAI player0, IKalahaAI player1, int numGames)
    {
        this.player0 = player0;
        this.player1 = player1;
        this.numGames = numGames;
    }

    public void RunTournament()
    {
        int wins0 = 0, wins1 = 0, draws = 0;
        long totalMoveTimeTicks = 0;
        int totalMoves = 0;

        for (int game = 0; game < numGames; game++)
        {
            KalahaState state = new KalahaState();
            // Optionally alternate who starts
            state.CurrentPlayer = game % 2; 

            while (!state.IsTerminal())
            {
                IKalahaAI currentAI = state.CurrentPlayer == 0 ? player0 : player1;

                // Measure the move time
                Stopwatch sw = Stopwatch.StartNew();
                int move = currentAI.GetAiMove(state.Clone()); 
                sw.Stop();
                totalMoveTimeTicks += sw.ElapsedTicks;
                totalMoves++;

                state.ApplyMove(move);
            }

            // Determine game result
            int[] board = state.Board;
            int player0Score = board[6];
            int player1Score = board[13];

            if (player0Score > player1Score)
                wins0++;
            else if (player1Score > player0Score)
                wins1++;
            else
                draws++;

            Console.WriteLine($"Game {game + 1}: {player0Score} - {player1Score}");
        }

        double avgMoveTimeMs = (totalMoveTimeTicks * 1000.0 / Stopwatch.Frequency) / totalMoves;
        Console.WriteLine("Tournament Results:");
        Console.WriteLine($"Games played: {numGames}");
        Console.WriteLine($"Player 0 wins: {wins0}");
        Console.WriteLine($"Player 1 wins: {wins1}");
        Console.WriteLine($"Draws: {draws}");
        Console.WriteLine($"Average move time: {avgMoveTimeMs:F2} ms");
    }
}
