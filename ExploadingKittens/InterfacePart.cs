using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExploadingKittens;
using System.IO; 

namespace InterfaceWork
{
    static class ImageWorker
    {
        static public Game MyGame { get; set; }
        static private byte firstcardindex;
        static public byte Firscardindex { get { return firstcardindex; } }
        static public PictureBox OnTopBox = null; 
        static public void LoadImageFromCard(Card c, PictureBox pb)
        {
            if (OnTopBox != null)
                BackImage();
            byte num_card = c.Number; 
            if (c is Blank)
            {
                Blank bc = (Blank)c;
                num_card = (byte)bc._blank; 
            }
            pb.Load(@"Cards\" + c.ToString() + "\\"+ num_card.ToString()+".jpg");
        }
        static void ClearImages()
        {
            foreach (var b in MyGame.playerboxes)
                b.Image = null;
        }
        static public void LoadPlayerCards(Player p, byte fromcard = 0)
        {
            ClearImages();
            if (fromcard == 0)
                firstcardindex = 0; 
            if (p.Cards.Count > MyGame.playerboxes.Length)
                MyGame.TurnOnOrOffButtons(true);
            else MyGame.TurnOnOrOffButtons(false);

            foreach (var b in MyGame.playerboxes) b.Visible = true;
            int i = fromcard;
            for (int j = 0; j < MyGame.playerboxes.Length && j < p.Cards.Count-fromcard; j++, i++)
            LoadImageFromCard(p.Cards[i], MyGame.playerboxes[j]);
            if (i < MyGame.playerboxes.Length)
                MyGame.playerboxes[i].Visible = false;
        }
        public static void GoToRight(Player p)
        {
            if (firstcardindex < p.Cards.Count - MyGame.playerboxes.Length)
                LoadPlayerCards(p, ++firstcardindex);
        }
        public static void GoToLeft(Player p)
        {
            if (firstcardindex > 0)
                LoadPlayerCards(p, --firstcardindex);
        }
        public static void SetImageOnTop(PictureBox p)
        {
            if (OnTopBox != null)
            {
                if (OnTopBox.Equals(p))
                    return;
                BackImage();
            }
            p.Top = p.Top - p.Height;
            OnTopBox = p; 
        }
        public static void BackImage()
        {
            if (OnTopBox != null)
                OnTopBox.Top = OnTopBox.Top + OnTopBox.Height;
            OnTopBox = null;
        }
    }
}