using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using InterfaceWork;

namespace ExploadingKittens
{
    public partial class Game : Form
    {
        struct Changest
        {
            public bool doubleturn_changed;
            public bool IsGiving_changed;
            public bool IsNope_changed;
            public bool IsToward_changed;
            public Player privius_plr; 
        }
        private BlankResults _test_blank = BlankResults.None;
        private Changest ch;
        void ResolveSkip()
        {
            if (doubleturn) DoubleTurn = false;
            else GoToNextPlayer();
        }
        void ResolveReverse()
        {
            if (AllPlayers.Count > 2)
            {
                ch.IsToward_changed = true;
                AllPlayers.Direction.Switch();             
            }
            ResolveSkip(); 
        }
        void ResolveAttack()
        {
            GoToNextPlayer();
            DoubleTurn = true;
        }               
        void ResolveTargetAttak()
        {
            if (AllPlayers.Count > 2)
            {
                DoubleTurn = true;
                infoLabel.Text = "Оберіть граця, на якого граєте карту";
                answerButton.Text = "Зіграти на";
                VisibleOrUnvisibleChoise(value: true); 
            }
            else ResolveAttack(); 
        }
        void ResolveExKitten()
        {
            isExkitten = true;
        }
        void ResolveFavor()
        {
            if (AllPlayers.Count > 2)
            {
                infoLabel.Text = "Оберіть граця, що має ввідати карту";
                VisibleOrUnvisibleChoise(value: true); 
            }
            else
            {
                ResolveOnWhoPlayedCard(AllPlayers.NextPlayer);
                GivingAndTwoPlayers = true; 
            }
        }
        void ResolveShufle()
        {
            ExploadingDeck.Shuffle();
            if (!(ExploadingDeck.Top is ImpladingKitten)) pictureBox2.Image = Properties.Resources.draw_pile;  
        }
        void ResolveImpladingKitten()
        {
            var k = (ImpladingKitten)CurrentCard;
            if (k.Flag) Exploading();
            else
            {
                k.Flag = true;
                BackCardToDeck(CurrentCard);
                if (ExploadingDeck.Top is ImpladingKitten) ImageWorker.LoadImageFromCard(ExploadingDeck.Top, pictureBox2);
            }
        }
        void ResolveBlank()
        {
            blist.Add((Blank)CurrentCard);
            TestBlank(); 
            switch (_test_blank)
            {
                case BlankResults.Double:
                    {
                        if (AllPlayers.Count > 2)
                        {
                            infoLabel.Text = "Оберіть гравця в якого бажаєте витягти карту";
                            VisibleOrUnvisibleChoise(value: true);
                        }
                        else
                        {
                            GivingAndTwoPlayers = true;
                            ResolveOnWhoPlayedCard(AllPlayers.NextPlayer);
                        }
                    }
                    break;
                case BlankResults.Triple:
                    {
                        if (AllPlayers.Count > 2) infoLabel.Text = "Оберіть гравця і тип карти.";
                        else infoLabel.Text = "Оберіть тип карти.";
                        VisibleOrUnvisibleChoise(value: true);
                        comboBox2.Visible = true; 
                    }
                    break;
                case BlankResults.FiveCards: infoLabel.Text = "Оберіть карту з відбою."; 
                    break; 
            }
        }
        private void TestBlank()
        {
            bool equals = true;
            bool alldifferent = true;
            Blank template = null;
            foreach (Blank b in blist)
                if (b._blank != BlankType.FeralCat)
                {
                    template = b;
                    break;
                }
            if (template == null) template = blist[0];
            // Equils ? 
            foreach (Blank b in blist)
                if (b._blank != BlankType.FeralCat && b._blank != template._blank) equals = false;
            _test_blank = BlankResults.None; 
            switch (blist.Count)
            {
                case 2:
                    if (equals) _test_blank = BlankResults.Double;
                    break;
                case 3:
                    if (equals) _test_blank = BlankResults.Triple;
                    break; 
                case 5:
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (blist[i]._blank == BlankType.FeralCat) continue;
                            for (int j = 0; j < 5; j++)
                            {
                                if (j == i || blist[j]._blank == BlankType.FeralCat) continue;
                                if (blist[i]._blank == blist[j]._blank) alldifferent = false;
                            }
                        }
                        if (alldifferent) _test_blank = BlankResults.FiveCards; 
                    }
                    break;
            }
        }
        void ResolveNope()
        {
            RejectChangest();
        }
        void ResolveDefuse()
        {
            if (!isExkitten) return; 
            isExkitten = false;
            BackCardToDeck(new ExKitten(0));
            GoToNextPlayer();
        }
        void ResolveSeeTheFeuture()
        {
            MakeFeatureUnvisible(true);
            ImageWorker.LoadImageFromCard(ExploadingDeck.Top, pictureBox15);
            ImageWorker.LoadImageFromCard(ExploadingDeck[1], pictureBox14);
            ImageWorker.LoadImageFromCard(ExploadingDeck[2], pictureBox13);
        }
        void ResolveTakeFromBottom()
        {
            GetCardFromDeck(bottom:true);    
        }
        void ResolveAlterTheFeauture()
        {
            ResolveSeeTheFeuture();
            label5.Visible = true;
            MakeHandOnFeautureImages();
        }
        private void ClearChengest(bool flag = false)
        {
            ch.IsGiving_changed = flag;
            ch.IsNope_changed = flag;
            ch.doubleturn_changed = flag;
            ch.IsToward_changed = flag; 
        }
        private void RejectChangest()
        {
            Player buf_plr = ch.privius_plr;
            if (DoubleTurn)
            {
                doubleturn = !DoubleTurn;
                ch.doubleturn_changed = true; 
            }
            if (isNope)
            {
                isNope = !isNope;
                ch.IsNope_changed = true;
            }
            if (ItsGiving)
            {
                itsGiving = !ItsGiving;
                ch.IsGiving_changed = true; 
            }
            if (ch.IsToward_changed)
            {
                AllPlayers.Direction.Switch();
                ch.IsToward_changed = true; 
            }
            if (!CurrentPlayer.Equals(ch.privius_plr)) GoToNextPlayer(ch.privius_plr);
            ch.privius_plr = buf_plr; 
        }

        private void ResolveOnWhoPlayedCard(Player OnWhoPlr)
        {
            switch (CurrentCard.ToString())
            {
                case "TargetAttack": GoToNextPlayer(OnWhoPlr);
                    break;
                case "Blank":
                    {
                        switch (_test_blank)
                        {
                            case BlankResults.Double:
                                {
                                    ItsGiving = true;
                                    GoToNextPlayer(OnWhoPlr);
                                    for (byte b = 0; b < playerboxes.Length; b++) playerboxes[b].Image = Properties.Resources.back;
                                    label1.Text = $"Гравець в якого витягують карту {CurrentPlayer.Name}";
                                }
                                break;
                            case BlankResults.Triple:
                                {
                                    if (comboBox1.Text == String.Empty || comboBox2.Text == String.Empty)
                                    {
                                        MessageBox.Show("Ви не обрали!");
                                        return;
                                    }
                                    for (byte i = 0; i < OnWhoPlr.Cards.Count; i++)
                                    {
                                        if (OnWhoPlr.Cards[i].ToString() == comboBox2.Text)
                                        {
                                            CurrentPlayer.Cards.Add(OnWhoPlr.GiveCard(i));
                                            break;
                                        }
                                    }
                                }
                                break;
                            case BlankResults.FiveCards:
                                {
                                    pictureBox12.Cursor = Cursors.Hand;
                                    infoLabel.Text = "Оберіть карту з відбою!";
                                    swap_i = 1;
                                }
                                break;
                        }
                    }
                    break;
                case "Favor":
                    {
                        ch.privius_plr = CurrentPlayer;
                        GoToNextPlayer(OnWhoPlr);
                        ItsGiving = true;
                        label2.Text = $"Оберіть карту для гравця {ch.privius_plr.Name}";
                    }
                    break;
            }
        }
        void VisibleOrUnvisibleChoise(bool value)
        {
            answerButton.Visible = value;
            comboBox1.Visible = value;
            if (!value)
            {
                infoLabel.Text = String.Empty;
                label2.Text = String.Empty;
                comboBox2.Visible = value;
            }
            else
            {
                comboBox1.Items.Clear();
                foreach (Player p in AllPlayers)
                    if (!p.Equals(CurrentPlayer)) comboBox1.Items.Add(p.Name); 
            }
        }
    }
}