using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErrorDiffusionSamplerSys {

	public class Moment {

		#region interface
		#region object
		public override string ToString() {
			return $"<{GetType().Name}: n={Count} average={Average} unbiased variance={UnbiasedVariance}>";
		}
		#endregion

		public int Count { get; protected set; }
		public float Sum { get; protected set; }
		public float Average { get; protected set; }
		public float M2 { get; protected set; }

		public float UnbiasedVariance { get => (Count < 2 ? 0f : M2 / (Count - 1)); }
		public float SD { get => Mathf.Sqrt(UnbiasedVariance); }

		public Moment Add(float value) {
			var prevAvg = Average;

			Count++;
			Sum += value;
			Average += (value - prevAvg) / Count;
			M2 += (value - Average) * (value - prevAvg);
			return this;
		}
		public Moment Add(IEnumerable<float> values) {
			foreach (var v in values) Add(v);
			return this;
        }
		#endregion
	}
}