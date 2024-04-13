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
/// The x64 implementation of vibration motor.
/// </summary>
internal static class VibrationMotor64
{
    private const string AssemblyName = "x64\\RichTapWinSDK.dll";

    public class ImplProxy : IVibrationMotorWrapper
    {
        public bool Available()
            => Exists();

        public void Initialize()
            => Init();

        public void RegisterCallback(VibrationMotorCallback cb)
            => SetCallback(cb);

        public void Dispose()
            => Release();

        public void Play(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0)
            => Start(strHE, loop, interval, intensityFactor, freqFactor);

        public void PlaySection(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0, int start = 0, int end = int.MaxValue)
            => StartSection(strHE, loop, interval, intensityFactor, freqFactor, start, end);

        public void Stop()
            => End();

        public void SendLoopParam(int interval, int intensityFactor, int freqFactor)
            => SendLoopParameters(interval, intensityFactor, freqFactor);

        public void SetTrigger(int index, int mode, int amplitude, int frequency, int resistive, int startPosition, int endPosition)
            => SetTriggerMode(index, mode, amplitude, frequency, resistive, startPosition, endPosition);

        public string GameControllers()
        {
            var p = GetConnectedGameControllers();
            return VibrationMotorWrapper.PtrToString(p);
        }

        public bool StrengthGain(int index, int value)
            => SetStrengthGain(index, value);

        public bool SignalConverterState(bool isEnabled)
            => EnableSignalConverter(isEnabled);

        public bool RumbleState(bool isEnabled)
            => EnableRumble(isEnabled);

        public string GetVersion()
        {
            var p = GetVersionName();
            return VibrationMotorWrapper.PtrToString(p);
        }

        public void DebugLog(bool enable)
            => EnableLog(enable);
    }

    public static ImplProxy Instance = new();

    /// <summary>
    /// Tests if the assembly file exists.
    /// </summary>
    /// <returns>true if the assembly file exists; otherwise, false.</returns>
    public static bool Exists()
        => File.Exists(AssemblyName);

    [DllImport(AssemblyName)]
    public extern static void Init();

    [DllImport(AssemblyName, EntryPoint = "RegisterCallback")]
    public extern static void SetCallback(VibrationMotorCallback cb);

    [DllImport(AssemblyName)]
    public extern static void Release();

    [DllImport(AssemblyName, EntryPoint = "Play")]
    public extern static void Start(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0);

    [DllImport(AssemblyName, EntryPoint = "PlaySection")]
    public extern static void StartSection(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0, int start = 0, int end = int.MaxValue);

    [DllImport(AssemblyName, EntryPoint = "Stop")]
    public extern static void End();

    [DllImport(AssemblyName)]
    public extern static void SendLoopParameters(int interval, int intensityFactor, int freqFactor);

    [DllImport(AssemblyName)]
    public extern static void SetTriggerMode(int index, int mode, int amplitude, int frequency, int resistive, int startPosition, int endPosition);

    [DllImport(AssemblyName)]
    public extern static IntPtr GetConnectedGameControllers();

    [DllImport(AssemblyName)]
    public extern static bool SetStrengthGain(int index, int value);

    [DllImport(AssemblyName)]
    public extern static bool EnableSignalConverter(bool enable);

    [DllImport(AssemblyName)]
    public extern static bool EnableRumble(bool enable);

    [DllImport(AssemblyName)]
    public extern static IntPtr GetVersionName();

    [DllImport(AssemblyName)]
    public extern static void EnableLog(bool enable);
}
