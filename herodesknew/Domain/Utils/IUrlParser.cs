namespace herodesknew.Domain.Utils
{
    public interface IUrlParser
    {
        bool TryGetPullRequestNumberFromUrl(string url, out int pullRequestNumber);
    }
}
