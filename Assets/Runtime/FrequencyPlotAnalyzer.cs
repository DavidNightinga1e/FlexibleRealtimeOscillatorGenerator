using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class FrequencyPlotAnalyzer
{
	private readonly int fftSize;
	private readonly int sampleRate;
	private readonly float[] samplesBuffer;
	private readonly Complex[] fftBuffer;
	private readonly float[] window;
	private int sampleIndex;

	// For amplitude tracking
	private readonly float windowSum;
	private float referenceAmplitude;

	public FrequencyPlotAnalyzer(int fftSize = 4096, int sampleRate = 44100, float referenceAmplitude = 1.0f)
	{
		if ((fftSize & (fftSize - 1)) != 0)
			throw new ArgumentException("FFT size must be a power of 2");

		this.fftSize = fftSize;
		this.sampleRate = sampleRate;
		this.samplesBuffer = new float[fftSize];
		this.fftBuffer = new Complex[fftSize];
		this.window = CreateHannWindow(fftSize);
		this.sampleIndex = 0;
		this.referenceAmplitude = referenceAmplitude;

		// Calculate window sum for amplitude compensation
		this.windowSum = window.Sum();
	}

	public Vector2[] ProcessSample(float sample)
	{
		samplesBuffer[sampleIndex] = sample;
		sampleIndex++;

		if (sampleIndex >= fftSize)
		{
			sampleIndex = 0;
			return GenerateFrequencyPlot();
		}

		return null;
	}

	/// <summary>
	/// Set the reference amplitude for normalization (default 1.0 = full scale)
	/// </summary>
	public void SetReferenceAmplitude(float amplitude)
	{
		referenceAmplitude = amplitude;
	}

	/// <summary>
	/// Get the RMS amplitude of the current buffer
	/// </summary>
	public float GetCurrentAmplitude()
	{
		double sumSquares = 0;
		for (int i = 0; i < fftSize; i++)
		{
			sumSquares += samplesBuffer[i] * samplesBuffer[i];
		}

		return (float)Math.Sqrt(sumSquares / fftSize);
	}

	/// <summary>
	/// Get the peak amplitude of the current buffer
	/// </summary>
	public float GetPeakAmplitude()
	{
		float peak = 0;
		for (int i = 0; i < fftSize; i++)
		{
			float absValue = Math.Abs(samplesBuffer[i]);
			if (absValue > peak)
				peak = absValue;
		}

		return peak;
	}

	private Vector2[] GenerateFrequencyPlot()
	{
		// Calculate input signal amplitude before windowing
		float inputRMS = GetCurrentAmplitude();
		float inputPeak = GetPeakAmplitude();

		// Apply window function
		for (int i = 0; i < fftSize; i++)
		{
			fftBuffer[i] = new Complex(samplesBuffer[i] * window[i], 0);
		}

		// Perform FFT
		FFT(fftBuffer);

		// Calculate magnitudes with proper amplitude scaling
		return CreateAmplitudeAwareSpectrum(inputRMS, inputPeak);
	}

	private Vector2[] CreateAmplitudeAwareSpectrum(float inputRMS, float inputPeak)
	{
		var points = new List<Vector2>();
		int usefulBins = fftSize / 2;

		// Calculate amplitude scaling factor
		// For a sine wave with amplitude A, the FFT magnitude should be A * N / 2
		// where N is the FFT size, but we need to account for windowing
		float windowCompensation = 2.0f / windowSum; // Compensate for window reduction
		float expectedPeakMagnitude = referenceAmplitude * fftSize * 0.5f * windowCompensation;

		// Find actual peak magnitude in frequency domain
		float maxMagnitude = 0f;
		float[] magnitudes = new float[usefulBins - 1];

		for (int i = 1; i < usefulBins; i++)
		{
			double real = fftBuffer[i].Real;
			double imag = fftBuffer[i].Imaginary;
			double magnitude = Math.Sqrt(real * real + imag * imag);

			// Apply window compensation and scaling
			magnitude *= windowCompensation;

			magnitudes[i - 1] = (float)magnitude;
			if (magnitude > maxMagnitude)
				maxMagnitude = (float)magnitude;
		}

		// Calculate amplitude relationship 
		float amplitudeRatio = maxMagnitude / expectedPeakMagnitude;

		// Create points with consistent amplitude scaling
		double minFreq = 20.0;
		double maxFreq = sampleRate / 2.0;
		double logMin = Math.Log10(minFreq);
		double logMax = Math.Log10(maxFreq);

		for (int i = 0; i < magnitudes.Length; i++)
		{
			// Calculate actual frequency
			double freq = (double)(i + 1) * sampleRate / fftSize;
			if (freq < minFreq) continue;

			// Convert to logarithmic frequency scale
			double logFreq = Math.Log10(freq);
			float normalizedFreq = (float)((logFreq - logMin) / (logMax - logMin));
			normalizedFreq = Math.Max(0, Math.Min(1, normalizedFreq));

			// Normalize magnitude relative to reference amplitude
			float normalizedMagnitude = magnitudes[i] / expectedPeakMagnitude;

			// Convert to dB scale
			float magnitudeDB = 20 * MathF.Log10(normalizedMagnitude + 1e-10f);

			// Normalize dB to 0-1 range (-96dB to 0dB)
			float normalizedDB = Math.Max(0, Math.Min(1, (magnitudeDB + 96f) / 96f));

			points.Add(new Vector2(normalizedFreq, normalizedDB));
		}

		// Output amplitude information for debugging
		Console.WriteLine($"Input: RMS={inputRMS:F3}, Peak={inputPeak:F3}, " +
		                  $"FFT Peak={maxMagnitude:F1}, Ratio={amplitudeRatio:F3}");

		return points.ToArray();
	}

	private void FFT(Complex[] buffer)
	{
		int n = buffer.Length;

		// Bit-reversal permutation
		for (int i = 1, j = 0; i < n; i++)
		{
			int bit = n >> 1;
			for (; (j & bit) != 0; bit >>= 1)
			{
				j ^= bit;
			}

			j ^= bit;

			if (i < j)
			{
				var temp = buffer[i];
				buffer[i] = buffer[j];
				buffer[j] = temp;
			}
		}

		// Cooley-Tukey FFT
		for (int len = 2; len <= n; len <<= 1)
		{
			double angle = -2.0 * Math.PI / len;
			Complex wlen = new Complex(Math.Cos(angle), Math.Sin(angle));

			for (int i = 0; i < n; i += len)
			{
				Complex w = new Complex(1, 0);
				for (int j = 0; j < len / 2; j++)
				{
					Complex u = buffer[i + j];
					Complex v = buffer[i + j + len / 2] * w;

					buffer[i + j] = u + v;
					buffer[i + j + len / 2] = u - v;
					w *= wlen;
				}
			}
		}
	}

	private float[] CreateHannWindow(int size)
	{
		float[] window = new float[size];
		for (int i = 0; i < size; i++)
		{
			window[i] = (float)(0.5 * (1 - Math.Cos(2 * Math.PI * i / (size - 1))));
		}

		return window;
	}

	public void Reset()
	{
		sampleIndex = 0;
		Array.Clear(samplesBuffer, 0, samplesBuffer.Length);
	}
}