using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trivial.Text;

namespace RichTap;

/// <summary>
/// The wrapper callback of vibration motor.
/// </summary>
/// <param name="data">The data.</param>
/// <param name="size">The size.</param>
public delegate void VibrationMotorCallback(string data, int size);

/// <summary>
/// The core implementation interface of vibration motor.
/// </summary>
public interface IVibrationMotorWrapper
{
    /// <summary>
    /// Tests if the assembly file exists.
    /// </summary>
    /// <returns>true if the assembly file exists; otherwise, false.</returns>
    public bool Available();

    /// <summary>
    /// Initializes.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Sets callback.
    /// </summary>
    /// <param name="cb">The callback.</param>
    void RegisterCallback(VibrationMotorCallback cb);

    /// <summary>
    /// Dispose.
    /// </summary>
    void Dispose();

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
    /// Gets the information in JSON object about connected game controllers.
    /// </summary>
    /// <returns>The name.</returns>
    string GameControllers();

    /// <summary>
    /// Set the gain of strength to hardware.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value</param>
    bool StrengthGain(int index, int value);

    /// <summary>
    /// Gets or sets the signal-convert on controller.
    /// </summary>
    /// <param name="isEnabled">true if enable; otherwise, false.</param>
    /// <returns></returns>
    bool SignalConverterState(bool isEnabled);

    /// <summary>
    /// Gets or sets the rumble effect on controller.
    /// </summary>
    /// <param name="isEnabled">true if enable; otherwise, false.</param>
    /// <returns></returns>
    bool RumbleState(bool isEnabled);

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
    internal static IVibrationMotorWrapper Instance
    {
        get
        {
            if (instance != null) return instance;
#if NET461
            IVibrationMotorWrapper obj = Environment.Is64BitOperatingSystem
                ? VibrationMotor64.Instance
                : VibrationMotor32.Instance;
#else
            IVibrationMotorWrapper obj = RuntimeInformation.OSArchitecture switch
            {
                Architecture.Arm64 => VibrationMotorArm.Instance,
                Architecture.X86 => VibrationMotor32.Instance,
                _ => VibrationMotor64.Instance
            };
#endif
            if (!obj.Available() && VibrationMotorLocal.Instance.Available()) obj = VibrationMotorLocal.Instance;
            instance = obj;
            return obj;
        }

        set
        {
            instance = value;
        }
    }

    /// <summary>
    /// Sets by x64 wrapper instance to override.
    /// </summary>
    public static void UseX64()
        => instance = VibrationMotor64.Instance;

    /// <summary>
    /// Sets by x86 wrapper instance to override.
    /// </summary>
    public static void UseX86()
        => instance = VibrationMotor32.Instance;

    /// <summary>
    /// Sets by arm64 wrapper instance to override.
    /// </summary>
    public static void UseArm64()
        => instance = VibrationMotorArm.Instance;

    /// <summary>
    /// Sets by local wrapper instance to override.
    /// </summary>
    public static void UseLocal()
        => instance = VibrationMotorLocal.Instance;

    /// <summary>
    /// Sets by a specific wrapper instance to override.
    /// </summary>
    public static void Use(IVibrationMotorWrapper wrapper)
        => instance = wrapper;

    internal static void Init()
        => Instance.Initialize();

    internal static void RegisterCallback(VibrationMotorCallback cb)
        => Instance.RegisterCallback(cb);

    internal static void Destroy()
        => Instance.Dispose();

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

    internal static bool StrengthGain(int index, int value)
        => Instance.StrengthGain(index, value);

    internal static bool SignalConverterState(bool isEnabled)
        => Instance.SignalConverterState(isEnabled);

    internal static bool RumbleState(bool isEnabled)
        => Instance.RumbleState(isEnabled);

    internal static string GameControllers()
        => Instance.GameControllers();

    internal static string GetVersion()
        => Instance.GetVersion();

    internal static void DebugLog(bool enable)
        => Instance.DebugLog(enable);

    internal static string PtrToString(IntPtr p)
        => Marshal.PtrToStringAnsi(p);
}
