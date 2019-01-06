using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sobokan
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Jeu jeu;

        public MainWindow()
        {
            DemarrerJeu();
        }

        private void DemarrerJeu()
        {
            InitializeComponent();
            jeu = new Jeu();

            //abonnement à l'évènement KeyDown (touche pressée)
            KeyDown += MainWindow_KeyDown;

            Dessiner();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Right) || e.Key.Equals(Key.Left) || e.Key.Equals(Key.Up) || e.Key.Equals(Key.Down))
            {
                jeu.ToucheAppuyee(e.Key);
                Redessiner();

                if (jeu.LevelComplete())
                {
                    if (jeu.Niveau < jeu.grilleTxt.Count)
                    {
                        MessageBoxResult msg = MessageBox.Show("Bravo ! Vous avez gagnez en " + jeu.Deplacement + " mouvements.\nPasser au niveau suivant", "Niveau Terminé", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        if (msg == MessageBoxResult.OK)
                        {
                            jeu.Niveau++;
                            jeu.Restart();
                            Dessiner();
                        }
                    }
                    else
                    {
                        MessageBoxResult msg = MessageBox.Show("Bravo ! Vous avez gagnez en " + jeu.Deplacement + " mouvements.\nJeu Terminé. Merci d'avoir jouer.", "Jeu Terminé", MessageBoxButton.OK, MessageBoxImage.Stop);
                        if (msg == MessageBoxResult.OK)
                        {
                            this.Close();
                        }
                    }
                }
            }
        }
        
        private void Redessiner()
        {
            cnvMobile.Children.Clear();
            DessinerCaisse();
            DessinerPersonnage();
            AffichernbDeplacements();
            AfficherNiveau();
        }

        private void AfficherNiveau()
        {
            int niveauAffiche = jeu.Niveau + 1;
            tbNiveau.Text = niveauAffiche.ToString();
        }

        private void AffichernbDeplacements()
        {
            tbNbDeplacements.Text = jeu.Deplacement.ToString();
        }

        private void Dessiner()
        {
            DessinerCarte();
            Redessiner();
        }

        private void DessinerPersonnage()
        {
            Position pos = jeu.Personnage;
            Rectangle r = new Rectangle();
            r.Width = 50;
            r.Height = 50;
            r.Margin = new Thickness(pos.y * 50, pos.x * 50, 0, 0);

            switch (jeu.NewDirection)
            {
                case Jeu.Direction.playerStart:
                    r.Fill = new ImageBrush(new BitmapImage(new Uri("images/playerStart.png", UriKind.Relative)));
                    break;
                case Jeu.Direction.playerDown:
                    r.Fill = new ImageBrush(new BitmapImage(new Uri("images/playerDown.png", UriKind.Relative)));
                    break;
                case Jeu.Direction.playerLeft:
                    r.Fill = new ImageBrush(new BitmapImage(new Uri("images/playerLeft.png", UriKind.Relative)));
                    break;
                case Jeu.Direction.playerRight:
                    r.Fill = new ImageBrush(new BitmapImage(new Uri("images/playerRight.png", UriKind.Relative)));
                    break;
                case Jeu.Direction.playerUp:
                    r.Fill = new ImageBrush(new BitmapImage(new Uri("images/playerUp.png", UriKind.Relative)));
                    break;
            }
            cnvMobile.Children.Add(r);
        }

        private void DessinerCaisse()
        {
            foreach(Position pos in jeu.Caisses)
            {
                Rectangle r = new Rectangle();
                r.Width = 50;
                r.Height = 50;
                r.Margin = new Thickness(pos.y * 50, pos.x * 50, 0, 0);
                r.Fill = new ImageBrush(new BitmapImage(new Uri("images/caisse.png", UriKind.Relative)));
                cnvMobile.Children.Add(r);
            }
        }

        private void DessinerCarte()
        {
            cnvGrille.Children.Clear();
            for (int ligne = 0; ligne < 10; ligne++)
            {
                for (int colonne = 0; colonne < 10; colonne++)
                {
                    Rectangle r = new Rectangle();
                    r.Height = 50;
                    r.Width = 50;
                    r.Margin = new Thickness(colonne * 50, ligne * 50, 0, 0);
                    
                    switch(jeu.Case(ligne,colonne))
                    {
                        case Jeu.Etat.Vide:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/sol.png", UriKind.Relative)));
                            break;
                        case Jeu.Etat.Mur:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/mur.png", UriKind.Relative)));
                            break;
                        case Jeu.Etat.Cible:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/cible.png", UriKind.Relative)));
                            break;
                    }

                    cnvGrille.Children.Add(r);
                }
            }
        }

        private void btnRecommencer_Click(object sender, RoutedEventArgs e)
        {
            jeu.Restart();
            Redessiner();
        }
    }
}
