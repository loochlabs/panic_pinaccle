using PanicPinnacle.Combatants;
using PanicPinnacle.Legacy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Matches.Legacy;

namespace PanicPinnacle.Combatants.Behaviors.Legacy {
	/// <summary>
	/// Self contained behavior for various states of Player.
	/// </summary>
	public class StateFixedUpdateBehavior : CombatantFixedUpdateBehavior {
		//maintain state of combatant from previous frame
		private CombatantStateType prevConbatantState;
		//particle system in player prefab
		private GameObject dazedParticleObject;
		private GameObject punchingParticleObject;


		public override void Prepare(Combatant combatant) {
			//references for particle systems
			dazedParticleObject = combatant.gameObject.transform.Find("Dazed Particles").gameObject;
			dazedParticleObject.SetActive(false);
			punchingParticleObject = combatant.gameObject.transform.Find("Punching Particles").gameObject;
			punchingParticleObject.SetActive(false);
		}

		public override void FixedUpdate(Combatant combatant) {
			if (combatant.State != prevConbatantState) {
				UpdateState(combatant.State);
			}
			prevConbatantState = combatant.State;
		}





		/// <summary>
		/// Handle updating the current state of combatant.
		/// </summary>
		/// <param name="state">Current state of combatant.</param>
		private void UpdateState(CombatantStateType state) {
			switch (state) {
				case CombatantStateType.dazed:
					//emit smoke particles during dazed state
					dazedParticleObject.SetActive(true);
					//disable all other particle systems
					punchingParticleObject.SetActive(false);

					break;

				case CombatantStateType.playing:
					//disable any particles during played state
					dazedParticleObject.SetActive(false);
					punchingParticleObject.SetActive(false);

					break;

				case CombatantStateType.punching:
					//emit smoke particles during punching state
					punchingParticleObject.SetActive(true);
					//disable all other particle systems
					dazedParticleObject.SetActive(false);

					break;
			}
		}




		#region INSPECTOR JUNK
		private static string behaviorDescription = "Manage the various states of Combatant.";
		protected override string InspectorDescription {
			get {
				return behaviorDescription;
			}
		}
		#endregion
	}

}