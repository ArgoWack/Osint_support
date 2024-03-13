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
using System.Windows.Shapes;

namespace Osint_WPF.View
{
    /// <summary>
    /// Logika interakcji dla klasy SearchResults.xaml
    /// </summary>
    public partial class SearchResults : Window
    {
        private string _resultsText;
        public SearchResults(string resultsText)
        {
            InitializeComponent();
            _resultsText = resultsText;
            DisplayResultsInRichTextBox(_resultsText);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayResultsInRichTextBox(_resultsText);
        }
        private void DisplayResultsInRichTextBox(string resultsText)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(resultsText));
            ResultsRichTextBox.Document.Blocks.Clear();
            ResultsRichTextBox.Document.Blocks.Add(paragraph);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Return button
            var BeginDataSearch = new BeginDataSearch();
            BeginDataSearch.Show();
            this.Close();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // place for results to be displayed
        }
    }
}
