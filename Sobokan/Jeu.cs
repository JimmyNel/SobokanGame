using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sobokan
{
    public class Jeu
    {
        // définition d'une énumération d'état d'une case de la grille de jeu (qui s'apparente à une tableau)
        public enum Etat
        {
            Vide,
            Mur,
            Cible
        }

        // enumération des différentes directions du personnage
        public enum Direction
        {
            playerStart,
            playerDown,
            playerUp,
            playerLeft,
            playerRight
        }

        private Direction newDirection;

        private int deplacement = 0;

        private int niveau = 0;
        
        // définition de la grille de jeu 2 dimensions
        private Etat[,] grille;

        // définition liste des positions des caisses
        private List<Position> caisses;

        // définition de la position de départ du personnage
        private Position personnage;

        // chaine de caractère à parser : grille jeu sous forme de liste de string
        //static string grilleTxt = "..XXXXXX..XXX.oo.XXXX..o..o..XX........XXXX....XXX..XX.CXX...XXXC.XXX..X.CP.C.X..X......X..XXXXXXXX.";
        public List<string> grilleTxt = new List<string>();

        public List<Position> Caisses { get => caisses; }

        internal bool LevelComplete()
        {
            foreach (Position caisse in caisses)
            {
                if(grille[caisse.x, caisse.y] != Etat.Cible)
                {
                    return false;
                }
            }
            return true;          
        }

        public Position Personnage { get => personnage; }
        public Direction NewDirection { get => newDirection; }
        public int Deplacement { get => deplacement; }
        public int Niveau { get => niveau; set => niveau = value; }

        public void ToucheAppuyee(Key key)
        {
            Position NewPos = new Position(personnage.x, personnage.y);
            CalculNewPos(NewPos, key);

            if (CaseOK(NewPos, key))
            {
                personnage = NewPos;
                deplacement++;
            }
        }

        private void CalculNewPos(Position NewPos, Key key)
        {            
            switch (key)
            {
                case Key.Down:
                    NewPos.x++;
                    newDirection = Direction.playerDown;
                    break;
                case Key.Up:
                    NewPos.x--;
                    newDirection = Direction.playerUp;
                    break;
                case Key.Left:
                    NewPos.y--;
                    newDirection = Direction.playerLeft;
                    break;
                case Key.Right:
                    NewPos.y++;
                    newDirection = Direction.playerRight;
                    break;
                default:
                    newDirection = Direction.playerStart;
                    break;
            }
        }

        private bool CaseOK(Position newPos, Key key)
        {
            // présence d'un mur
            if (grille[newPos.x, newPos.y] == Etat.Mur)
            {
                return false;
            }
            // Présence d'une caisse
            Position caisse = CaisseInPos(newPos);
            if (CaisseInPos(newPos) != null)
            {
                // Déplacement de la caisse possible ?
                Position newPosCaisse = new Position(caisse.x, caisse.y);
                CalculNewPos(newPosCaisse, key);

                if (grille[newPosCaisse.x, newPosCaisse.y] == Etat.Mur)
                {
                    return false;
                }
                else if (CaisseInPos(newPosCaisse) != null)
                {
                    return false;
                }
                else
                {
                    caisse.x = newPosCaisse.x;
                    caisse.y = newPosCaisse.y;
                    return true;
                }
            }
            // pas d'obstacle
            return true;
        }

        private Position CaisseInPos(Position newPos)
        {
            foreach (Position caisse in caisses)
            {
                if (caisse.x == newPos.x && caisse.y == newPos.y)
                {
                    return caisse;
                }
            }
            return null;
        }

        private void InitGrille()
        {
            grilleTxt.Add("..XXXXXX..XXX.oo.XXXX..o..o..XX........XXXX....XXX..XX.CXX...XXXC.XXX..X.CP.C.X..X......X..XXXXXXXX.");
            grilleTxt.Add("...........XXXXXXXX..X..o.XXX..X....XXX..X.XCCo.X..Xo..XX.X..XPC.XX.X..XXX....X..XXXXXXXX...........");
                    
        }

        // Constructeur de jeu
        public Jeu()
        {
            grille = new Etat[10, 10];
            InitGrille();
            InitCarte();         
        }

        private void InitCarte()
        {
            // créer un liste vide de caisse
            caisses = new List<Position>();

            // pour chaque case, initialiser la bonne valeur / état
            // ajouter les caisses + déterminer la position de départ du personnage
            for (int ligne = 0; ligne < 10; ligne++)
            {
                for (int colonne = 0; colonne < 10; colonne++)
                {
                    switch(grilleTxt.ElementAt(niveau)[ligne*10 + colonne])
                    {
                        case '.':
                            grille[ligne, colonne] = Etat.Vide;
                            break;
                        case 'X':
                            grille[ligne, colonne] = Etat.Mur;
                            break;
                        case 'o':
                            grille[ligne, colonne] = Etat.Cible;
                            break;
                        case 'C':
                            caisses.Add(new Position(ligne, colonne));
                            grille[ligne, colonne] = Etat.Vide;
                            break;
                        case 'P':
                            personnage = new Position(ligne, colonne);
                            grille[ligne, colonne] = Etat.Vide;
                            break;
                    }
                }
            }   
        }

        public void Restart()
        {
            InitCarte();
            deplacement = 0;
            
        }

        public Etat Case(int ligne, int colonne)
        {
            return grille[ligne, colonne];
        }
    }
}
