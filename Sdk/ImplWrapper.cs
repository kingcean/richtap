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
/// The core implementation interface of vibration motor.
/// </summary>
public interface IVibrationMotorWrapper
{
    /// <summary>
    /// Initializes.
    /// </summary>
    void Init();

    /// <summary>
    /// Sets callback.
    /// </summary>
    /// <param name="cb"></param>
    void SetCallback(VibrationMotorCallback cb);

    /// <summary>
    /// Destroys.
    /// </summary>
    void Destroy();

    /// <summary>
    /// Plays.
    /// </summary>
    /// <param name="strHE">An HE format string.</param>
    /// <param name="loop">The loop number.</param>
    /// <param name="interval">The interval in millisecond.</param>
    /// <param name="intensityFactor">The intensity factor.</param>
    /// <param name="freqFactor">The frequent factor.</param>
    void Play(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0);

    /// <summary>
    /// Plays a specific section.
    /// </summary>
    /// <param name="strHE">An HE format string.</param>
    /// <param name="loop">The loop number.</param>
    /// <param name="interval">The interval in millisecond.</param>
    /// <param name="intensityFactor">The intensity factor.</param>
    /// <param name="freqFactor">The frequent factor.</param>
    /// <param name="start">The start time position in millisecond.</param>
    /// <param name="end">The end time position in millisecond.</param>
    void PlaySection(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0, int start = 0, int end = int.MaxValue);

    /// <summary>
    /// Stops.
    /// </summary>
    void Stop();

    /// <summary>
    /// Sends a loop parameter.
    /// </summary>
    /// <param name="interval">The interval in millisecond.</param>
    /// <param name="intensityFactor">The intensity factor.</param>
    /// <param name="freqFactor">The frequent factor.</param>
    void SendLoopParam(int interval, int intensityFactor, int freqFactor);

    /// <summary>
    /// Sets the trigger.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="mode">The mode.</param>
    /// <param name="amplitude">The amplitude.</param>
    /// <param name="frequency">The frequency.</param>
    /// <param name="resistive">The resistive.</param>
    /// <param name="startPosition">The start time position in millisecond.</param>
    /// <param name="endPosition">The end time position in millisecond.</param>
    void SetTrigger(int index, int mode, int amplitude, int frequency, int resistive, int startPosition, int endPosition);

    /// <summary>
    /// Gets the connected game controllers.
    /// </summary>
    /// <returns>The name.</returns>
    string GetConnectedGameControllers();

    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <returns>The version of the SDK.</returns>
    string GetVersion();

    /// <summary>
    /// Sets a value indicating whether output debug log.
    /// </summary>
    /// <param name="enable">true if enable debug log output; otherwise, false.</param>
    void DebugLog(bool enable);
}

/// <summary>
/// The x64 implementation of vibration motor.
/// </summary>
public static class VibrationMotorWrapper
{
    private static IVibrationMotorWrapper instance;

    /// <summary>
    /// Gets or sets the instance.
    /// </summary>
    public static IVibrationMotorWrapper Instance
    {
        get
        {
#if NET461
            instance ??= Environment.Is64BitOperatingSystem
                ? VibrationMotor64.Instance
                : VibrationMotor32.Instance;
#else
            instance ??= RuntimeInformation.OSArchitecture switch
            {
                Architecture.Arm64 => VibrationMotorArm.Instance,
                Architecture.X86 => VibrationMotor32.Instance,
                _ => VibrationMotor64.Instance
            };
#endif
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    internal static void Init()
        => Instance.Init();

    internal static void SetCallback(VibrationMotorCallback cb)
        => Instance.SetCallback(cb);

    internal static void Destroy()
        => Instance.Destroy();

    internal static void Play(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0)
        => Instance.Play(strHE, loop, interval, intensityFactor, freqFactor);

    internal static void PlaySection(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0, int start = 0, int end = int.MaxValue)
        => Instance.PlaySection(strHE, loop, interval, intensityFactor, freqFactor, start, end);

    internal static void Stop()
        => Instance.Stop();

    internal static void SendLoopParam(int interval, int intensityFactor, int freqFactor)
        => Instance.SendLoopParam(interval, intensityFactor, freqFactor);

    internal static void SetTrigger(int index, int mode, int amplitude, int frequency, int resistive, int startPosition, int endPosition)
        => Instance.SetTrigger(index, mode, amplitude, frequency, resistive, startPosition, endPosition);

    internal static string GetConnectedGameControllers()
        => Instance.GetConnectedGameControllers();

    internal static string GetVersion()
        => Instance.GetVersion();

    internal static void DebugLog(bool enable)
        => Instance.DebugLog(enable);
}
