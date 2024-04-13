using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trivial.CommandLine;
using Trivial.IO;
using Trivial.Tasks;
using Trivial.Text;

namespace RichTap;

/// <summary>
/// The command verb for vibration.
/// </summary>
public class VibrationCommandVerb : BaseCommandVerb
{
    /// <inheritdoc />
    protected override async Task OnProcessAsync(CancellationToken cancellationToken = default)
    {
        await RunAsync();
        Process();
    }

    internal void Process()
    {
        WriteLine();
        var controllers = WriteControllers();
        if (controllers.Count < 1) return;
        var file = GetFile();
        Play(file);
    }

    private void Play(FileInfo file)
    {
        var console = GetConsole();
        VibrationMotor.Play(file);
        console.WriteLine("Now playing. Press any key to stop.");
        console.ReadKey();
        console.WriteLine();
        VibrationMotor.Stop();
        console.WriteLine("Stopped.");
    }

    private FileInfo GetFile()
    {
        var file = Arguments?.GetFirst("file")?.Value;
        var console = GetConsole();
        if (string.IsNullOrWhiteSpace(file))
        {
            console.Write("File path > ");
            file = console.ReadLine();
            if (string.IsNullOrWhiteSpace(file))
            {
                console.Write("File path > ");
                file = console.ReadLine();
            }
        }

        if (string.IsNullOrWhiteSpace(file)) return null;
        var fileInfo = FileSystemInfoUtility.TryGetFileInfo(file);
        if (fileInfo != null && fileInfo.Exists) return fileInfo;
        console.Write(ConsoleColor.Red, "Error!");
        console.WriteLine(" The file does not exist.");
        return null;
    }

    private List<string> WriteControllers()
    {
        var console = GetConsole();
        List<string> controllers;
        try
        {
            controllers = VibrationMotor.ListGameControllers();
        }
        catch (AccessViolationException ex)
        {
            console.Write(ConsoleColor.Red, "Error!");
            console.WriteLine(" List game controllers failed.");
            console.WriteLine(ex.ToString());
            return new();
        }
        catch (InvalidOperationException ex)
        {
            console.Write(ConsoleColor.Red, "Error!");
            console.WriteLine(" List game controllers failed.");
            console.WriteLine(ex.ToString());
            return new();
        }

        if (controllers.Count < 1)
        {
            console.WriteLine(ConsoleColor.Yellow, "No controller connected.");
            return controllers;
        }

        var i = 0;
        foreach (var controller in controllers)
        {
            i++;
            console.Write(' ');
            console.Write(controller);
            if (i % 3 == 0) console.WriteLine();
            else console.Write(" \t");
        }

        if (i % 3 > 0) console.WriteLine();
        return controllers;
    }

    private void WriteLine()
    {
        var console = StyleConsole.Default;
        string version = null;
        try
        {
            version = VibrationMotor.Version();
        }
        catch (InvalidOperationException)
        {
        }

        console.WriteLine(string.IsNullOrWhiteSpace(version)
            ? "RichTap Vibration"
            : string.Concat("RichTap Vibration ", version));
    }
}
