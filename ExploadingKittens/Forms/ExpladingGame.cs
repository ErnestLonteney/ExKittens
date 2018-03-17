using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using InterfaceWork;
using ExploadingKittens.Stuctures; 

namespace ExploadingKittens
{
    public partial class Game : Form
    {
        #region Filds
        private Form welcomeform;
        private bool EndGame = false;
        private Deck ExploadingDeck;
        private byte howmanydudes; 
        private byte swap_i = 99;
        #region Game_flags
        private bool doubleturn = false;
        private bool isExkitten = false;
        private bool isNope = false;
        private bool itsGiving = false;
        private bool GivingAndTwoPlayers = false; 
        #endregion
        private List<Blank> blist = new List<Blank>();
        private byte expl_kittens = 0; 
        public PictureBox[] playerboxes = new PictureBox[10];
        public PictureBox PlayngCardImage; 
        private PlayerCollection AllPlayers;
        #endregion
        #region Properties
        public bool DoubleTurn
        {
            get { return doubleturn; }

            set
                {
                   doubleturn = value;
                   ch.doubleturn_changed = true;
                }
        }
        public bool ItsGiving
        {
            get
             {
                return itsGiving;
             }
            set
            {
                itsGiving = value;
                ch.IsGiving_changed = true;
            }
        }
        public bool IsNope
        {
            get { return isNope; }
            set { isNope = true; }
        }

        private Card CurrentCard
        {
            get
            { return ExploadingDeck.CurCard; }
            set
            { ExploadingDeck.CurCard = value; }     
         }
        Player CurrentPlayer
        { get { return AllPlayers.CurrentPlayer; } }
        #endregion
        #region Constructor
        public Game()
        {
            InitializeComponent();
            #region PictureBoxes
            playerboxes[0] = pictureBox1;
            playerboxes[1] = pictureBox3;
            playerboxes[2] = pictureBox4;
            playerboxes[3] = pictureBox5;
            playerboxes[4] = pictureBox6;
            playerboxes[5] = pictureBox7;
            playerboxes[6] = pictureBox8;
            playerboxes[7] = pictureBox9;
            playerboxes[8] = pictureBox10;
            playerboxes[9] = pictureBox11;
            PlayngCardImage = pictureBox12;
            #endregion
            for (int i = 0; i < playerboxes.Length; i++) playerboxes[i].SizeMode = PictureBoxSizeMode.Zoom;
            ImageWorker.MyGame = this;
            ExKitten.Resolve = new Action(ResolveExKitten);
            Attack.Resolve = new Action(ResolveAttack);
            TargetAttack.Resolve = new Action(ResolveTargetAttak);
            Skip.Resolve = new Action(ResolveSkip);
            Reverse.Resolve = new Action(ResolveReverse);
            Nope.Resolve = new Action(ResolveNope);
            ImpladingKitten.Resolve = new Action(ResolveImpladingKitten);
            SeeTheFeuture.Resolve = new Action(ResolveSeeTheFeuture);
            Blank.Resolve = new Action(ResolveBlank);
            Favor.Resolve = new Action(ResolveFavor);
            Shufle.Resolve = new Action(ResolveShufle);
            Defuse.Resolve = new Action(ResolveDefuse);
            DrawFromTheBottom.Resolve = new Action(ResolveTakeFromBottom);
            AlterTheFeauture.Resolve = new Action(ResolveAlterTheFeauture);
            AllPlayers = new PlayerCollection(); 
        }
        #endregion 
        public void CreateGame(string[] names, DeckType decktype, Form f)
        {
            welcomeform = f;
            howmanydudes = (byte)names.Length;
            expl_kittens = (byte)(howmanydudes - 1); 
            // Create Deck 
            ExploadingDeck = new Deck(decktype, howmanydudes);
            ExploadingDeck.Shuffle();
            Player.D = ExploadingDeck;
            // Create Players
            for (int i = 0; i < howmanydudes; i++)
            {
                Player new_plr = new Player(names[i]);
                new_plr.TakeFirstCards();
                new_plr.TakeDefuse((byte)i);
                AllPlayers.Add(new_plr);
            }
            ExploadingDeck.AddExKittens();
            if (decktype == DeckType.Impladings)
            {
                ExploadingDeck.AddImplKitten();
                DisplayVector(); 
                if (howmanydudes == 2) expl_kittens += 1;
            }
            ExploadingDeck.Shuffle();
            // First player 
            AllPlayers.SetWhoIsFirst();
            ImageWorker.LoadPlayerCards(CurrentPlayer);
            label1.Text = "Хід граця " + CurrentPlayer.Name;
            DisplayExploadingInfo();
        }
        private void DisplayExploadingInfo()
        {
            label2.Text = "Лишилось " + ExploadingDeck.Count.ToString() + " карт в колоді!";
            label3.Text = $"Шанс витягнути вибухове кошеня {100 / ExploadingDeck.Count * expl_kittens} %";
        }
        private void BackCardToDeck(Card c)
        {
            BackCardForm bkf = new BackCardForm();
            bkf.ShowDialog();
            ExploadingDeck.BackCard(bkf.response, c); 
        }
        private void GoToNextPlayer(Player p = null)
        {
            if (AllPlayers.Count == 1)
            {
                EndGame = true;
                MessageBox.Show("Виграв гравець " + AllPlayers.Winner.Name);
                return;
            }
            ch.privius_plr = CurrentPlayer;
            AllPlayers.GoToNextPlayer(p);
            if (p == null) label1.Text = "Хід гравця " + CurrentPlayer.Name;
            else label1.Text = "Хід гравця " + p.Name;
            ImageWorker.LoadPlayerCards(CurrentPlayer);
        }
        private void GetCardFromDeck(bool bottom = false)
        {
            if (isExkitten) return; 
            MakeFeatureUnvisible(false);
            blist.Clear();
            bool flag = doubleturn;
            if (doubleturn) doubleturn = false; 
            Card c = CurrentPlayer.TakeCard(bottom);
            CurrentCard = c;
            if (ExploadingDeck.NextCard is ImpladingKitten)
            {
                ImpladingKitten ik = ExploadingDeck.NextCard as ImpladingKitten;
                if (ik.Flag) ImageWorker.LoadImageFromCard(ExploadingDeck.NextCard, pictureBox2);
            }
            if (c is ExKitten || c is ImpladingKitten)
            {
                ImageWorker.LoadImageFromCard(c, pictureBox12);
                if (c is ImpladingKitten)
                {
                    ImpladingKitten ik = c as ImpladingKitten;
                    if (ik.Flag) pictureBox2.Image = Properties.Resources.draw_pile;
                }
                c.ResolveCardEffect();
                if (EndGame) return; 
                if (c is ExKitten) notHaveButton.Visible = true;
            }
            else
            {
                pictureBox16.Visible = true;
                ImageWorker.LoadImageFromCard(c, pictureBox16);
                Application.DoEvents(); 
                System.Threading.Thread.Sleep(300);
                pictureBox16.Visible = false;
            }
            ImageWorker.LoadPlayerCards(CurrentPlayer);
            if (!(flag || isExkitten))
            {
                doubleturn = false;
                isExkitten = false;
                GoToNextPlayer();
            }
        }
        void Exploading()
        {
            MessageBox.Show(CurrentPlayer.Name +" вибухнув!");
            AllPlayers.Remove(CurrentPlayer);
            isExkitten = false;
            GoToNextPlayer(); 
        }
        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            welcomeform.Close();
        }
        private void MakeFeatureUnvisible(bool turner)
        {
            pictureBox13.Visible = turner;
            pictureBox14.Visible = turner;
            pictureBox15.Visible = turner;
            pictureBox13.Cursor = Cursors.Default;
            pictureBox14.Cursor = Cursors.Default;
            pictureBox15.Cursor = Cursors.Default; 
            if (notHaveButton.Visible == true) notHaveButton.Visible = false;
            if (label5.Visible && !turner) label5.Visible = false;  
        }
        private Card GetWhatCardChoise(PictureBox cl_pb)
        {
            int i = 0;
            MakeFeatureUnvisible(false);
            foreach (PictureBox pb in playerboxes)
                if (cl_pb.Equals(pb)) break;
                else i++;
            i = i + ImageWorker.Firscardindex;
            return CurrentPlayer.Cards[i];
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (EndGame) return;         
            PictureBox thispb = sender as PictureBox;
            if (thispb == null || thispb.Image == null) return;
            CurrentCard = GetWhatCardChoise(thispb); 
            if (isExkitten && !(CurrentCard is Defuse)) return;
            if (CurrentCard is Defuse && !isExkitten) return;
            ClearChengest(); 
            if (itsGiving) CurrentPlayer.Cards.Remove(CurrentCard);
            else
            {
                ImageWorker.LoadImageFromCard(CurrentCard, pictureBox12);
                //================================
                CurrentPlayer.PlayCard(CurrentCard);
                //================================
                thispb.Image = null;
                ImageWorker.LoadPlayerCards(CurrentPlayer);
            }
            if (label4.Text != String.Empty) DisplayVector();
            if (itsGiving)
            {
                if (GivingAndTwoPlayers)
                {
                    ImageWorker.LoadPlayerCards(CurrentPlayer);
                    GivingAndTwoPlayers = false; 
                    return; 
                }
                ItsGiving = false;
                ch.privius_plr.Cards.Add(CurrentCard); 
                GoToNextPlayer(ch.privius_plr);
                blist.Clear();
                _test_blank = 0; 
            }
        }
        private void DisplayVector()
        {
            if (AllPlayers.Count == 2) return; 
            label4.Text = (AllPlayers.Direction.State == TriggerState.DefaultState) ? "Напрям ходу ----->" : "Напрям ходу <-------";
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (EndGame) return;
            GetCardFromDeck();
            DisplayExploadingInfo();
        }
        public void TurnOnOrOffButtons(bool state)
        {
            leftButton.Visible = state;
            rightButton.Visible = state; 
        }
        private void rightButton_Click(object sender, EventArgs e)
        {
            ImageWorker.GoToRight(CurrentPlayer); 
        }
        private void leftButton_Click(object sender, EventArgs e)
        {
            ImageWorker.GoToLeft(CurrentPlayer); 
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            ch.privius_plr = CurrentPlayer; 
            Player OnWhoPlr = null;
            if (!(comboBox1.Text == CurrentPlayer.Name))
            {
                foreach (Player p in AllPlayers)
                    if (p.Name == comboBox1.Text)
                    {
                        OnWhoPlr = p;
                        break;
                    }
            }
            ResolveOnWhoPlayedCard(OnWhoPlr);
            VisibleOrUnvisibleChoise(value: false); 
        }
        private void button1_Click(object sender, EventArgs e)
        {     
            Exploading();
            expl_kittens--;
            DisplayExploadingInfo(); 
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (((PictureBox)sender).Image == null) ((PictureBox)sender).Cursor = Cursors.Default;
            else ((PictureBox)sender).Cursor = Cursors.Hand; 
            return;
            timer1.Stop();
            timer1.Interval = 1000; 
            timer1.Start();
            timer1.Tag = sender;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            PictureBox pb = timer1.Tag as PictureBox;
            ImageWorker.SetImageOnTop(pb);
        }
        void SwapCards(int index1, int index2)
        {
            Card buf = ExploadingDeck[index1];
            ExploadingDeck[index1] = ExploadingDeck[index2];
            ExploadingDeck[index2] = buf;
        }
        private void MakeHandOnFeautureImages()
        {
            pictureBox13.Cursor = Cursors.Hand;
            pictureBox14.Cursor = Cursors.Hand;
            pictureBox15.Cursor = Cursors.Hand;
        }
        private void pictureBox15_Click(object sender, EventArgs e)
        {
            if (CurrentCard is AlterTheFeauture)
            {
                SwapCards(0, 1);
                ResolveSeeTheFeuture();
                MakeHandOnFeautureImages();
            }
        }
        private void pictureBox14_Click(object sender, EventArgs e)
        {
            if (CurrentCard is AlterTheFeauture)
            {
                SwapCards(1, 2);
                ResolveSeeTheFeuture();
                MakeHandOnFeautureImages(); 
            }
        }
        private void pictureBox13_Click(object sender, EventArgs e)
        {
            if (CurrentCard is AlterTheFeauture)
            {
                SwapCards(2, 0);
                ResolveSeeTheFeuture();
                MakeHandOnFeautureImages();
            }
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            foreach (Player p in AllPlayers)
                MessageBox.Show(p.Name); 
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            string rule = String.Empty;
            switch (ExploadingDeck.Type)
            {
                case DeckType.Standart: rule = @"Rules\rules.pdf";
                    break;
                case DeckType.Impladings: rule = @"Rules\Impl_rules.pdf";
                    break; 
            }
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = rule; 
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }
        private void pictureBox12_Click(object sender, EventArgs e)
        {
            if (swap_i == 99) return; 
            if (swap_i > Player.B.ToArray().Length-1) swap_i = 0;
            ImageWorker.LoadImageFromCard(Player.B.ToArray()[swap_i++], pictureBox12); 
        }
        private void takeButton_Click(object sender, EventArgs e)
        {
            CurrentPlayer.Cards.Add(Player.B.ToArray()[swap_i - 1]);
            List<Card> buf = Player.B.ToList();
            buf.Remove(Player.B.ToArray()[swap_i - 1]);
            buf.Reverse(); 
            Player.B.Clear();
            foreach (Card c in buf) Player.B.Push(c);
            ImageWorker.LoadImageFromCard(Player.B.Peek(), pictureBox12);
            infoLabel.Text = String.Empty;
            takeButton.Visible = false;
            swap_i = 99; 
            pictureBox12.Cursor = Cursors.Default;
            ImageWorker.LoadPlayerCards(CurrentPlayer); 
        }
        private void Game_MouseMove(object sender, MouseEventArgs e)
        {
            if (ImageWorker.OnTopBox == null) return;
            timer1.Stop();
            ImageWorker.BackImage();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int i = 0; 
            string[] names = new string[AllPlayers.Count];
            foreach (Player p in AllPlayers) names[i++] = p.Name;
            CreateGame(names, ExploadingDeck.Type, welcomeform);  
        }
    }
}
