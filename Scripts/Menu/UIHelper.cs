using Godot;
using System;

namespace BetterUISuite {
	public static class UIHelper {

		public static AudioStreamPlayer AddSFX(Control node, int audioBus, AudioStream sfx) {
			AudioStreamPlayer audio = new() {
				Stream = sfx,
				Bus = AudioServer.GetBusName(audioBus)
			};

			node.AddChild(audio, false, Node.InternalMode.Back);

			return audio;
		}

		public static AudioStreamPlayer AddSFX(Control node, StringName audioBus, AudioStream sfx) {
			AudioStreamPlayer audio = new() {
				Stream = sfx,
				Bus = audioBus
			};

			node.AddChild(audio, false, Node.InternalMode.Back);

			return audio;
		}

		public static AudioStreamPlayer AddSFX(Control node, SFXResource sfx) {
			AudioStreamPlayer audio = new() {
				Stream = sfx.SFX,
				Bus = sfx.AudioBus
			};

			node.AddChild(audio, false, Node.InternalMode.Back);

			return audio;
		}

	}
}
