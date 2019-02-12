using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Lettertype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random randomGenerator;
        private string[] fonts;
        private int[] usedFonts;
        private int usedFontsIndex;

        public MainWindow()
        {
            InitializeComponent();

            randomGenerator = new Random();
            fonts = new string[Fonts.SystemFontFamilies.Count];
            usedFonts = new int[Fonts.SystemFontFamilies.Count];

            FillFontsArray();
        }

        private void ButtonChangeFont_Click(object sender, RoutedEventArgs e)
        {
            char[] richtextboxChar = new TextRange(richttextboxTextInput.Document.ContentStart, richttextboxTextInput.Document.ContentEnd).Text.ToCharArray();
            TextPointer textpointerStart;
            TextRange textRangeTextInput;
            int fontsIndex = 0;
            string character;
            object fontValue;

            // Iterates through each character in richtextbox
            for (int characterIndex = 0; characterIndex < richtextboxChar.Length - 2; characterIndex++)
            {
                fontsIndex = GetUniqueValue();
                fontValue = fonts[fontsIndex];
                character = richtextboxChar[characterIndex].ToString();
                // Sets startindex by moving 1 index forward
                textpointerStart = richttextboxTextInput.Document.ContentStart;//.GetPositionAtOffset(characterIndex);
                textRangeTextInput = GetCharacterFromPosition(textpointerStart, character);
                textRangeTextInput.ClearAllProperties();
                textRangeTextInput.ApplyPropertyValue(TextElement.FontFamilyProperty, fontValue);

                // Gives feedback for debugging
                //MessageBox.Show(fontValue.ToString(), textRangeTextInput.Text);
            }
        }

        // Gets the specific character
        TextRange GetCharacterFromPosition(TextPointer position, string character)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    // Find the starting index of any substring that matches "character".
                    int indexInRun = textRun.IndexOf(character);
                    if (indexInRun >= 0)
                    {
                        TextPointer start = position.GetPositionAtOffset(indexInRun);
                        TextPointer end = start.GetPositionAtOffset(character.Length);
                        return new TextRange(start, end);
                    }
                }

                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            // Sets position to null if "character" is not found.
            return null;
        }

        private int GetUniqueValue()
        {
            int uniqueValue = 0;

            if (usedFontsIndex < fonts.Length)
            {
                do
                {
                    uniqueValue = randomGenerator.Next(1, fonts.Length);
                } while (usedFonts.Contains(uniqueValue) && uniqueValue ==0);
                usedFonts[usedFontsIndex] = uniqueValue;
                usedFontsIndex++;
            }
            else
            {
                Array.Clear(usedFonts, 0, Fonts.SystemFontFamilies.Count);
                usedFontsIndex = 0;
                uniqueValue = randomGenerator.Next(1, fonts.Length);
            }

            return uniqueValue;
        }

        private void FillFontsArray()
        {
            int index = 0;

            // Fills fotns array with all fonts
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                // FontFamily.Source contains the font family name.
                fonts[index] = fontFamily.Source;
                index++;
            }
        }
    }
}