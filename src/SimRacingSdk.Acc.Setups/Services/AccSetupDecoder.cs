using SimRacingSdk.Acc.Setups.Models;

namespace SimRacingSdk.Acc.Setups.Services;

// Translates a parsed AccSetupFile's raw click indices into real units via the car's AccSetupMap -
// the actual "decode" step, kept separate from SRT's curation (which fields to show, how to round
// them for display, how many ride-height corners are meaningful). Every field ACC's file format
// carries is decoded here, whether or not SRT ends up showing it.
public static class AccSetupDecoder
{
    public static AccDecodedSetup Decode(AccSetupFile setupFile, AccSetupMap setupMap)
    {
        return new AccDecodedSetup
        {
            Tyres = DecodeTyres(setupFile, setupMap),
            Electronics = DecodeElectronics(setupFile.BasicSetup.Electronics, setupMap),
            FuelAndStrategy = DecodeFuelAndStrategy(setupFile.BasicSetup.Strategy, setupMap),
            MechanicalGrip = DecodeMechanicalGrip(setupFile.AdvancedSetup, setupMap),
            Dampers = DecodeDampers(setupFile.AdvancedSetup.Dampers, setupMap),
            Aero = DecodeAero(setupFile.AdvancedSetup.AeroBalance, setupMap)
        };
    }

    private static AccDecodedTyres DecodeTyres(AccSetupFile setupFile, AccSetupMap setupMap)
    {
        var tyres = setupFile.BasicSetup.Tyres;
        var alignment = setupFile.BasicSetup.Alignment;

        return new AccDecodedTyres
        {
            Compound = setupMap.TyreCompound.ValueAt(tyres.TyreCompound),
            PressureFl = setupMap.TyrePressuresFront.ValueAt(tyres.TyrePressure[0]),
            PressureFr = setupMap.TyrePressuresFront.ValueAt(tyres.TyrePressure[1]),
            PressureRl = setupMap.TyrePressuresRear.ValueAt(tyres.TyrePressure[2]),
            PressureRr = setupMap.TyrePressuresRear.ValueAt(tyres.TyrePressure[3]),
            // ACC writes these out pre-computed alongside the raw clicks - more accurate than
            // recomputing via the map, which only captures the adjustable portion, not the
            // car's fixed base geometry.
            CamberFl = Math.Round(alignment.StaticCamber[0], 2),
            CamberFr = Math.Round(alignment.StaticCamber[1], 2),
            CamberRl = Math.Round(alignment.StaticCamber[2], 2),
            CamberRr = Math.Round(alignment.StaticCamber[3], 2),
            ToeMmFl = Math.Round(alignment.ToeOutLinear[0] * 1000, 2),
            ToeMmFr = Math.Round(alignment.ToeOutLinear[1] * 1000, 2),
            ToeMmRl = Math.Round(alignment.ToeOutLinear[2] * 1000, 2),
            ToeMmRr = Math.Round(alignment.ToeOutLinear[3] * 1000, 2),
            CasterLf = setupMap.Caster.ValueAt(alignment.CasterLf),
            CasterRf = setupMap.Caster.ValueAt(alignment.CasterRf),
            SteerRatio = setupMap.SteerRatio.ValueAt(alignment.SteerRatio)
        };
    }

    private static AccDecodedElectronics DecodeElectronics(AccSetupElectronics electronics, AccSetupMap setupMap)
    {
        return new AccDecodedElectronics
        {
            TractionControl1 = setupMap.Tc.ValueAt(electronics.Tc1),
            TractionControl2 = setupMap.Tc2.ValueAt(electronics.Tc2),
            Abs = setupMap.Abs.ValueAt(electronics.Abs),
            EcuMap = setupMap.EcuMap.ValueAt(electronics.EcuMap),
            TelemetryLaps = setupMap.TelemetryLaps.ValueAt(electronics.TelemetryLaps),
            // No calibration entry for fuel mix in AccSetupMap - the raw setup value, unconverted.
            FuelMix = electronics.FuelMix
        };
    }

    private static AccDecodedFuelAndStrategy DecodeFuelAndStrategy(AccSetupStrategy strategy, AccSetupMap setupMap)
    {
        return new AccDecodedFuelAndStrategy
        {
            FuelLitres = setupMap.Fuel.ValueAt(strategy.Fuel),
            FuelPerLap = Math.Round(strategy.FuelPerLap, 2),
            PlannedPitStops = strategy.NPitStops,
            TyreSet = strategy.TyreSet,
            FrontBrakePadCompound = setupMap.BrakeCompoundFront.ValueAt(strategy.FrontBrakePadCompound),
            RearBrakePadCompound = setupMap.BrakeCompoundRear.ValueAt(strategy.RearBrakePadCompound)
        };
    }

    private static AccDecodedMechanicalGrip DecodeMechanicalGrip(AccAdvancedSetup advancedSetup, AccSetupMap setupMap)
    {
        var mechanicalBalance = advancedSetup.MechanicalBalance;

        return new AccDecodedMechanicalGrip
        {
            AntiRollBarFront = setupMap.AntiRollBarFront.ValueAt(mechanicalBalance.ArbFront),
            AntiRollBarRear = setupMap.AntiRollBarRear.ValueAt(mechanicalBalance.ArbRear),
            WheelRateFl = setupMap.WheelRateFront.ValueAt(mechanicalBalance.WheelRate[0]),
            WheelRateFr = setupMap.WheelRateFront.ValueAt(mechanicalBalance.WheelRate[1]),
            WheelRateRl = setupMap.WheelRateRear.ValueAt(mechanicalBalance.WheelRate[2]),
            WheelRateRr = setupMap.WheelRateRear.ValueAt(mechanicalBalance.WheelRate[3]),
            BumpStopRateFl = setupMap.BumpStopRateFront.ValueAt(mechanicalBalance.BumpStopRateUp[0]),
            BumpStopRateFr = setupMap.BumpStopRateFront.ValueAt(mechanicalBalance.BumpStopRateUp[1]),
            BumpStopRateRl = setupMap.BumpStopRateRear.ValueAt(mechanicalBalance.BumpStopRateUp[2]),
            BumpStopRateRr = setupMap.BumpStopRateRear.ValueAt(mechanicalBalance.BumpStopRateUp[3]),
            BumpStopRangeFl = setupMap.BumpStopRangeFront.ValueAt(mechanicalBalance.BumpStopWindow[0]),
            BumpStopRangeFr = setupMap.BumpStopRangeFront.ValueAt(mechanicalBalance.BumpStopWindow[1]),
            BumpStopRangeRl = setupMap.BumpStopRangeRear.ValueAt(mechanicalBalance.BumpStopWindow[2]),
            BumpStopRangeRr = setupMap.BumpStopRangeRear.ValueAt(mechanicalBalance.BumpStopWindow[3]),
            BrakeTorque = mechanicalBalance.BrakeTorque,
            BrakeBiasPercent = setupMap.BrakeBias.ValueAt(mechanicalBalance.BrakeBias),
            DifferentialPreload = setupMap.Preload.ValueAt(advancedSetup.DriveTrain.Preload)
        };
    }

    private static AccDecodedDampers DecodeDampers(AccDampers dampers, AccSetupMap setupMap)
    {
        return new AccDecodedDampers
        {
            BumpSlowFl = setupMap.BumpFront.ValueAt(dampers.BumpSlow[0]),
            BumpSlowFr = setupMap.BumpFront.ValueAt(dampers.BumpSlow[1]),
            BumpSlowRl = setupMap.BumpRear.ValueAt(dampers.BumpSlow[2]),
            BumpSlowRr = setupMap.BumpRear.ValueAt(dampers.BumpSlow[3]),
            BumpFastFl = setupMap.FastBumpFront.ValueAt(dampers.BumpFast[0]),
            BumpFastFr = setupMap.FastBumpFront.ValueAt(dampers.BumpFast[1]),
            BumpFastRl = setupMap.FastBumpRear.ValueAt(dampers.BumpFast[2]),
            BumpFastRr = setupMap.FastBumpRear.ValueAt(dampers.BumpFast[3]),
            ReboundSlowFl = setupMap.ReboundFront.ValueAt(dampers.ReboundSlow[0]),
            ReboundSlowFr = setupMap.ReboundFront.ValueAt(dampers.ReboundSlow[1]),
            ReboundSlowRl = setupMap.ReboundRear.ValueAt(dampers.ReboundSlow[2]),
            ReboundSlowRr = setupMap.ReboundRear.ValueAt(dampers.ReboundSlow[3]),
            ReboundFastFl = setupMap.FastReboundFront.ValueAt(dampers.ReboundFast[0]),
            ReboundFastFr = setupMap.FastReboundFront.ValueAt(dampers.ReboundFast[1]),
            ReboundFastRl = setupMap.FastReboundRear.ValueAt(dampers.ReboundFast[2]),
            ReboundFastRr = setupMap.FastReboundRear.ValueAt(dampers.ReboundFast[3])
        };
    }

    private static AccDecodedAero DecodeAero(AccAeroBalance aeroBalance, AccSetupMap setupMap)
    {
        return new AccDecodedAero
        {
            RideHeightFl = setupMap.RideHeightFront.ValueAt(aeroBalance.RideHeight[0]),
            RideHeightFr = setupMap.RideHeightFront.ValueAt(aeroBalance.RideHeight[1]),
            RideHeightRl = setupMap.RideHeightRear.ValueAt(aeroBalance.RideHeight[2]),
            RideHeightRr = setupMap.RideHeightRear.ValueAt(aeroBalance.RideHeight[3]),
            Splitter = setupMap.Splitter.ValueAt(aeroBalance.Splitter),
            RearWing = setupMap.Wing.ValueAt(aeroBalance.RearWing),
            BrakeDuctFront = setupMap.BrakeDuctsFront.ValueAt(aeroBalance.BrakeDuct[0]),
            BrakeDuctRear = setupMap.BrakeDuctsRear.ValueAt(aeroBalance.BrakeDuct[1]),
            RodLength = aeroBalance.RodLength
        };
    }
}
