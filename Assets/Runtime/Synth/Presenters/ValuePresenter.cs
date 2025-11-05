using System;
using UnityEngine;

namespace Runtime.Synth.Presenters
{
	public class ValuePresenter<T> : MonoBehaviour where T : struct
	{
		public event Action<T> ValueChanged;

		public virtual void SetValueWithoutNotify(T value)
		{
		}

		protected void RaiseValueChanged(T value)
		{
			ValueChanged?.Invoke(value);
		}
	}
}