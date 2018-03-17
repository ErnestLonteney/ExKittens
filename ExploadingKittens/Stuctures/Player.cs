using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploadingKittens
{
    class Player
    {
        public static Deck D { get; set; }
        public static Stack<Card> B { get; set; } = new Stack<Card>(); 
        public string Name { get; set; }
        private List<Card> _cards = new List<Card>();
        public List<Card> Cards
        {
            get
            {
                return _cards;
            }
            set
            {
                _cards = value;
            }
        }
        public Player(string name)
        {
            Name = name;
        }
        public Card TakeCard(bool bottom = false)
        {
            Card c =  D.TakeCard(bottom);
            if (c is ExKitten || c is ImpladingKitten) return c;
            else
            {
                Cards.Add(c);
                return c; 
            }
        }
        public void TakeFirstCards()
        {
            byte howmanycards = (byte)(D.Type == DeckType.Standart ? D.Type == DeckType.Impladings ? 6 : 5 : 5);
            for (byte i = 0; i < howmanycards; i++) Cards.Add(D.TakeCard());  
        }
        public void TakeDefuse(byte i)
        {
            Cards.Add(new Defuse(i)); 
        }
        public Card GiveCard(byte indexer = 99)
        {
            Card gc; 
            if (indexer == 99) gc = Cards[0];
            else gc = Cards[indexer];

            Cards.Remove(gc);

            return gc; 
        }
        public void PlayCard(Card c)
        {
            B.Push(c); 
            if (!Cards.Remove(c)) throw new Exception("Невизначена карта");
            c.ResolveCardEffect(); 
        }
    }
}
