namespace LexAn
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string filePath = @".\source.scr";
                string fileContents = File.ReadAllText(filePath);
                //Console.WriteLine(fileContents);
                Console.WriteLine("\n\nIniciando Analisis lexico ----\n");
                Lexer lexer = new Lexer(fileContents);
                Token[] tks = lexer.LexerAna(out int[] invLex);
                Console.WriteLine("Resultado de analisis: " + (invLex.Length > 0 ? "Codigo invalido" : "Codigo Valido"));
                if(invLex.Length > 0)
                {
                    Console.WriteLine("Tokens invalidos: ");
                    for (int i = 0; i < invLex.Length; i++)
                    {
                        Console.WriteLine(fileContents.Substring(tks[invLex[i]].from, tks[invLex[i]].to - tks[invLex[i]].from));
                    }

                    Console.WriteLine("\n\n---------------------------------");
                    Console.WriteLine("Tokens validos: ");
                }
                
                for (int i = 0; i < tks.Length; i++)
                {
                    if(!invLex.Contains(i))
                        Console.WriteLine(fileContents.Substring(tks[i].from, tks[i].to - tks[i].from) + "\n\t" + tks[i].GetInfo());
                }

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File source.scr not found");
            }
        }
    }
}