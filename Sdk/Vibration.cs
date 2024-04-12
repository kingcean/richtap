using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trivial.Text;

namespace RichTap;

/// <summary>
/// The vibration motor.
/// </summary>
public static class VibrationMotor
{
    private static bool init;
    private static readonly object obj = new();

    static VibrationMotor()
    {
        try
        {
            VibrationMotorWrapper.Init();
            init = true;
            VibrationMotorWrapper.SetCallback(OnGameControllerChange);
        }
        catch (ExternalException)
        {
        }
    }

    /// <summary>
    /// Adds or removes the event handler on a game controller is attached or disattached.
    /// </summary>
    public static event EventHandler<EventArgs> GameControllerChanged;

    /// <summary>
    /// Gets the initialization state of the SDK.
    /// </summary>
    public static bool GetInitState => init;

    /// <summary>
    /// Sets initialization state of the SDK.
    /// </summary>
    /// <param name="initialize">true if initializes; otherwise, false.</param>
    /// <exception cref="InvalidOperationException">Failed to initialize.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void SetInitState(bool initialize)
    {
        if (initialize == init) return;
        try
        {
            if (initialize) VibrationMotorWrapper.Init();
            else VibrationMotorWrapper.Destroy();
            init = initialize;
        }
        catch (ExternalException ex)
        {
            throw new InvalidOperationException("Failed to set initialization state.", ex);
        }
    }

    /// <summary>
    /// Gets the internal SDK version of RichTap.
    /// </summary>
    /// <returns>The version.</returns>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static string Version()
        => VibrationMotorWrapper.GetVersion();

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="NotSupportedException">Not supported.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(string he, VibrationOptions options = null)
    {
        if (string.IsNullOrEmpty(he)) return;
        options ??= new();
        try
        {
            VibrationMotorWrapper.Play(he, options.LoopCount, options.LoopIntervalMilliseconds, options.GainIntValue, options.FrequencyFactorIntValue);
        }
        catch (ExternalException ex)
        {
            throw new InvalidOperationException("Failed to play.", ex);
        }
    }

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="start">The start position.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="NotSupportedException">Not supported.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(string he, TimeSpan start, VibrationOptions options = null)
    {
        if (string.IsNullOrEmpty(he)) return;
        options ??= new();
        try
        {
            VibrationMotorWrapper.PlaySection(he, options.LoopCount, options.LoopIntervalMilliseconds, options.GainIntValue, options.FrequencyFactorIntValue, GetMilliseconds(start), int.MaxValue);
        }
        catch (ExternalException ex)
        {
            throw new InvalidOperationException("Failed to play.", ex);
        }
    }

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="start">The start position.</param>
    /// <param name="end">The end position.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="NotSupportedException">Not supported.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(string he, TimeSpan start, TimeSpan end, VibrationOptions options = null)
    {
        if (string.IsNullOrEmpty(he)) return;
        options ??= new();
        if (end <= start) return;
        try
        {
            VibrationMotorWrapper.PlaySection(he, options.LoopCount, options.LoopIntervalMilliseconds, options.GainIntValue, options.FrequencyFactorIntValue, GetMilliseconds(start), GetMilliseconds(end));
        }
        catch (ExternalException ex)
        {
            throw new InvalidOperationException("Failed to play.", ex);
        }
    }

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="NotSupportedException">Not supported.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(JsonObjectNode he, VibrationOptions options = null)
        => Play(he?.ToString(), options);

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="start">The start position.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="NotSupportedException">Not supported.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(JsonObjectNode he, TimeSpan start, VibrationOptions options = null)
        => Play(he?.ToString(), start, options);

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="start">The start position.</param>
    /// <param name="end">The end position.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="NotSupportedException">Not supported.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(JsonObjectNode he, TimeSpan start, TimeSpan end, VibrationOptions options = null)
        => Play(he?.ToString(), start, end, options);

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="NotSupportedException">Not supported.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(VibrationDescriptionModel he, VibrationOptions options = null)
        => Play(he?.ToString(), options);

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="start">The start position.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="NotSupportedException">Not supported.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(VibrationDescriptionModel he, TimeSpan start, VibrationOptions options = null)
        => Play(he?.ToString(), start, options);

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="start">The start position.</param>
    /// <param name="end">The end position.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="NotSupportedException">Not supported.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(VibrationDescriptionModel he, TimeSpan start, TimeSpan end, VibrationOptions options = null)
        => Play(he?.ToString(), start, end, options);

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="ArgumentNullException">he is null.</exception>
    /// <exception cref="IOException">Read file failed.</exception>
    /// <exception cref="UnauthorizedAccessException">Unauthorized to access the file.</exception>
    /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="NotSupportedException">The path of filee is in an invalid format or not supported to read.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(FileInfo he, VibrationOptions options = null)
        => Play(GetFileString(he), options);

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="start">The start position.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="ArgumentNullException">he is null.</exception>
    /// <exception cref="IOException">Read file failed.</exception>
    /// <exception cref="UnauthorizedAccessException">Unauthorized to access the file.</exception>
    /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="NotSupportedException">The path of filee is in an invalid format or not supported to read.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(FileInfo he, TimeSpan start, VibrationOptions options = null)
        => Play(GetFileString(he), start, options);

    /// <summary>
    /// Plays a specific HE format content.
    /// </summary>
    /// <param name="he">The HE format content.</param>
    /// <param name="start">The start position.</param>
    /// <param name="end">The end position.</param>
    /// <param name="options">The options.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="ArgumentNullException">he is null.</exception>
    /// <exception cref="IOException">Read file failed.</exception>
    /// <exception cref="UnauthorizedAccessException">Unauthorized to access the file.</exception>
    /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="NotSupportedException">The path of filee is in an invalid format or not supported to read.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Play(FileInfo he, TimeSpan start, TimeSpan end, VibrationOptions options = null)
        => Play(GetFileString(he), start, end, options);

    /// <summary>
    /// Stops current playback.
    /// </summary>
    /// <exception cref="InvalidOperationException">Failed to stop.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void Stop()
    {
        try
        {
            VibrationMotorWrapper.Stop();
        }
        catch (ExternalException ex)
        {
            throw new InvalidOperationException("Failed to stop.", ex);
        }
    }

    /// <summary>
    /// Updates loop parameters.
    /// </summary>
    /// <param name="interval">The loop interval.</param>
    /// <param name="gain">The vibration gain.</param>
    /// <param name="frequencyFactor">The factor of frequency.</param>
    /// <exception cref="InvalidOperationException">Failed to play.</exception>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void UpdateLoopParameters(TimeSpan interval, double gain, double frequencyFactor)
    {
        try
        {
            VibrationMotorWrapper.SendLoopParam(GetMilliseconds(interval), VibrationOptions.GetGainValue(gain), VibrationOptions.GetFrequencyFactorValue(frequencyFactor));
        }
        catch (ExternalException ex)
        {
            throw new InvalidOperationException("Failed to update loop parameters.", ex);
        }
    }

    /// <summary>
    /// Gets all game controllers.
    /// </summary>
    /// <returns>The name list of game controller.</returns>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static List<string> ListGameControllers()
    {
        var s = VibrationMotorWrapper.GetConnectedGameControllers();
        if (string.IsNullOrEmpty(s)) return new();
        var json = JsonObjectNode.TryParse(s);
        return json?.TryGetStringListValue("controllers", true) ?? new();
    }

    /// <summary>
    /// Sets a value indicating whether enable debug log.
    /// </summary>
    /// <param name="enable">true if enable debug log; otherwise, false.</param>
    /// <exception cref="DllNotFoundException">The implementation assembly required does not exist.</exception>
    public static void SetDebugLog(bool enable)
        => VibrationMotorWrapper.DebugLog(enable);

    internal static int GetMilliseconds(TimeSpan span)
    {
        var ms = span.TotalMilliseconds;
        if (ms <= 0) return 0;
        if (ms > int.MaxValue) return int.MaxValue;
        return (int)ms;
    }

    private static void OnGameControllerChange(string data, int size)
        => GameControllerChanged?.Invoke(obj, new EventArgs());

    private static string GetFileString(FileInfo file)
    {
        if (file == null) throw new ArgumentNullException(nameof(file));
        return File.ReadAllText(file.FullName);
    }
}
