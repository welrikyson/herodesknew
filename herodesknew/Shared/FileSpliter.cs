using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Shared
{
    public static class FileSpliter
    {
        public static void Split(string inputFilePath, int linesToRead)
        {
            try
            {
                string inputDirectory = Path.GetDirectoryName(inputFilePath)!;
                string extension = Path.GetExtension(inputFilePath);
                string inputFileName = Path.GetFileNameWithoutExtension(inputFilePath);

                using (StreamReader reader = new(inputFilePath))
                {
                    for (var fileCounter = 1; !reader.EndOfStream; fileCounter++)
                    {
                        string outputFileName = Path.Combine(inputDirectory, $"{inputFileName}--pt{fileCounter}.{extension}");
                        using (StreamWriter writer = new(outputFileName))
                        {
                            for (int i = 0; i < linesToRead && !reader.EndOfStream; i++)
                            {
                                var line = reader.ReadLine();
                                writer.WriteLine(line);
                            }
                        }
                    }
                }

                Console.WriteLine("Linhas lidas e salvas em arquivos individuais com sucesso!");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Erro ao ler ou gravar no arquivo: {ex.Message}");
            }
        }
    }
}
