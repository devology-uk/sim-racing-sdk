using System.Globalization;
using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice.Commands;

public record ReadTargetCommand(VoiceTarget Target) : VoiceCommand
{
    public override string Execute(VoiceCommandContext context)
    {
        return Readback.Target(this.Target, context.Editor);
    }
}

public record SetAvailabilityCommand(VoiceTarget Target, R3eSetupControlAvailability Availability) : VoiceCommand
{
    public override bool ChangesMap => true;

    public override string Execute(VoiceCommandContext context)
    {
        foreach(var row in context.RowsOf(this.Target))
        {
            row.Availability = this.Availability;
        }

        return Readback.Target(this.Target, context.Editor);
    }
}

// A range makes a row adjustable. With no step spoken, the row keeps its step, or steps by 1 if it has none.
public record SetRangeCommand(VoiceTarget Target, double Min, double Max, double? Step) : VoiceCommand
{
    private const double DefaultStep = 1;

    public override bool ChangesMap => true;

    public override string Execute(VoiceCommandContext context)
    {
        var rows = context.RowsOf(this.Target).Where(row => row.TakesRange).ToList();
        if(rows.Count == 0)
        {
            return $"{this.Target.Description} takes options, not a range.";
        }

        foreach(var row in rows)
        {
            var step = this.Step ?? row.Range?.Step ?? DefaultStep;
            row.SetRange(new SetupRange(this.Min, step, this.Max));
        }

        return Readback.Target(this.Target, context.Editor);
    }
}

public record SetGearCountCommand(VoiceTarget Target, int GearCount) : VoiceCommand
{
    public override bool ChangesMap => true;

    public override string Execute(VoiceCommandContext context)
    {
        var gearRows = context.RowsOf(this.Target).Where(row => row.TakesGearPairs).ToList();
        if(gearRows.Count == 0)
        {
            return $"For {this.Target.Description}, say a range, like 1 to 10 step 1.";
        }

        var fittedRows = gearRows.Where(row => row.CanHaveGearCount).ToList();
        if(fittedRows.Count == 0)
        {
            return "Gears are not fitted. Say gears fixed or gears adjustable first.";
        }

        foreach(var row in fittedRows)
        {
            row.GearCountText = this.GearCount.ToString(CultureInfo.InvariantCulture);
        }

        return $"{this.GearCount} gears.";
    }
}

// The game steps from the shortest gear to the tallest, so a pair that isn't taller than the one before is
// probably a misread.
public record AddGearPairCommand(VoiceTarget Target, int DrivingTeeth, int DrivenTeeth) : VoiceCommand
{
    private const int RatioDecimals = 3;

    public override bool ChangesMap => true;

    public override string Execute(VoiceCommandContext context)
    {
        var row = context.RowsOf(this.Target).FirstOrDefault(candidate => candidate.TakesGearPairs);
        if(row is null)
        {
            return $"{this.Target.Description} doesn't take tooth pairs.";
        }

        if(!row.CanHaveOptions)
        {
            return "Gears are not fitted. Say gears fixed or gears adjustable first.";
        }

        var pair = $"{this.DrivingTeeth}:{this.DrivenTeeth}";
        if(row.Options.Contains(pair))
        {
            return $"{this.DrivingTeeth} to {this.DrivenTeeth} is already listed.";
        }

        var previousRatio = row.Options.Select(RatioOf).LastOrDefault();
        row.SetOptions([..row.Options, pair]);
        return this.Reply(row, previousRatio);
    }

    private static double? RatioOf(string pair)
    {
        var teeth = pair.Split(':');
        if(teeth.Length != 2
           || !double.TryParse(teeth[0], CultureInfo.InvariantCulture, out var driving)
           || !double.TryParse(teeth[1], CultureInfo.InvariantCulture, out var driven)
           || driving <= 0)
        {
            return null;
        }

        return driven / driving;
    }

    private string Reply(SetupControlRowViewModel row, double? previousRatio)
    {
        var ratio = (double)this.DrivenTeeth / this.DrivingTeeth;
        var reply = $"{this.DrivingTeeth} to {this.DrivenTeeth}, ratio {Readback.Number(Math.Round(ratio, RatioDecimals))}, pair {row.Options.Count}";
        if(previousRatio is not null && ratio >= previousRatio)
        {
            reply += ". Warning, that's not taller than the pair before";
        }

        return $"{reply}.";
    }
}

public record AddOptionCommand(VoiceTarget Target, string Option) : VoiceCommand
{
    public override bool ChangesMap => true;

    public override string Execute(VoiceCommandContext context)
    {
        var rows = context.RowsOf(this.Target).Where(row => row.TakesOptions).ToList();
        if(rows.Count == 0)
        {
            return $"{this.Target.Description} doesn't take options.";
        }

        var fittedRows = rows.Where(row => row.CanHaveOptions).ToList();
        if(fittedRows.Count == 0)
        {
            return $"{this.Target.Description} is not fitted. Say fixed or adjustable first.";
        }

        if(fittedRows.All(row => row.Options.Contains(this.Option)))
        {
            return $"{this.Option} is already listed.";
        }

        foreach(var row in fittedRows.Where(row => !row.Options.Contains(this.Option)))
        {
            row.SetOptions([..row.Options, this.Option]);
        }

        return $"{this.Option} added, {fittedRows[0].Options.Count} options.";
    }
}

public record SetOptionsCommand(VoiceTarget Target, IReadOnlyList<string> Options) : VoiceCommand
{
    public override bool ChangesMap => true;

    public override string Execute(VoiceCommandContext context)
    {
        var rows = context.RowsOf(this.Target).Where(row => row.TakesOptions && row.CanHaveOptions).ToList();
        if(rows.Count == 0)
        {
            return $"{this.Target.Description} can't take options. Say fixed or adjustable first.";
        }

        foreach(var row in rows)
        {
            row.SetOptions(this.Options);
        }

        return Readback.Target(this.Target, context.Editor);
    }
}

public record ClearOptionsCommand(VoiceTarget Target) : VoiceCommand
{
    public override bool ChangesMap => true;

    public override string Execute(VoiceCommandContext context)
    {
        foreach(var row in context.RowsOf(this.Target).Where(row => row.TakesOptions || row.TakesGearPairs))
        {
            row.SetOptions([]);
        }

        return $"{this.Target.Description} options cleared.";
    }
}
