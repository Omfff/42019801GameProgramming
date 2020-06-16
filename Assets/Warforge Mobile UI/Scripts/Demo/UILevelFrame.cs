using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	/// <summary>
	/// Level frame is an extension of Toggle with a slight change to keep the toggle in the group even when disabled.
	/// </summary>
	public class UILevelFrame : Toggle {
	
		protected override void OnDisable()
		{
			base.OnDisable();
			
			if (base.group != null)
				base.group.RegisterToggle(this);
		}
	}
}