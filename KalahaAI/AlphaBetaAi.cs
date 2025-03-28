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
            int[] b = state.Board;
            int player0Store = b[6];
            int player1Store = b[13];
            int storeDiff = player0Store - player1Store;
            int player0Stones = b.Take(6).Sum();
            int player1Stones = b.Skip(7).Take(6).Sum();
            int stonesDiff = player0Stones - player1Stones;
            int extraTurnsPlayer0 = 0;
            for (int i = 0; i < 6; i++)
            {
                int stones = b[i];
                int distanceToStore = 6 - i;
                if (stones == distanceToStore) extraTurnsPlayer0++;
            }

            int extraTurnsPlayer1 = 0;
            for (int i = 7; i < 13; i++)
            {
                int stones = b[i];
                int distanceToStore = 13 - i;
                if (stones == distanceToStore) extraTurnsPlayer1++;
            }
            int extraTurnsDiff = extraTurnsPlayer0 - extraTurnsPlayer1;

            int captureOppositePlayer0 = 0;
            for (int i = 0; i < 6; i++)
            {
                int oppositePit = 12 - i;
                if (b[i] == 0 && b[oppositePit] > 0) captureOppositePlayer0++;
            }
            int captureOppositePlayer1 = 0;
            for (int i = 7; i < 13; i++)
            {
                int oppositePit = 12 - i;
                if (b[i] == 0 && b[oppositePit] > 0) captureOppositePlayer1++;
            }
            int captureDiff = captureOppositePlayer0 - captureOppositePlayer1;

            int emptyPitsPlayer0 = b.Take(6).Count(x => x == 0);
            int emptyPitsPlayer1 = b.Skip(7).Take(6).Count(x => x == 0);
            int emptyPitsDiff = (-emptyPitsPlayer0) + emptyPitsPlayer1;
            // those values are random as a result of testing, everyrthing yields at best tie
            return storeDiff * 7      
                + stonesDiff * (-4)     
                + extraTurnsDiff * 10  
                + captureDiff * 12     
                + emptyPitsDiff * (-2);  
        }
    }
}