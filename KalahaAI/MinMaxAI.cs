using System;
using System.Linq;
using ITAI_Assignemnt_1.game;

namespace ITAI_Assignment_1.Game
{
    /// <summary>
    /// Simple minimax AI for Kalaha (Mancala).
    /// Tries to maximize store difference for Player 0, minimize for Player 1.
    /// </summary>
    public class MinimaxAi : IKalahaAI
    {
        private const int DEFAULT_DEPTH = 10; // Limit search depth to avoid huge trees

        public int GetAiMove(KalahaState state)
        {
            var possibleMoves = state.GetPossibleMoves();
            if (possibleMoves.Count == 1)
            {
                // If there's only one move, just pick it
                return possibleMoves[0];
            }

            // Minimax search at a certain depth
            bool isMaximizingPlayer = (state.CurrentPlayer == 0);
            int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;
            int bestMove = possibleMoves[0];

            foreach (int move in possibleMoves)
            {
                // Clone the state so as not to modify the original
                KalahaState cloned = state.Clone();
                cloned.ApplyMove(move);

                // Recursively evaluate
                int score = Minimax(cloned, DEFAULT_DEPTH - 1);

                // If current player is maximizing, pick the highest score
                if (isMaximizingPlayer && score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
                // If current player is minimizing, pick the lowest score
                else if (!isMaximizingPlayer && score < bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        /// <summary>
        /// Standard Minimax recursion with depth limit.
        /// The "player" perspective is determined by KalahaState.CurrentPlayer in the child.
        /// We evaluate from Player 0's perspective (store difference).
        /// Player 0 is maximizing, Player 1 is minimizing.
        /// </summary>
        private int Minimax(KalahaState state, int depth)
        {
            // Base cases
            if (depth == 0 || state.IsTerminal())
            {
                return Evaluate(state);
            }

            var moves = state.GetPossibleMoves();
            if (moves.Count == 0)
            {
                // No moves left => evaluate
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
                    value = Math.Max(value, Minimax(clone, depth - 1));
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
                    value = Math.Min(value, Minimax(clone, depth - 1));
                }
                return value;
            }
        }

        /// <summary>
        /// Evaluate the board from the perspective of Player 0 (bottom).
        /// store0 = board[6], store1 = board[13].
        /// Higher store difference => better for Player 0.
        /// </summary>
        private int Evaluate(KalahaState state)
        {
            int[] b = state.Board; // copy of the board
            int player0Store = b[6];
            int player1Store = b[13];
            return player0Store - player1Store;
        }
    }
}
