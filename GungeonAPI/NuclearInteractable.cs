using System;
using System.Collections;
using UnityEngine;



namespace GungeonAPI
{
	// Token: 0x0200000B RID: 11
	public class NuclearInteractable : SimpleInteractable, IPlayerInteractable
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00005450 File Offset: 0x00003650
		private void Start()
		{
			this.talkPoint = base.transform.Find("talkpoint");
			this.m_isToggled = false;
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
			this.m_canUse = true;
			base.spriteAnimator.Play("idle");
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000054A4 File Offset: 0x000036A4
		public void Interact(PlayerController interactor)
		{
			bool flag = TextBoxManager.HasTextBox(this.talkPoint);
			if (!flag)
			{
				this.m_canUse = ((this.CanUse != null) ? this.CanUse(interactor, base.gameObject) : this.m_canUse);
				bool flag2 = !this.m_canUse;
				if (flag2)
				{
					//TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 2f, "Please come back later", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);/*
					PlayableCharacters characterIdentity = interactor.characterIdentity;
					bool flag3 = characterIdentity == PlayableCharacters.Robot;
					if (flag3)
					{
						TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 2f, "Hey! You're not the right robot get out of here!", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
						
					}
					else
					{
						TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 2f, "The guy who programmed me was to lazy to let you undo this...", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
					}
					
					base.spriteAnimator.PlayForDuration("talk", 2f, "idle", false);
				}
				else
				{
					base.StartCoroutine(this.HandleConversation(interactor));
				}
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005554 File Offset: 0x00003754
		private IEnumerator HandleConversation(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
			base.spriteAnimator.PlayForDuration("talk_start", 1f, "talk", false);
			interactor.SetInputOverride("npcConversation");
			Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
			yield return null;
			int num;
			for (int conversationIndex = this.m_allowMeToIntroduceMyself ? 0 : (this.conversation.Count - 1); conversationIndex < this.conversation.Count - 1; conversationIndex = num + 1)
			{
				//Tools.Print<string>(string.Format("Index: {0}", conversationIndex), "1ee000", false);
				Tools.Print<string>(string.Format("Index: {0}", conversationIndex), "FFFFFF", false);
				TextBoxManager.ClearTextBox(this.talkPoint);
				TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, this.conversation[conversationIndex], interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
				float timer = 0f;
				while (!BraveInput.GetInstanceForPlayer(interactor.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
				{
					timer += BraveTime.DeltaTime;
					yield return null;
				}
				num = conversationIndex;
			}
			this.m_allowMeToIntroduceMyself = false;
			TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, this.conversation[this.conversation.Count - 1], interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
			GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, this.acceptText, this.declineText);
			int selectedResponse = -1;
			while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
			{
				yield return null;
			}
			bool flag = selectedResponse == 0;
			if (flag)
			{
				TextBoxManager.ClearTextBox(this.talkPoint);
				base.spriteAnimator.PlayForDuration("do_effect", 2f, "talk", false);
				while (base.spriteAnimator.CurrentFrame < 20)
				{
					yield return null;
				}
				Action<PlayerController, GameObject> onAccept = OnAccept;
				if (onAccept != null)
				{
					onAccept(interactor, base.gameObject);
				}
				base.spriteAnimator.Play("talk");
				TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 1f, "Have fun", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
				//TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 1f, "", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
				yield return new WaitForSeconds(1f);
			}
			else
			{
				Action<PlayerController, GameObject> onDecline = this.OnDecline;
				if (onDecline != null)
				{
					onDecline(interactor, base.gameObject);
				}
				TextBoxManager.ClearTextBox(this.talkPoint);
			}
			interactor.ClearInputOverride("npcConversation");
			Pixelator.Instance.LerpToLetterbox(1f, 0.25f);
			base.spriteAnimator.Play("idle");
			yield break;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000556A File Offset: 0x0000376A
		public void OnEnteredRange(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005595 File Offset: 0x00003795
		public void OnExitRange(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000055B4 File Offset: 0x000037B4
		public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
		{
			shouldBeFlipped = false;
			return string.Empty;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000055D0 File Offset: 0x000037D0
		public float GetDistanceToPoint(Vector2 point)
		{
			bool flag = base.sprite == null;
			float result;
			if (flag)
			{
				result = 100f;
			}
			else
			{
				Vector3 v = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
				result = Vector2.Distance(point, v) / 1.5f;
			}
			return result;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005630 File Offset: 0x00003830
		public float GetOverrideMaxDistance()
		{
			return -1f;
		}

		// Token: 0x0400002D RID: 45
		private bool m_allowMeToIntroduceMyself = true;
	}
}
