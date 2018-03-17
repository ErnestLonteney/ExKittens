using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploadingKittens.Stuctures
{
    class PlayerCollection : ICollection<Player>, IEnumerator<Player>
    {
        #region filds
        private CircularLinkedList<Player> _list = new CircularLinkedList<Player>();
        private CircularLinkedListNode<Player> _current = null;
        private CircularLinkedListNode<Player> _enumerator_counter = null;
        public Trigger Direction = new Trigger(true); 
        #endregion
        #region Properties
        public Player CurrentPlayer
        {
            get
            {
                return _current.Value;
            }
        }
        public int Count
        {
            get { return _list.Count(); }
        }
        public Player NextPlayer 
        {
            get
            {
                if (Direction.State == TriggerState.DefaultState) return _current.Next.Value;
                else  return _current.Previus.Value; 
            }
        } 
        public Player PreviusPlayer
        {
            get
            {
                if (Direction.State == TriggerState.DefaultState) return _current.Previus.Value;
                else return _current.Next.Value;
            }
        }
        public Player Winner
        {
            get
            {
                if (Count == 1) return PreviusPlayer;
                else return null; 
            }
        }
        #endregion
        #region ICollection
        public bool IsReadOnly
        {
            get { return false; }
        }
        public void Add(Player p)
        {
            _list.Add(p); 
        }
        public void Clear()
        {
            _list = new CircularLinkedList<Player>();
        }
        public bool Contains(Player item)
        {
            foreach (var el in _list)
                if (el.Value.Equals(item)) return true;

            return false;
        }
        public void CopyTo(Player[] array, int arrayIndex)
        {
            var CurrNode = _current;
            int i = arrayIndex;
            while (!CurrNode.Next.Equals(_current))
            {
                array[i] = CurrNode.Value;
                i++;
            }
        }
        #endregion 
        public void GoToNextPlayer(Player p = null)
        {
            if (p != null)
            {
                foreach (var p_el in _list)
                    if (p.Equals(p_el.Value)) _current = p_el;
                return; 
            }
            if (Direction.State == TriggerState.DefaultState) _current = _current.Next;
            else _current = _current.Previus; 
        }
        public void SetWhoIsFirst(Player p = null)
        {
            if (p != null)
            {
                if (Contains(p)) GoToNextPlayer(p);
            }
            else
            {
                byte i = (byte)new Random().Next(Count); 
                byte j = 0;
                foreach (var r in _list)
                    if (j == i)
                    {
                        _current = r;
                        break; 
                    }
                    else j++; 
            }
        }
        #region IEnumerable
        public IEnumerator<Player> GetEnumerator()
        {
            return this;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this; 
        }
        public Player Current
        {
            get { return _enumerator_counter.Value; }
        }
        object IEnumerator.Current
        {
            get { return _enumerator_counter.Value; }
        }
        public bool Remove(Player item)
        {
            return _list.Remove(item);
        }
        public void Dispose()
        {
            Reset(); 
        }
        public bool MoveNext()
        {
            if (_enumerator_counter == null)
            {
                _enumerator_counter = _current;
                return true; 
            }
            if (Direction.State == TriggerState.DefaultState) _enumerator_counter = _enumerator_counter.Next;
            else _enumerator_counter = _enumerator_counter.Previus;
            if (_enumerator_counter.Equals(_current)) return false;
            return true;
        }
        public void Reset()
        {
            _enumerator_counter = _current; 
        }
        #endregion
        public Player[] ToArray()
        {
            int i = 0;
            Player[] arr = new Player[Count];
            foreach (var n in _list) arr[i++] = n.Value; 
            return arr;
        }
    }
}
