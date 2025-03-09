using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ITAI_Assignemnt_1.game
{
    public class KalahaState
    {
        // player 1(0) 0-5 and 6 as store, player 2(1) 7-12 and 13 as store
        private int[] _board = new int[14];
        public int CurrentPlayer { get; set; }

        public KalahaState()
        {
            _board = [.. Enumerable.Range(0, 14).Select(i => (i == 6 || i == 13) ? 0 : 4)];
        }

        public KalahaState Clone()
        {
            KalahaState copy = new KalahaState
            {
                _board = (int[])_board.Clone(),
                CurrentPlayer = CurrentPlayer
            };

            return copy;
        }

        public KalahaState ApplyMove(int index)
        {
            if (!GetPossibleMoves().Contains(index))
            {
                throw new InvalidOperationException("Invalid move");
            }

            int stones = _board[index];
            _board[index] = 0;
            int currentPit = index;

            while (stones > 0)
            {
                currentPit = (currentPit + 1) % 14;
                // we only put stones into own store not opponents
                if (CurrentPlayer == 0 && currentPit == 13)
                {
                    continue;
                }
                else if (CurrentPlayer == 1 && currentPit == 6)
                {
                    continue;
                }

                _board[currentPit]++;
                stones--;
            }
            // this is for capturing the stones of oposite if we land on empty one
            if (CurrentPlayer == 0 && currentPit < 6 && _board[currentPit] == 1)
            {
                int oppositePit = 12 - currentPit;
                _board[6] += _board[oppositePit] + 1;
                _board[oppositePit] = 0;
                _board[currentPit] = 0;
            }
            else if (CurrentPlayer == 1 && currentPit > 6 && currentPit < 13 && _board[currentPit] == 1)
            {
                int oppositePit = 12 - currentPit;
                _board[13] += _board[oppositePit] + 1;
                _board[oppositePit] = 0;
                _board[currentPit] = 0;
            }
            // if we land in own store then we can do one more move
            bool getExtraTurn = false;
            if (CurrentPlayer == 0 && currentPit == 6)
            {
                getExtraTurn = true;
            }
            else if (CurrentPlayer == 1 && currentPit == 13)
            {
                getExtraTurn = true;
            }

            if (!getExtraTurn)
            {
                CurrentPlayer = 1 - CurrentPlayer;
            }

            return this;
        }



        public List<int> GetPossibleMoves()
        {
            if (CurrentPlayer == 0)
            {
                return [.. Enumerable.Range(0, 6).Where(i => _board[i] > 0)];
            }
            else
            {
                return [.. Enumerable.Range(7, 6).Where(i => _board[i] > 0)];
            }
        }

        public bool IsTerminal()
        {
            return _board.Take(6).All(x => x == 0) || _board.Skip(7).Take(6).All(x => x == 0);
        }

        private bool IsStore(int player, int index)
        {
            return player == 0 ? index == 6 : index == 13;
        }

    }
}