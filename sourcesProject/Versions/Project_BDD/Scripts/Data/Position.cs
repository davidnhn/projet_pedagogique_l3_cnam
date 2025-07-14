using Godot;

namespace JeuVideo.Data
{
    /// <summary>
    /// Représente une position dans le jeu.
    /// </summary>
    public class Position : Resource
    {
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public Position()
        {
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Déplace la position.
        /// </summary>
        /// <param name="dx">Déplacement en X</param>
        /// <param name="dy">Déplacement en Y</param>
        public void Deplacer(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        /// <summary>
        /// Calcule la distance avec une autre position.
        /// </summary>
        /// <param name="autre">L'autre position</param>
        /// <returns>La distance entre les deux positions</returns>
        public double DistanceAvec(Position autre)
        {
            int dx = X - autre.X;
            int dy = Y - autre.Y;
            return System.Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
} 