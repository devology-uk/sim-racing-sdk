namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice.Commands;

// Picking another car saves the one being edited first, which is refused while it has a problem.
public abstract record ChangeCarCommand : VoiceCommand
{
    protected abstract string NoCarReply { get; }

    public override string Execute(VoiceCommandContext context)
    {
        var carBefore = context.Editor.CarGameId;
        if(!this.Select(context.Navigator))
        {
            return this.NoCarReply;
        }

        var problem = context.Editor.Problem();
        if(context.Editor.CarGameId == carBefore && problem is not null)
        {
            return $"Still on {context.Editor.CarName}. It can't be saved: {problem}.";
        }

        var currentCar = context.Navigator.CurrentCar;
        return currentCar is null ? "No car is loaded." : Readback.Car(currentCar);
    }

    protected abstract bool Select(ISetupMapNavigator navigator);
}

public record NextCarCommand : ChangeCarCommand
{
    protected override string NoCarReply => "No more cars in the list.";

    protected override bool Select(ISetupMapNavigator navigator)
    {
        return navigator.SelectNextCar();
    }
}

public record PreviousCarCommand : ChangeCarCommand
{
    protected override string NoCarReply => "No earlier cars in the list.";

    protected override bool Select(ISetupMapNavigator navigator)
    {
        return navigator.SelectPreviousCar();
    }
}

// Saying a name shared by several cars again moves on to the next of them.
public record SelectCarCommand(string CarName) : ChangeCarCommand
{
    protected override string NoCarReply => $"I can't find {this.CarName}.";

    protected override bool Select(ISetupMapNavigator navigator)
    {
        return navigator.SelectCarNamed(this.CarName);
    }
}

public record WhichCarCommand : VoiceCommand
{
    public override string Execute(VoiceCommandContext context)
    {
        var car = context.Navigator.CurrentCar;
        if(car is null)
        {
            return "No car is loaded.";
        }

        var unsaved = context.Editor.HasUnsavedChanges ? " Unsaved changes." : string.Empty;
        return $"{Readback.Car(car)}{unsaved}";
    }
}

public record CopyPreviousCarCommand : VoiceCommand
{
    public override bool ChangesMap => true;

    public override string Execute(VoiceCommandContext context)
    {
        var previousCar = context.Navigator.PreviousCar;
        if(previousCar is null)
        {
            return "There's no previous car to copy.";
        }

        if(!previousCar.HasMap)
        {
            return $"{previousCar.Car.Name} has no saved map to copy.";
        }

        context.Navigator.CopyFrom(previousCar);
        return $"Copied from {previousCar.Car.Name}, {previousCar.Car.Class}. Unvalidated.";
    }
}

public record SaveMapCommand : VoiceCommand
{
    public override string Execute(VoiceCommandContext context)
    {
        var problem = context.Navigator.SaveMap();
        return problem is null ? "Saved." : $"Not saved: {problem}.";
    }
}

public record SetValidatedCommand(bool IsValidated) : VoiceCommand
{
    public override bool ChangesMap => true;

    public override string Execute(VoiceCommandContext context)
    {
        context.Editor.Validated = this.IsValidated;
        return this.IsValidated ? "Validated." : "Not validated.";
    }
}
