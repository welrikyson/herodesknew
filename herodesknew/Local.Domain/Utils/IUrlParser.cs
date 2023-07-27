namespace herodesknew.Local.Domain.Utils
{
    public interface IUrlParser
    {
        bool TryGetPullRequestNumberFromUrl(string url, out int pullRequestNumber);
    }
}
