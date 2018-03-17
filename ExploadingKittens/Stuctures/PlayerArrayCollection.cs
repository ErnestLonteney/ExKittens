using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploadingKittens.Stuctures
{
    [Obsolete]
    class PlayerArrayCollection : IEnumerator<Player>, IEnumerable<Player>
    {
        private Player[] _players;
        private byte _curIndex = 0;
        private byte _i = 0;
        private byte _plrcnt = 0; 
        private bool Toward { get; set; } = true; 

        public PlayerArrayCollection(byte count, byte whoisfirst)
        {
            _players = new Player[count];
            _curIndex = whoisfirst;
            _i = _curIndex; 
        }
        public void Add(Player p)
        {
            for (byte i=0; i < _players.Length; i++)
                if (_players[i] == null)
                {
                    _players[i] = p;
                    break; 
                }
        }
        public void Remove(Player p)
        {
            for (int i = 0; i < _players.Length; i++) if (_players[i].Equals(p)) _players[i] = null;  
        }
        public byte Count
        {
            get
            {
                return (byte)_players.Where(p => p == null).Count();  
            }
        }
        public Player CurrentPlayer
        {
            get { return _players[_curIndex]; }
        }

        public Player Current
        {
            get { return _players[_i]; }
        }

        object IEnumerator.Current
        {
            get { return _players[_i]; }
        }
        public void TurnNext(ref byte indexer)
        {
            if (_players.Where(p => p == null).Count() == _players.Length - 1) return;
            if (Toward)
            {
                if (_curIndex == _players.Length - 1) indexer = 0;
                else indexer++;
            }
            else
            {
                if (indexer == 0) _curIndex = (byte)(_players.Length - 1);
                else indexer--;
            }
            if (_players[indexer] == null) GoToNextPlayer();
        }

        public bool GoToNextPlayer(byte plrinx = 99)
        {
            if (_players.Where(p => p == null).Count() == _players.Length - 1) return false;
            if (plrinx != 99)
            {
                _curIndex = plrinx;
                return true; 
            }
            TurnNext(ref _curIndex); 

            return true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            if (_plrcnt == _players.Where(p => p != null).Count()) return false;
            TurnNext(ref _i);
            _plrcnt++; 
            return true; 
        }

        public void Reset()
        {
            _plrcnt = _curIndex; 
        }

        public IEnumerator<Player> GetEnumerator()
        {
            return this; 
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this; 
        }
    }
}
