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

internal delegate void VibrationMotorCallback(string data, int size);

/// <summary>
/// The options of vibration.
/// </summary>
public class VibrationOptions
{
    /// <summary>
    /// Gets or sets the count of loop playback.
    /// The current one is not included.
    /// </summary>
    public int LoopCount { get; set; }

    /// <summary>
    /// Gets or sets the interval for loop playback.
    /// </summary>
    public TimeSpan LoopInterval { get; set; }

    /// <summary>
    /// Gets or sets the vibration gain.
    /// -1d is the weakest; 0d is normal; 1d is the strongest.
    /// </summary>
    public double Gain { get; set; }

    /// <summary>
    /// Gets or sets the factor of frequency.
    /// -1d is the weakest; 0d is normal; 1d is the strongest.
    /// </summary>
    public double FrequencyFactor { get; set; }

    internal int LoopIntervalMilliseconds => VibrationMotor.GetMilliseconds(LoopInterval);

    internal int GainIntValue => GetGainValue(Gain);

    internal int FrequencyFactorIntValue => GetFrequencyFactorValue(FrequencyFactor);

    internal static int GetGainValue(double value)
    {
        if (value == 0) return 0;
        if (value > 0) return (int)Math.Round(value * 256) + 255;
        value += 1;
        if (value < 0) value = 0;
        return (int)Math.Round(value * 255);
    }

    internal static int GetFrequencyFactorValue(double value)
    {
        if (value < -1) value = -1;
        else if (value > 1) value = 1;
        return (int)Math.Round(value * 100);
    }
}
