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
    public class ImplProxy : IVibrationMotorWrapper
    {
        public bool Available()
            => Exists();

        public void Init()
            => RichTapInit();

        public void SetCallback(VibrationMotorCallback cb)
            => RichTapSetCallback(cb);

        public void Destroy()
            => RichTapDestroy();

        public void Play(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0)
            => RichTapPlay(strHE, loop, interval, intensityFactor, freqFactor);

        public void PlaySection(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0, int start = 0, int end = int.MaxValue)
            => RichTapPlaySection(strHE, loop, interval, intensityFactor, freqFactor, start, end);

        public void Stop()
            => RichTapStop();

        public void SendLoopParam(int interval, int intensityFactor, int freqFactor)
            => RichTapSendLoopParam(interval, intensityFactor, freqFactor);

        public void SetTrigger(int index, int mode, int amplitude, int frequency, int resistive, int startPosition, int endPosition)
            => RichTapSetTrigger(index, mode, amplitude, frequency, resistive, startPosition, endPosition);

        public string GetConnectedGameControllers()
            => RichTapGetConnectedGameControllers();

        public string GetVersion()
            => RichTapGetVersion();

        public void DebugLog(bool enable)
            => EnableDebugLog(enable);
    }

    private const string AssemblyName = "x64\\RichTapWinSDK.dll";

    public static ImplProxy Instance = new();

    /// <summary>
    /// Tests if the assembly file exists.
    /// </summary>
    /// <returns>true if the assembly file exists; otherwise, false.</returns>
    public static bool Exists()
        => File.Exists(AssemblyName);

    [DllImport(AssemblyName)]
    public extern static void RichTapInit();

    [DllImport(AssemblyName)]
    public extern static void RichTapSetCallback(VibrationMotorCallback cb);

    [DllImport(AssemblyName)]
    public extern static void RichTapDestroy();

    [DllImport(AssemblyName)]
    public extern static void RichTapPlay(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0);

    [DllImport(AssemblyName)]
    public extern static void RichTapPlaySection(string strHE, int loop = 0, int interval = 0, int intensityFactor = 255, int freqFactor = 0, int start = 0, int end = int.MaxValue);

    [DllImport(AssemblyName)]
    public extern static void RichTapStop();

    [DllImport(AssemblyName)]
    public extern static void RichTapSendLoopParam(int interval, int intensityFactor, int freqFactor);

    [DllImport(AssemblyName)]
    public extern static void RichTapSetTrigger(int index, int mode, int amplitude, int frequency, int resistive, int startPosition, int endPosition);

    [DllImport(AssemblyName)]
    public extern static string RichTapGetConnectedGameControllers();

    [DllImport(AssemblyName)]
    public extern static string RichTapGetVersion();

    [DllImport(AssemblyName)]
    public extern static void EnableDebugLog(bool enable);
}
