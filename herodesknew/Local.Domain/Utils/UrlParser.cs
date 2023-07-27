using System.Text.RegularExpressions;

namespace herodesknew.Local.Domain.Utils;

public class UrlParser : IUrlParser
{
    public bool TryGetPullRequestNumberFromUrl(string url, out int pullRequestNumber)
    {
        const string pattern = @"/pullrequest/(\d+)";
        var match = Regex.Match(url, pattern);

        if (!match.Success || !int.TryParse(match.Groups[1].Value, out pullRequestNumber))
        {
            pullRequestNumber = 0;
            return false;
        }

        return true;
    }
}
