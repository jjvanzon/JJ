﻿namespace JJ.Business.Synthesizer.Configuration
{
    public enum CalculationEngineConfigurationEnum
    {
        Undefined,
        EntityToCalculatorDirectly,
        EntityThruDtoToCalculator,
        RoslynRuntimeCompilation,
        HardCoded,
        ExampleGeneratedCode
    }
}