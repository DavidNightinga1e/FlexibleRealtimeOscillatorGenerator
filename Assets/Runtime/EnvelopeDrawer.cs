using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
	public class EnvelopeDrawer : MonoBehaviour
	{
		[SerializeField] private UILineRenderer _lineRenderer;
		[SerializeField] private float plotWidth;
		[SerializeField] private float plotHeight;
		[SerializeField] private EnvelopeParameters _envelopeParameters;

		[ContextMenu("Draw Envelope")]
		private void Draw()
		{
			var result = GetADSREnvelopeKeyPoints(
				_envelopeParameters.Attack, 
				_envelopeParameters.Decay,
				_envelopeParameters.Sustain, 
				_envelopeParameters.Release);

			Vector2[] points = new Vector2[5];
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = new Vector2(
					plotWidth * (float)result[i].time, 
					(float)result[i].value * plotHeight);
			}
			
			_lineRenderer.Points = points;
		}
		
		public static List<(double time, double value)> GetADSREnvelopeKeyPoints(
			double attackTime, 
			double decayTime, 
			double sustainLevel, 
			double releaseTime,
			double sustainWidth = 0.3,
			double totalWidth = 1.0)
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
        
			var keyPoints = new List<(double time, double value)>();
			keyPoints.Add((0, 0));
			keyPoints.Add((attackEndTime, 1.0));
			keyPoints.Add((decayEndTime, sustainLevel));
			keyPoints.Add((sustainEndTime, sustainLevel));
			keyPoints.Add((releaseEndTime, 0));
        
			return keyPoints;
		}
	}
}