using System;
using System.Linq;
using ITAI_Assignemnt_1.game;

namespace ITAI_Assignment_1.Game
{
    public class AlphaBetaAi : IKalahaAI
    {
        private const int DEFAULT_DEPTH = 10;

        public int GetAiMove(KalahaState state)
        {
            var possibleMoves = state.GetPossibleMoves();
            if (possibleMoves.Count == 1)
            {
                return possibleMoves[0];
            }

            bool isMaximizingPlayer = (state.CurrentPlayer == 0);
            int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;
            int bestMove = possibleMoves[0];
            int alpha = int.MinValue;
            int beta = int.MaxValue;

            foreach (int move in possibleMoves)
            {
                KalahaState cloned = state.Clone();
                cloned.ApplyMove(move);

                int score = Minimax(cloned, DEFAULT_DEPTH - 1, alpha, beta);

                if (isMaximizingPlayer)
                {
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = move;
                    }
                    alpha = Math.Max(alpha, bestScore);
                }
                else
                {
                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestMove = move;
                    }
                    beta = Math.Min(beta, bestScore);
                }

                if (alpha >= beta)
                {
                    break;
                }
            }

            return bestMove;
        }

        private int Minimax(KalahaState state, int depth, int alpha, int beta)
        {
            if (depth == 0 || state.IsTerminal())
            {
                return Evaluate(state);
            }

            var moves = state.GetPossibleMoves();
            if (moves.Count == 0)
            {
                return Evaluate(state);
            }

            bool isMaximizingPlayer = (state.CurrentPlayer == 0);

            if (isMaximizingPlayer)
            {
                int value = int.MinValue;
                foreach (int move in moves)
                {
                    KalahaState clone = state.Clone();
                    clone.ApplyMove(move);
                    value = Math.Max(value, Minimax(clone, depth - 1, alpha, beta));
                    alpha = Math.Max(alpha, value);
                    if (alpha >= beta)
                        break;
                }
                return value;
            }
            else
            {
                int value = int.MaxValue;
                foreach (int move in moves)
                {
                    KalahaState clone = state.Clone();
                    clone.ApplyMove(move);
                    value = Math.Min(value, Minimax(clone, depth - 1, alpha, beta));
                    beta = Math.Min(beta, value);
                    if (beta <= alpha)
                        break;
                }
                return value;
            }
        }

        private int Evaluate(KalahaState state)
        {
            // early end so it dominates
            if (state.IsTerminal())
            {
                int player0Total = state.Board[6] + Enumerable.Range(0, 6).Sum(i => state.Board[i]);
                int player1Total = state.Board[13] + Enumerable.Range(7, 6).Sum(i => state.Board[i]);
                return (player0Total - player1Total) * 1000;
            }

            int[] board = state.Board;
            int score = 0;

            // store differrence = weight 10
            int storeDiff = board[6] - board[13];
            score += storeDiff * 10; 

            // potential = weight 3
            int player0Seeds = Enumerable.Range(0, 6).Sum(i => board[i]);
            int player1Seeds = Enumerable.Range(7, 6).Sum(i => board[i]);
            score += (player0Seeds - player1Seeds) * 3;

            // instant capture = weight 2
            for (int i = 0; i < 6; i++)
            {
                if (board[i] == 0)
                {
                    int oppositePit = 12 - i;
                    score += board[oppositePit] * 2; 
                }
            }

            // extra turn = arbitrarry +5
            for (int i = 0; i < 6; i++)
            {
                if (board[i] == (6 - i)) 
                    score += 5;
            }
            return score;
        }
    }
}