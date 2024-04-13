using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RichTap;

/// <summary>
/// The entire model of vibration description, a.k.a. HE.
/// </summary>
public class VibrationDescriptionModel
{
    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    public VibrationMetadataModel Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the pattern list.
    /// </summary>
    [JsonPropertyName("PatternList")]
    public IList<VibrationPatternListModel> Patterns { get; set; } = new List<VibrationPatternListModel>();

    /// <summary>
    /// Returns a string that represents the current description model.
    /// </summary>
    /// <returns>A string that represents the current description model.</returns>
    public override string ToString()
    {
        try
        {
            return JsonSerializer.Serialize(this);
        }
        catch (JsonException)
        {
        }
        catch (NotSupportedException)
        {
        }
        catch (InvalidOperationException)
        {
        }
        catch (ArgumentException)
        {
        }
        catch (NullReferenceException)
        {
        }
        catch (ExternalException)
        {
        }

        return base.ToString();
    }

    /// <summary>
    /// Writes into a file.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    public void WriteTo(string filePath)
        => File.WriteAllText(filePath, JsonSerializer.Serialize(this));
}

/// <summary>
/// The basic metadata.
/// </summary>
public class VibrationMetadataModel
{
    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Gets or sets the creation date time.
    /// </summary>
    [JsonPropertyName("Created")]
    public string CreationTimeString { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string Description { get; set; }
}

/// <summary>
/// The pattern list model for vibration.
/// </summary>
public class VibrationPatternListModel
{
    /// <summary>
    /// Gets or sets the absolute time.
    /// </summary>
    public int AbsoluteTime { get; set; }

    /// <summary>
    /// Gets or sets the patterns.
    /// </summary>
    public IList<VibrationPatternItemModel> Patterns { get; set; } = new List<VibrationPatternItemModel>();

    /// <summary>
    /// Adds a pattern.
    /// </summary>
    /// <param name="pattern">The pattern item.</param>
    public void AddPattern(VibrationPatternItemModel pattern)
    {
        Patterns ??= new List<VibrationPatternItemModel>();
        Patterns.Add(pattern);
    }

    /// <summary>
    /// Adds a pattern.
    /// </summary>
    /// <param name="eventData">The event model.</param>
    /// <returns>The pattern item model added.</returns>
    public VibrationPatternItemModel AddPattern(VibrationEventModel eventData)
    {
        Patterns ??= new List<VibrationPatternItemModel>();
        var model = new VibrationPatternItemModel()
        {
            EventData = eventData
        };
        Patterns.Add(model);
        return model;
    }

    /// <summary>
    /// Adds a pattern.
    /// </summary>
    /// <param name="type">The vibration type.</param>
    /// <param name="duration">The duration.</param>
    /// <param name="relativeTime">The relative time.</param>
    /// <param name="index">The index.</param>
    /// <param name="parameters">The additional parameters.</param>
    /// <returns>The pattern item model added.</returns>
    public VibrationPatternItemModel AddContinuousPattern(string type, int duration, int relativeTime, int index, VibrationEventParameterModel parameters = null)
        => AddPattern(new VibrationEventModel
        {
            VibrationType = type,
            Duration = duration,
            RelativeTime = relativeTime,
            Index = index,
            Parameters = parameters
        });
}

/// <summary>
/// The pattern item model for vibration.
/// </summary>
public class VibrationPatternItemModel
{
    /// <summary>
    /// Gets or sets the absolute time.
    /// </summary>
    [JsonPropertyName("Event")]
    public VibrationEventModel EventData { get; set; }
}

/// <summary>
/// The event model for vibration.
/// </summary>
public class VibrationEventModel
{
    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    [JsonPropertyName("Type")]
    public string VibrationType { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the relative time.
    /// </summary>
    public int RelativeTime { get; set; }

    /// <summary>
    /// Gets or sets the parameters.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VibrationEventParameterModel Parameters { get; set; }

    /// <summary>
    /// Gets or sets the index.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Returns a string that represents the current description model.
    /// </summary>
    /// <returns>A string that represents the current description model.</returns>
    public override string ToString()
    {
        try
        {
            return JsonSerializer.Serialize(this);
        }
        catch (JsonException)
        {
        }
        catch (NotSupportedException)
        {
        }
        catch (InvalidOperationException)
        {
        }
        catch (ArgumentException)
        {
        }
        catch (NullReferenceException)
        {
        }
        catch (ExternalException)
        {
        }

        return base.ToString();
    }
}

/// <summary>
/// The event parameter model for vibration.
/// </summary>
public class VibrationEventParameterModel
{
    /// <summary>
    /// Gets or sets the intensity.
    /// </summary>
    public int Intensity { get; set; }

    /// <summary>
    /// Gets or sets the frequency.
    /// </summary>
    public int Frequency { get; set; }

    /// <summary>
    /// Gets or sets the index.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<VibrationCurvePointModel> Curve { get; set; }
}

/// <summary>
/// The event parameter model for vibration.
/// </summary>
public class VibrationCurvePointModel
{
    /// <summary>
    /// Gets or sets the time.
    /// </summary>
    public int Time { get; set; }

    /// <summary>
    /// Gets or sets the intensity.
    /// </summary>
    public double Intensity { get; set; }

    /// <summary>
    /// Gets or sets the frequency.
    /// </summary>
    public int Frequency { get; set; }
}
