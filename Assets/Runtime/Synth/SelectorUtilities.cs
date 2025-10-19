using System;
using JetBrains.Annotations;
using Runtime.Test;

namespace Runtime.Synth
{
	public static class SelectorUtilities
	{
		[CanBeNull]
		public static EnvelopeInstance SelectEnvelope
		(
			EnvelopeInstance env1,
			EnvelopeInstance env2,
			EnvelopeSelection selection
		) => selection switch
		{
			EnvelopeSelection.Off => null,
			EnvelopeSelection.Env1 => env1,
			EnvelopeSelection.Env2 => env2,
			_ => throw new ArgumentOutOfRangeException(nameof(selection), selection, null)
		};

		[CanBeNull]
		public static LfoInstance SelectLfo
		(
			LfoInstance lfo1,
			LfoInstance lfo2,
			LfoSelection selection
		) => selection switch
		{
			LfoSelection.Off => null,
			LfoSelection.Lfo1 => lfo1,
			LfoSelection.Lfo2 => lfo2,
			_ => throw new ArgumentOutOfRangeException(nameof(selection), selection, null)
		};
	}
}