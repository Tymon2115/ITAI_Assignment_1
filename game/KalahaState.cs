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
        
        // Returns a copy to prevent external modification, but the access to it is
        public int[] Board => _board.ToArray(); 
        public int CurrentPlayer { get; set; }

        public KalahaState()
        {
            int startingStonesInPit = 4;
            _board = [.. Enumerable.Range(0, 14).Select(i => (i == 6 || i == 13) ? 0 : startingStonesInPit)];
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
            if (CurrentPlayer == 0 && currentPit < 6 && _board[currentPit] == 1 )
            {
                int oppositePit = 12 - currentPit;
                if(_board[oppositePit] != 0){
                    _board[6] += _board[oppositePit] + 1;
                    _board[oppositePit] = 0;
                    _board[currentPit] = 0;
                }else{

                }
                
            }
            else if (CurrentPlayer == 1 && currentPit > 6 && currentPit < 13 && _board[currentPit] == 1 )
            {
                int oppositePit = 12 - currentPit;

                if(_board[oppositePit] != 0){
                    _board[13] += _board[oppositePit] + 1;
                    _board[oppositePit] = 0;
                    _board[currentPit] = 0;
                }else{
                    
                }
                
                
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

        public bool IsTerminal() =>
            IsPlayer1PitsEmpty() || IsPlayer2PitsEmpty();

        private bool IsPlayer1PitsEmpty() =>
            SumPits(0, 6) == 0;

        private bool IsPlayer2PitsEmpty() =>
            SumPits(7, 6) == 0;

        private int SumPits(int start, int count) =>
            _board.Skip(start).Take(count).Sum();

        private void ClearPits(int start, int count)
        {
            for (int i = start; i < start + count; i++)
                _board[i] = 0;
        }

        private void CollectStonesForPlayer(int pitsStart, int pitsCount, int storeIndex)
        {
            int stones = SumPits(pitsStart, pitsCount);
            ClearPits(pitsStart, pitsCount);
            _board[storeIndex] += stones;
        }

        public bool IsFinalized()
        {
            if (!IsTerminal()) return false;

            if (IsPlayer1PitsEmpty())
                CollectStonesForPlayer(7, 6, 13);
            else if (IsPlayer2PitsEmpty())
                CollectStonesForPlayer(0, 6, 6);

            return true;
        }

        private bool IsStore(int player, int index)
        {
            return player == 0 ? index == 6 : index == 13;
        }

    }
}