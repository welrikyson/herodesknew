using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using herodesknew.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace herodesknew.Infrastructure.Data.Repositories
{
    public sealed class PullRequestReponsitory
    {
        private readonly string _fileName;

        public PullRequestReponsitory(IConfiguration configuration)
        {
            _fileName = configuration["Files:PlanoDeploy"]!;

        }

        public IEnumerable<PullRequest>? GetSolutions(int idTicket)
        {
            var pastaSlt = GetSolutionFolder(idTicket);

            if (pastaSlt == null) return null;

            string[] subpastas = Directory.Exists(pastaSlt) ?
                Directory.GetDirectories(pastaSlt)
                : Array.Empty<string>();

            var idSolutions = subpastas.Select(Path.GetFileName)
                                       .Where(nome => int.TryParse(nome, out _))
                                       .Select(nome => int.Parse(nome!));
            if (!idSolutions.Any()) return null;

            var pullRequests = new List<PullRequest>();
            foreach (var idSolution in idSolutions)
            {
                string planoDeploy = Path.Combine(pastaSlt, $"{idSolution}", _fileName);

                string? pullRequestUrl = null;
                if (File.Exists(planoDeploy))
                {
                    const string RefCellPullRequestUrl = "D44";

                    using var document = SpreadsheetDocument.Open(planoDeploy, true);

                    var worksheetPart = document.WorkbookPart?.WorksheetParts.Last();
                    var cells = worksheetPart?.Worksheet.Descendants<Cell>();
                    var cellPullRequest = cells?.FirstOrDefault(c => c.CellReference == RefCellPullRequestUrl)?.CellValue;
                    pullRequestUrl = cellPullRequest?.Text;

                }

                if(pullRequestUrl != null )
                {
                    var pullRequestNumber = GetPullRequestNumberFromUrl(pullRequestUrl);
                    if(pullRequestNumber != null )
                    {
                        var pullRequest = new PullRequest()
                        {
                            TicketId = idTicket,
                            Id = pullRequestNumber.Value
                        };

                        pullRequests.Add(pullRequest);
                    }                    
                }
            }
            return pullRequests;
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

        public int? GetPullRequestNumberFromUrl(string url)
        {
            string pattern = @"/pullrequest/(\d+)";
            Match match = Regex.Match(url, pattern);

            if (!match.Success)
            {
                return null;
            }

            return int.TryParse(match.Groups[1].Value, out int pullRequestNumber) ? 
                pullRequestNumber : 
                null;
        }
        
    }
}
