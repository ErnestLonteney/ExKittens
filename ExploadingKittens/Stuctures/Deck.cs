using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploadingKittens
{
    class Deck : IEnumerable<Card>
    {
        #region Filds
        private static ExKitten[] _exkittens;
        private static Defuse[] _defuses;
        private static Attack[] _attaks;
        private static SeeTheFeuture[] _seefeutures;
        private static AlterTheFeauture[] _alterfutures;
        private static Reverse[] _reverses;
        private static DrawFromTheBottom[] _drawfrombottoms;
        private static Nope[] _nopes;
        private static Skip[] _skips;
        private static Blank[] _blanks;
        private static Shufle[] _shufles;
        private static Favor[] _favors;
        private static TargetAttack[] _targetattaks;
        private Stack<Card> _cards = new Stack<Card>();
        private DeckType _dt;
        private byte _player_count;
        #endregion
        #region Properties
        public Card CurCard { get; set; } = null;
        public DeckType Type { get { return _dt; } }
        public int Count { get { return _cards.Count; } }
        #endregion
        public Deck(DeckType type, byte PlayersCount)
        {
            _dt = type;
            _player_count = PlayersCount;
            PrepareDeck();
        }
        private void PrepareDeck()
        {
            #region InnerMethods
            void PrepareStandartDeck()
            {
                _exkittens = new ExKitten[_player_count - 1];
                for (byte i = 0; i < _exkittens.Length; i++) _exkittens[i] = new ExKitten(i);
                _defuses = new Defuse[(_player_count == 2) ? 2 : _player_count == 3 ? 3 : 4];
                for (byte i = 0; i < _defuses.Length; i++) _defuses[i] = new Defuse(i);
                _attaks = new Attack[4];
                for (byte i = 0; i < _attaks.Length; i++) _attaks[i] = new Attack(i);
                _seefeutures = new SeeTheFeuture[5];
                for (byte i = 0; i < _seefeutures.Length; i++) _seefeutures[i] = new SeeTheFeuture(i);
                _nopes = new Nope[4];
                for (byte i = 0; i < _nopes.Length; i++) _nopes[i] = new Nope(i);
                _blanks = new Blank[20];
                for (byte i = 0; i < 4; i++) _blanks[i] = new Blank(BlankType.CatMellon, i);
                for (byte i = 4; i < 8; i++) _blanks[i] = new Blank(BlankType.HeirCat, i);
                for (byte i = 8; i < 12; i++) _blanks[i] = new Blank(BlankType.PotatoCat, i);
                for (byte i = 12; i < 16; i++) _blanks[i] = new Blank(BlankType.RainbowBlu, i);
                for (byte i = 16; i < 20; i++) _blanks[i] = new Blank(BlankType.TakkoCat, i);
                _skips = new Skip[4];
                for (byte i = 0; i < _skips.Length; i++) _skips[i] = new Skip(i);
                _shufles = new Shufle[4];
                for (byte i = 0; i < _shufles.Length; i++) _shufles[i] = new Shufle(i);
                _favors = new Favor[4];
                for (byte i = 0; i < _favors.Length; i++) _favors[i] = new Favor(i);
            }
            void PrepareImploadingDeck()
            {
                if (_player_count > 2)
                {
                    ExKitten[] newexkittens = new ExKitten[_exkittens.Length - 1];
                    Array.Copy(_exkittens, newexkittens, _exkittens.Length - 1);
                    _exkittens = newexkittens;
                }
                Blank[] newblanks = new Blank[_blanks.Length + 4];
                Array.Copy(_blanks, newblanks, _blanks.Length);
                _blanks = newblanks;
                for (byte i = 0; i < 4; i++) _blanks[_blanks.Length - 1 - i] = new Blank(BlankType.FeralCat, i);
                _targetattaks = new TargetAttack[3];
                for (byte i = 0; i < 3; i++) _targetattaks[i] = new TargetAttack(i);
                _alterfutures = new AlterTheFeauture[4];
                for (byte i = 0; i < 4; i++) _alterfutures[i] = new AlterTheFeauture(i);
                _drawfrombottoms = new DrawFromTheBottom[4];
                for (byte i = 0; i < 4; i++) _drawfrombottoms[i] = new DrawFromTheBottom(i);
                _reverses = new Reverse[4];
                for (byte i = 0; i < 4; i++) _reverses[i] = new Reverse(i);
            }
            void CreateStandartDeck()
            {
                foreach (Card c in _nopes) _cards.Push(c);
                foreach (Card c in _attaks) _cards.Push(c);
                foreach (Card c in _skips) _cards.Push(c);
                foreach (Card c in _seefeutures) _cards.Push(c);
                foreach (Card c in _blanks) _cards.Push(c);
                foreach (Card c in _shufles) _cards.Push(c);
                foreach (Card c in _favors) _cards.Push(c);
            }
            void CreateImploadingDeck()
            {
                CreateStandartDeck();
                foreach (Card c in _targetattaks) _cards.Push(c);
                foreach (Card c in _alterfutures) _cards.Push(c);
                foreach (Card c in _reverses) _cards.Push(c);
                foreach (Card c in _drawfrombottoms) _cards.Push(c);
            }
            #endregion
            switch (_dt)
            {
                case DeckType.Standart:
                    {
                        PrepareStandartDeck();
                        CreateStandartDeck();
                    }
                    break;
                case DeckType.Impladings:
                    {
                        PrepareStandartDeck();
                        PrepareImploadingDeck();
                        CreateImploadingDeck();
                    }
                    break;
                case DeckType.Party:
                    {

                    }
                    break;
            }
        }
        public void AddExKittens()
        {
            foreach (Card c in _exkittens) _cards.Push(c);
        }
        public void AddImplKitten()
        {
            _cards.Push(new ImpladingKitten(0));
        }
        public void AddDefuses(ref Player[] players)
        {
            byte i = (byte)_defuses.Length;
            foreach (Player p in players) p.Cards.Add(new Defuse(i++));
            foreach (Defuse c in _defuses) _cards.Push(c);
        }
        public Card TakeCard(bool bottom = false)
        {
            if (bottom) return _cards.ToArray()[_cards.ToArray().Length - 1];
            return _cards.Pop();
        }
        public void Shuffle()
        {
            Card[] array1 = _cards.ToArray();
            Card[] array2 = new Card[array1.Length];

            Random r = new Random();

            for (int i = 0; i < array1.Length; i++)
            {
                int rn;
                do rn = r.Next(array1.Length);
                while (array1[rn] == null);

                array2[i] = array1[rn];
                array1[rn] = null;
            }
            _cards.Clear();
            foreach (Card x in array2) _cards.Push(x);
        }
        public void BackCard(BackPlace p, Card c)
        {
            var list = _cards.ToList();
            switch (p)
            {
                case BackPlace.Top:
                case BackPlace.Second:
                case BackPlace.Thirth:
                case BackPlace.Fourth:
                    list.Insert((int)p, c);
                    break;
                case BackPlace.Bottom:
                    list.Add(c);
                    break;
                case BackPlace.Random:
                    list.Insert(new Random().Next(list.Count), c);
                    break;
            }
            _cards.Clear();
            list.Reverse();
            foreach (Card card in list) _cards.Push(card);  
        }
        public IEnumerator<Card> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() 
        {
            return _cards.GetEnumerator();
        }
        public Card NextCard
        {
            get
            {
                if (_cards.Count == 0) return null;
                return _cards.Peek();
            }
        }

        public Card Top
        {
            get { return _cards.Peek(); }
        }

        public Card this[int i]
        {
            get
            {
                return _cards.ToArray()[i];
            }
            set
            {
                Card[] list = _cards.ToArray();
                list[i] = value;
                _cards.Clear();
                foreach (Card c in list.Reverse()) _cards.Push(c); 
            }
        }
    }
}
