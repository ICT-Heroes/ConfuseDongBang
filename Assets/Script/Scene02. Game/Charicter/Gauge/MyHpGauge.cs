using UnityEngine.UI;
using System.Text;

namespace Gauge {
	public class MyHpGauge : BaseGauge {

		public static MyHpGauge instance;

		public Text text;
		public Image gauge;

		void Awake() {
			instance = this;
		}

		public override void Set(int hp, int max) {
			StringBuilder sb = new StringBuilder();
			sb.Append(hp.ToString());
			sb.Append(" / ");
			sb.Append(max.ToString());
			text.text = sb.ToString();
			gauge.fillAmount = ((float)hp / (float)max);
		}

	}
}
