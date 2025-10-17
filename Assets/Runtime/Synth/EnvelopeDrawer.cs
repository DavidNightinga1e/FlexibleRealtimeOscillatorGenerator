using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Synth
{
	public class EnvelopeDrawer : MonoBehaviour
	{
		private struct EnvelopeKeys
		{
			public double Time;
			public double Value;

			public EnvelopeKeys(double time, double value)
			{
				Time = time;
				Value = value;
			}
		}

		[SerializeField] private UILineRenderer _lineRenderer;
		[SerializeField] private float plotWidth;
		[SerializeField] private float plotHeight;

		private readonly Vector2[] _points = new Vector2[5];
		private readonly EnvelopeKeys[] _envelopeKeys = new EnvelopeKeys[5];

		public void UpdateEnvelope(double attack, double decay, double sustain, double release)
		{
			UpdateEnvelopeKeyPoints(attack, decay, sustain, release);

			for (int i = 0; i < _points.Length; i++)
			{
				_points[i] = new Vector2
				(
					(float)_envelopeKeys[i].Time * plotWidth,
					(float)_envelopeKeys[i].Value * plotHeight
				);
			}

			_lineRenderer.Points = _points;
		}

		private void UpdateEnvelopeKeyPoints
		(
			double attackTime,
			double decayTime,
			double sustainLevel,
			double releaseTime,
			double sustainWidth = 0.3,
			double totalWidth = 1.0
		)
		{
			// Calculate time proportions
			double totalTime = attackTime + decayTime + sustainWidth + releaseTime;
			double scaleFactor = totalWidth / totalTime;

			double scaledAttack = attackTime * scaleFactor;
			double scaledDecay = decayTime * scaleFactor;
			double scaledSustain = sustainWidth * scaleFactor;
			double scaledRelease = releaseTime * scaleFactor;

			// Calculate key points
			double attackEndTime = scaledAttack;
			double decayEndTime = attackEndTime + scaledDecay;
			double sustainEndTime = decayEndTime + scaledSustain;
			double releaseEndTime = sustainEndTime + scaledRelease;

			_envelopeKeys[0] = new EnvelopeKeys(0, 0);
			_envelopeKeys[1] = new EnvelopeKeys(attackEndTime, 1);
			_envelopeKeys[2] = new EnvelopeKeys(decayEndTime, sustainLevel);
			_envelopeKeys[3] = new EnvelopeKeys(sustainEndTime, sustainLevel);
			_envelopeKeys[4] = new EnvelopeKeys(releaseEndTime, 0);
		}
	}
}