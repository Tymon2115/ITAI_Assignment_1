using System;
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
        long totalMoveTimeTicks0 = 0;
        long totalMoveTimeTicks1 = 0;
        int totalMoves0 = 0;
        int totalMoves1 = 0;

        for (int game = 0; game < numGames; game++)
        {
            KalahaState state = new KalahaState();
            state.CurrentPlayer = game % 2; 

            while (!state.IsFinalized())
            {
                Stopwatch sw = Stopwatch.StartNew();
                int move;
                
                if (state.CurrentPlayer == 0)
                {
                    move = player0.GetAiMove(state.Clone());
                    sw.Stop();
                    totalMoveTimeTicks0 += sw.ElapsedTicks;
                    totalMoves0++;
                }
                else
                {
                    move = player1.GetAiMove(state.Clone());
                    sw.Stop();
                    totalMoveTimeTicks1 += sw.ElapsedTicks;
                    totalMoves1++;
                }

                state.ApplyMove(move);
            }

            int[] board = state.Board;
            int player0Score = board[6];
            int player1Score = board[13];

            if (player0Score > player1Score)
                wins0++;
            else if (player1Score > player0Score)
                wins1++;
            else
                draws++;
        
        
        Console.WriteLine($"Game {game + 1}: Player 0: {player0Score}, Player 1: {player1Score}");
        }

        double avgMoveTimeMs0 = (totalMoveTimeTicks0 * 1000.0 / Stopwatch.Frequency) / totalMoves0;
        double avgMoveTimeMs1 = (totalMoveTimeTicks1 * 1000.0 / Stopwatch.Frequency) / totalMoves1;

        Console.WriteLine("Tournament Results:");
        Console.WriteLine($"Games played: {numGames}");
        Console.WriteLine($"Player 0 wins: {wins0}");
        Console.WriteLine($"Player 1 wins: {wins1}");
        Console.WriteLine($"Draws: {draws}");
        Console.WriteLine($"Average move time for Player 0: {avgMoveTimeMs0:F2} ms");
        Console.WriteLine($"Average move time for Player 1: {avgMoveTimeMs1:F2} ms");
    }
}
