using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanguageIdentifierByLetterFrequency
{
    class Program
    {
        //Dictionary of each language
        static Dictionary<char, double> DictEnglish;
        static Dictionary<char, double> DictGerman;
        static Dictionary<char, double> DictSpanish;
        static Dictionary<char, double> DictText;

        static void Main(string[] args)
        {
           DictionaryInit();
           String patito = Parser("por eso casi al final se saca el promedio de esas desviaciones para ver cual es el mas bajo y entonces decir de que idioma es");
           Console.Write(patito);
           Console.Read();
        }

        private static void DictionaryInit() {
            DictEnglish = new Dictionary<char, double>()
	                {{'a', 0.06516}, {'b', 0.01886}, {'c', 0.02732}, {'d', 0.05076}, {'e', 0.17396}, {'f', 0.01656}, {'g', 0.03009},
                     {'h', 0.04757}, {'i', 0.07550}, {'j', 0.00268}, {'k', 0.01417}, {'l', 0.03437}, {'m', 0.02534}, {'n', 0.09776},
                     {'o', 0.02594}, {'p', 0.00670}, {'q', 0.00018}, {'r', 0.07003}, {'s', 0.07273}, {'t', 0.06154}, {'u', 0.04346},
                     {'v', 0.00846}, {'w', 0.01921}, {'x', 0.00034}, {'y', 0.00039}, {'z', 0.01134}, {'á', 0.00000}, {'ä', 0.00447}, 
                     {'é', 0.00000}, {'í', 0.00000}, {'ñ', 0.00000}, {'ö', 0.00573}, {'ó', 0.00000}, {'ß', 0.00307}, {'ú', 0.00995},
                     {'ü', 0.00995}};

            DictGerman = new Dictionary<char, double>()
	                {{'a', 0.08167}, {'b', 0.01492}, {'c', 0.02782}, {'d', 0.04253}, {'e', 0.12702}, {'f', 0.02228}, {'g', 0.02015},
                     {'h', 0.06094}, {'i', 0.06966}, {'j', 0.00153}, {'k', 0.00772}, {'l', 0.04025}, {'m', 0.02406}, {'n', 0.06749},
                     {'o', 0.07507}, {'p', 0.01929}, {'q', 0.00095}, {'r', 0.05987}, {'s', 0.06327}, {'t', 0.09056}, {'u', 0.02758},
                     {'v', 0.00978}, {'w', 0.02360}, {'x', 0.00150}, {'y', 0.01974}, {'z', 0.00074}, {'á', 0.00000}, {'ä', 0.00000}, 
                     {'é', 0.00000}, {'í', 0.00000}, {'ñ', 0.00000}, {'ö', 0.00000}, {'ó', 0.00000}, {'ß', 0.00000}, {'ú', 0.00000},
                     {'ü', 0.00000}};

            DictSpanish = new Dictionary<char, double>()
                    {{'a', 0.12525}, {'b', 0.02215}, {'c', 0.04139}, {'d', 0.05860}, {'e', 0.13681}, {'f', 0.00692}, {'g', 0.01768},
                     {'h', 0.00703}, {'i', 0.06247}, {'j', 0.00443}, {'k', 0.00011}, {'l', 0.04967}, {'m', 0.03157}, {'n', 0.06712},
                     {'o', 0.08683}, {'p', 0.02510}, {'q', 0.00877}, {'r', 0.06871}, {'s', 0.07977}, {'t', 0.04632}, {'u', 0.03927},
                     {'v', 0.01138}, {'w', 0.00017}, {'x', 0.00215}, {'y', 0.01008}, {'z', 0.00517}, {'á', 0.00502}, {'ä', 0.00000}, 
                     {'é', 0.00433}, {'í', 0.00725}, {'ñ', 0.00311}, {'ö', 0.00000}, {'ó', 0.00827}, {'ß', 0.00000}, {'ú', 0.00168},
                     {'ü', 0.00012}};
        }

        private static String Parser(String Text)
        {
            DictText = new Dictionary<char, double>();
            
            //Cleaning blanks and numbers in text
            Text = Text.ToLower();
            String TextClean = "";
            string Letters = "abcdefghijklmnopqrstuvwxyzáäéíñöóßúü";
            foreach (char Letter in Text)
                if (Letters.IndexOf(Letter) != -1)
                    TextClean += Letter;

            //Parsing the text
            int TotalCleanLenght = TextClean.Length;
            while (TextClean.Length > 0){
                    //Save the actual lenght
                    int OldLenght = TextClean.Length;
                    //Save the actual letter
                    char ActualLetter = TextClean[0];
                    //Remove the actual letter
                    TextClean = TextClean.Replace(ActualLetter.ToString(), string.Empty);
                    //Create the dictionary
                    DictText.Add(ActualLetter, ((OldLenght - (double)TextClean.Length) / (double)TotalCleanLenght));}

            //Finding the language
            //Standard deviation
            double deviationEnglish = 0, deviationGerman = 0, deviationSpanish = 0;
            foreach (KeyValuePair<char, double> entry in DictText.OrderBy(Letter => Letter.Key))
            {
                deviationEnglish += Math.Pow((DictEnglish[entry.Key] - entry.Value), 2);
                deviationGerman += Math.Pow((DictGerman[entry.Key] - entry.Value), 2);
                deviationSpanish += Math.Pow((DictSpanish[entry.Key] - entry.Value), 2);

            }
            //Average
            deviationEnglish /= DictText.Count;
            deviationGerman /= DictText.Count;
            deviationSpanish /= DictText.Count;

            //Result
            String Result = "Please insert more words to better results";
            if (deviationEnglish < deviationGerman && deviationEnglish < deviationSpanish)
                Result = "English";
            else if (deviationGerman < deviationEnglish && deviationGerman < deviationSpanish)
                Result = "German";
            else if (deviationSpanish < deviationEnglish && deviationSpanish < deviationGerman)
                Result = "Spanish";
            return Result;
        }

        public static DataTable DictionaryToDatatable(Dictionary<char, double> Dictionary)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Letter", typeof(char));
            table.Columns.Add("Frequency", typeof(double));
            foreach (KeyValuePair<char, double> entry in DictText.OrderBy(Letter => Letter.Key))
            {
                table.Rows.Add(entry.Key, entry.Value);
            }
	        return table;
        }
    }
}
