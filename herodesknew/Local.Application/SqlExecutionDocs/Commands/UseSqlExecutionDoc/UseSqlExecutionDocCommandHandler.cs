using herodesknew.Local.Domain.Utils;
using herodesknew.Shared;
using System.Runtime.InteropServices;

namespace herodesknew.Local.Application.SqlExecutionDocs.Commands.UseSqlExecutionDoc
{
    public sealed class UseSqlExecutionDocCommandHandler
    {
        public Result Handle(UseSqlExecutionDocCommand useSqlExecutionDocCommand)
        {
            var sqlFile = TicketFolderManager
                .GetTicketSubDirectoriesWithScripts(new[] { useSqlExecutionDocCommand.TicketId })
                .SelectMany(item => item)
                .Where(sqlFile => sqlFile.Id == useSqlExecutionDocCommand.SqlFileId)
                .Select(sqlFile => $"{sqlFile.PathFullName}\\{SqlExecutionPlanDoc.FileName}")
                .SingleOrDefault();

            if (!File.Exists(sqlFile))
                return Result.Failure(new Error("SqlFile.Open", "File don´t Found"));

            var targetPath = Path.Combine(SqlExecutionPlanDoc.PathDefaultToUse, SqlExecutionPlanDoc.FileName);

            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }

            File.Copy(sqlFile, targetPath);
            KeyboardSimulator.SimulateCtrlAlt9Shortcut();
            return Result.Success(sqlFile);
        }
    }
    public static class KeyboardSimulator
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const int VK_CONTROL = 0x11;
        private const int VK_ALT = 0x12;
        private const int VK_9 = 0x39;
        private const int KEYEVENTF_KEYDOWN = 0;
        private const int KEYEVENTF_KEYUP = 2;

        public static void SimulateCtrlAlt9Shortcut()
        {
            // Aguarde um momento para garantir que a aplicação alvo tenha o foco
            System.Threading.Thread.Sleep(2000);

            // Simule o pressionamento de Ctrl
            keybd_event((byte)VK_CONTROL, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);

            // Simule o pressionamento de Alt
            keybd_event((byte)VK_ALT, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);

            // Simule o pressionamento de 8
            keybd_event((byte)VK_9, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);

            // Simule a liberação de 8
            keybd_event((byte)VK_9, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);

            // Simule a liberação de Alt
            keybd_event((byte)VK_ALT, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);

            // Simule a liberação de Ctrl
            keybd_event((byte)VK_CONTROL, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);

            Console.WriteLine("Ctrl + Alt + 9 enviado para a aplicação ativa.");
        }
    }

}
