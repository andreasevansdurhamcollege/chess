using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ChessGame
{
    /// <summary>
    /// Interaction logic for PromotionWindow.xaml
    /// </summary>
    public partial class PromotionWindow : Window
    {
        public string SelectedPiece { get; private set; }

        public PromotionWindow(Player owner)
        {
            InitializeComponent();
            CustomizePromotionImages(owner);
        }

        /// <summary>
        /// Customizes the promotion images based on the player's color.
        /// </summary>
        /// <param name="owner">The player performing the promotion.</param>
        private void CustomizePromotionImages(Player owner)
        {
            string color = owner == Player.White ? "white" : "black";

            // Update Queen Image
            QueenImage.Source = new BitmapImage(new Uri($"Images/{color}_queen.png", UriKind.Relative));

            // Update Rook Image
            RookImage.Source = new BitmapImage(new Uri($"Images/{color}_rook.png", UriKind.Relative));

            // Update Bishop Image
            BishopImage.Source = new BitmapImage(new Uri($"Images/{color}_bishop.png", UriKind.Relative));

            // Update Knight Image
            KnightImage.Source = new BitmapImage(new Uri($"Images/{color}_knight.png", UriKind.Relative));
        }

        /// <summary>
        /// Event handler for Queen promotion.
        /// </summary>
        private void QueenButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPiece = "Q";
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Event handler for Rook promotion.
        /// </summary>
        private void RookButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPiece = "R";
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Event handler for Bishop promotion.
        /// </summary>
        private void BishopButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPiece = "B";
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Event handler for Knight promotion.
        /// </summary>
        private void KnightButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPiece = "N";
            this.DialogResult = true;
            this.Close();
        }
    }
}
