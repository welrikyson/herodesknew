using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using herodesknew.Domain.Entities;
using herodesknew.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace herodesknew.Application.Local
{
    public class TicketSqlFileDiscovery
    {
        private readonly string _fileName;
        private readonly string _planoDeployfilePath;
        private readonly ISpreadsheetHelper _speadsheetHelper;
        private readonly SqlExecutionPlanDoc _SqlExecutionPlanDoc;

        public TicketSqlFileDiscovery(ISpreadsheetHelper speadsheetHelper)
        {
            _fileName = "";//configuration["Files:PlanoDeploy"]!;
            _planoDeployfilePath = Path.Combine(Environment.CurrentDirectory, _fileName);
            _speadsheetHelper = speadsheetHelper;
            _SqlExecutionPlanDoc = new SqlExecutionPlanDoc(speadsheetHelper);
        }
        public string CreateSqlFileFor(int ticketId)
        {
            string idChamado = ticketId.ToString();
            string pastaBase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MS\\src");

            string pastaChamado = Path.Combine(pastaBase, "HD", @$"{DateTime.Now:yyyy\\MM}", idChamado.ToString());

            string pastaSlt = Path.Combine(pastaChamado, "SLT");

            string[] subpastas = Directory.Exists(pastaSlt) ?
                Directory.GetDirectories(pastaSlt)
                : Array.Empty<string>();

            var nextSln = subpastas.Select(Path.GetFileName)
                                       .Where(nome => int.TryParse(nome, out _))
                                       .Select(nome => int.Parse(nome!))
                                       .DefaultIfEmpty()
                                       .Max() + 1;

            string pastaProximoSln = Path.Combine(pastaSlt, nextSln.ToString());

            string arquivoSql = Path.Combine(pastaProximoSln, $"{idChamado}--{nextSln}.sql");
            // Cria a pasta do chamado se não existir
            //Directory.CreateDirectory(pastaChamado);
            Directory.CreateDirectory(pastaProximoSln);

            File.WriteAllText(arquivoSql, $"""
                                   --HD {idChamado}
                                   USE Corporate1
                                   """);
            return arquivoSql;
        }

        public string? GetSolutionFolder(int idTicket)
        {
            var pastaBase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MS\\src");
            var pastaTicket = EncontrarPastaPorID(Path.Combine(pastaBase, "HD"), idTicket);
            if (pastaTicket == null) return null;
            return Path.Combine(pastaTicket, "SLT");
        }
        static string? EncontrarPastaPorID(string pastaBase, int idProcurado)
        {
            Queue<string> fila = new Queue<string>();
            fila.Enqueue(pastaBase);

            while (fila.Count > 0)
            {
                string pastaAtual = fila.Dequeue();
                string[] subpastas = Directory.GetDirectories(pastaAtual);

                foreach (string subpasta in subpastas)
                {
                    string nomeSubpasta = Path.GetFileName(subpasta);
                    int id;

                    if (int.TryParse(nomeSubpasta, out id))
                    {
                        if (id == idProcurado)
                        {
                            return subpasta; // Pasta encontrada!
                        }
                    }

                    fila.Enqueue(subpasta);
                }
            }

            return null; // Pasta não encontrada
        }

        public string? GetSqlFileContent(int ticketId, int sqlFileId)
        {
            var solutionFolder = GetSolutionFolder(ticketId);

            if (solutionFolder == null)
            {
                return null;
            }

            string sqlFilePath = $"{solutionFolder}\\{sqlFileId}\\{ticketId}--{sqlFileId}.sql";
            if (!File.Exists(sqlFilePath))
            {
                return null;
            }
            var title = ReadFirstLineFromFile(sqlFilePath);


            if (title == null)
            {
                return null;
            }

            string padrao = @"--HD (\d+) - (.+)";
            Match match = Regex.Match(title, padrao);

            if (!match.Success)
            {
                return null;
            }

            using StreamReader reader = new(sqlFilePath, Encoding.Latin1);

            return reader.ReadToEnd();
        }

        public void OpenSqlFile(int ticketId, int sqlId)
        {
            var solutionFolder = GetSolutionFolder(ticketId);

            if (solutionFolder == null)
            {
                return;
            }

            var sqlFile = $"{solutionFolder}\\{sqlId}\\{ticketId}--{sqlId}.sql";

            Process.Start(new ProcessStartInfo
            {
                FileName = sqlFile,
                UseShellExecute = true
            });
        }

        public static string? ReadFirstLineFromFile(string path)
        {
            using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using StreamReader streamReader = new StreamReader(fileStream);
            // Lê a primeira linha do arquivo
            string? primeiraLinha = streamReader.ReadLine();
            return primeiraLinha;
        }
    }

    public class SqlExecutionPlanDoc
    {
        public static readonly string RefCellPullRequestUrl = "D44";
        public static readonly string refCellMotivo = "D11";

        private readonly ISpreadsheetHelper _speadsheetHelper;
        private string _fileName;
        private readonly string _planoDeployfilePath;

        public SqlExecutionPlanDoc(ISpreadsheetHelper speadsheetHelper)
        {
            _fileName = "";//configuration["Files:PlanoDeploy"]!;
            _planoDeployfilePath = Path.Combine(Environment.CurrentDirectory, _fileName);
            _speadsheetHelper = speadsheetHelper;
        }
        public void CreateDeployDocAsync(string pastaDestino, int solutionId, string ticktTitle, string pullRequestUrl)
        {            
            string caminhoDestino = Path.Combine(pastaDestino, _fileName);

            if (File.Exists(caminhoDestino))
            {
                File.Delete(caminhoDestino);
                Console.WriteLine("Arquivo existente removido.");
            }

            File.Copy(_planoDeployfilePath, caminhoDestino);

            FillPlanoDeploySheet(pullRequestUrl, ticktTitle, caminhoDestino);            
        }

        public void FillPlanoDeploySheet(string valueCellPullRequestUrl, string valueCellmotivo, string caminhoDestino)
        {
            using var document = SpreadsheetDocument.Open(caminhoDestino, true);

            var worksheetPart = document.WorkbookPart?.WorksheetParts.Last();
            var cells = worksheetPart?.Worksheet.Descendants<Cell>();

            _speadsheetHelper.UpdateCellValue(cells, RefCellPullRequestUrl, valueCellPullRequestUrl);
            _speadsheetHelper.UpdateCellValue(cells, refCellMotivo, valueCellmotivo);

            worksheetPart?.Worksheet.Save(); // save changes to the worksheet      
        }
    }
}
