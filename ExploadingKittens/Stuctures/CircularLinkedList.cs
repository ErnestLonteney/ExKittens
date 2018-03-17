using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploadingKittens.Stuctures
{
    class CircularLinkedListNode<T>
    {
        public T Value { get; set; } 
        public CircularLinkedListNode<T> Next { get; set; }
        public CircularLinkedListNode<T> Previus { get; set; }

        public CircularLinkedListNode(T value)
        {
            Value = value; 
        }
    }
    class CircularLinkedList<T> :IEnumerator<CircularLinkedListNode<T>>, IEnumerable<CircularLinkedListNode<T>>  
    {
        private CircularLinkedListNode<T> _head = null;
        private CircularLinkedListNode<T> _tail = null;
        private CircularLinkedListNode<T> curr_el = null;
        public CircularLinkedListNode<T> CurrentEL { get; set; } 
        public CircularLinkedListNode<T> Current
        {
            get { return curr_el; }
        }
        object IEnumerator.Current
        {
            get { return curr_el; }
        }
        public void Add(T value)
        {
            CircularLinkedListNode<T> node = new CircularLinkedListNode<T>(value); 
            if (_head == null)
            {
                _head = node;
                _tail = node;
                _head.Previus = _tail;
                _tail.Next = _head; 
            }
            else
            {
                _tail.Next = node;
                node.Previus = _tail;
                _tail = node;
                _tail.Next = _head;
                _head.Previus = _tail; 
            }
        }
        public bool Remove(T value)
        {
            CircularLinkedListNode<T> Current = _head;

            while (Current != null)
            {
                if (Current.Value.Equals(value))
                {

                    if (_tail.Value.Equals(value) && _head.Value.Equals(value))
                    {
                        _tail = null;
                        _head = null;
                    }
                    else
                    if (_tail.Value.Equals(value))
                    {
                        _tail = _tail.Previus;
                        _tail.Next = _head;
                        _head.Previus = _tail;
                    }
                    else
                    if (_head.Value.Equals(value))
                    {
                        _head = _head.Next;
                        _head.Previus = _tail;
                        _tail.Next = _head;
                    }
                    else
                    {
                        Current.Previus.Next = Current.Next;
                        Current.Next.Previus = Current.Previus; 
                    }
                    return true;
                }
                else Current = Current.Next; 
            }
            return false; 
        }
        public int Count()
        {
            int i = 0;
            if (_head.Equals(_tail)) return 1; 
            CircularLinkedListNode<T> Current = _head;
            do
            {
                i++;
                Current = Current.Next;
            }
            while (!Current.Equals(_head));

           return i; 
        }
        public void Dispose()
        {
            Reset(); 
        }
        public bool MoveNext()
        {
            if (curr_el == null) curr_el = _head;
            else    
              if (curr_el.Equals(_tail))
                return false; 
               else curr_el = curr_el.Next;
            return true; 
        }
        public void Reset()
        {
            curr_el = null;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
           return this;
        }

        IEnumerator<CircularLinkedListNode<T>> IEnumerable<CircularLinkedListNode<T>>.GetEnumerator()
        {
            return this; 
        }
    }
}
