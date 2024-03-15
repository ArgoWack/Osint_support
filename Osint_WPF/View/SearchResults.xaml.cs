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
using System.Xml.Linq;
using System.IO;
using System.Reflection.Metadata;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;

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
            var paragraph = new System.Windows.Documents.Paragraph();
            paragraph.Inlines.Add(new Run(resultsText));
            ResultsRichTextBox.Document.Blocks.Clear();
            ResultsRichTextBox.Document.Blocks.Add(paragraph);
        }
        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // place for results to be displayed
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //save to pdf

            // creates a new MigraDoc document
            var document = new MigraDoc.DocumentObjectModel.Document();
            var section = document.AddSection();
            var paragraph = section.AddParagraph();
            paragraph.AddFormattedText(_resultsText, TextFormat.Bold);

            // renders the document to PDF
            var renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();

            // defines the path to save the PDF
            string filename = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "OSINTSearchResults.pdf");
            renderer.PdfDocument.Save(filename);

            MessageBox.Show($"PDF has been saved to: {filename}");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Return button
            var BeginDataSearch = new BeginDataSearch();
            BeginDataSearch.Show();
            this.Close();
        }
    }
}