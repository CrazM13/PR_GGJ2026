using Godot;
using System;

namespace BetterUISuite {

	public partial class BetterButton : BaseButton {

		public override void _Ready() {
			base._Ready();

			InitSFX();

		}

		#region SFX

		[ExportGroup("SFX")]
		[Export] private SFXResource clickSFX;
		[Export] private SFXResource hoverSFX;
		[Export] private SFXResource clickDisabledSFX;
		[Export] private SFXResource hoverDisabledSFX;

		private void InitSFX() {
			// Click SFX
			if (clickSFX != null) {
				AudioStreamPlayer audio = UIHelper.AddSFX(this, clickSFX);
				this.Pressed += () => {
					if (!Disabled) audio.Play();
				};
			}

			// Disabled Click SFX
			if (clickDisabledSFX != null) {
				AudioStreamPlayer audio = UIHelper.AddSFX(this, clickDisabledSFX);
				this.Pressed += () => {
					if (Disabled) audio.Play();
				};
			}

			// Hover SFX
			if (hoverSFX != null) {
				AudioStreamPlayer audio = UIHelper.AddSFX(this, hoverSFX);
				this.MouseEntered += () => {
					if (!Disabled) audio.Play();
				};
			}

			// Disabled Hover SFX
			if (hoverDisabledSFX != null) {
				AudioStreamPlayer audio = UIHelper.AddSFX(this, hoverDisabledSFX);
				this.MouseEntered += () => {
					if (Disabled) audio.Play();
				};
			}
		}

		#endregion

	}

}
