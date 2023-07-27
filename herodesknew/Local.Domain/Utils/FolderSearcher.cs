namespace herodesknew.Local.Domain.Utils;

public class FolderSearcher
{
    public static string FindFolderInDirectory(string targetFolderName, string rootDirectory)
    {
        Queue<string> folderQueue = new Queue<string>();
        folderQueue.Enqueue(rootDirectory);

        while (folderQueue.Count > 0)
        {
            string currentFolder = folderQueue.Dequeue();
            string[] subFolders = Directory.GetDirectories(currentFolder);

            foreach (string subFolder in subFolders)
            {
                string folderName = Path.GetFileName(subFolder);

                if (folderName == targetFolderName)
                {
                    return subFolder; // Pasta encontrada!                        
                }

                folderQueue.Enqueue(subFolder);
            }
        }

        return null; // Pasta não encontrada
    }
}
