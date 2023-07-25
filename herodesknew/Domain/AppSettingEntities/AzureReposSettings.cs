using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Domain.AppSettingEntities
{
    public sealed class AzureReposSettings
    {
        public required string UrlBase { get; set; }
        public required string Token { get; set; }
        public required string ProjName { get; set; }
        public required string RepoName { get; set; }

        public AzureReposSettings()
        {
        }
    }
}
