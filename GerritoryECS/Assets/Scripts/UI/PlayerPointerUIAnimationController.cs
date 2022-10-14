using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPointerUIAnimationController : MonoBehaviour
{
	[Header("Reference")]
	[SerializeField]
	private Transform m_PointerTipAnchor;

	[SerializeField]
	private TMPro.TextMeshPro m_ScoreText;

	[SerializeField]
	private CanvasGroup m_CanvasGroup;

	[SerializeField]
	private Image m_InnerFillCircle;

	[SerializeField]
	private Image m_OuterRing;

	[Header("Settings")]
	[SerializeField]
	private Color m_NormalRingColor;

	[SerializeField]
	private float m_FollowingHalfLifeTime = 0.2f;

	[Space]
	[SerializeField]
	private float m_ChangeStateAnimationTime = 0.5f;

	[SerializeField]
	private float m_ChangeStateAnimationStartScale = 1.2f;

	[SerializeField]
	private float m_ChangeStateAnimationEndScale = 1.0f;

	[SerializeField]
	private EasingFunction.Ease m_ChangeStateAnimationScaleEasing = EasingFunction.Ease.EaseOutQuad;

	private Transform m_FollowingTransform;

	// Tweener references
	private Tweener m_ChangeStateTweener = null;

	// Update is called once per frame
	void LateUpdate()
	{
		if (m_FollowingTransform == null)
		{
			// No following target, skip the frame.
			return;
		}

		Vector3 targetPosition = m_FollowingTransform.position;
		Vector3 currentPosition = transform.position;

		if ((targetPosition - currentPosition).sqrMagnitude > 0.001f)
		{
			// Use half-life lerp function to interpolate the position.
			Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, 1.0f - Mathf.Pow(0.5f, Time.deltaTime / m_FollowingHalfLifeTime));
			transform.position = newPosition;
		}
		else
		{
			// If the positions are close enough, set it to the target position.
			transform.position = targetPosition;
		}
	}

	/// <summary>
	/// This is useful for solving UI overlapping issue when two players are located at the same position.
	/// </summary>
	/// <param name="value"></param>
	public void SetZValue(float value)
	{
		Vector3 localPosition = m_PointerTipAnchor.localPosition;
		localPosition.z = value;
		m_PointerTipAnchor.localPosition = localPosition;
	}

	public void SnapToTargetTransform()
	{
		Vector3 targetPosition = m_FollowingTransform.position;
		transform.position = targetPosition;
	}

	public void SetFollowingTargetTransform(Transform target)
	{
		m_FollowingTransform = target;
	}

	public void ChangeScoreText(string text)
	{
		m_ScoreText.text = text;
	}

	public void SetScoreTextColor(Color color)
	{
		m_ScoreText.color = color;
	}

	public void PlayChangeStateAnimation(Color ringColor)
	{
		m_PointerTipAnchor.localScale = Vector3.one * m_ChangeStateAnimationStartScale;
		m_OuterRing.color = ringColor;

		Tweener.SafeAbortTweener(ref m_ChangeStateTweener);

		m_ChangeStateTweener = TweenManager.Instance.Tween((float progress) =>
		{
			m_PointerTipAnchor.localScale =
				Vector3.one * 
				EasingFunction.GetEasingFunction(m_ChangeStateAnimationScaleEasing)
				(
					m_ChangeStateAnimationStartScale, 
					m_ChangeStateAnimationEndScale,
					progress
				);

			Color color = Color.Lerp(ringColor, m_NormalRingColor, progress);
			m_OuterRing.color = color;

		})
		.SetTerminateCallback(() =>
		{
			m_ChangeStateTweener = null;
		})
		.SetEase(EasingFunction.Ease.Linear).SetTime(m_ChangeStateAnimationTime);
	}

	public void PlayLeaveStateAnimation()
	{
		m_OuterRing.color = Color.white;

		m_ChangeStateTweener = TweenManager.Instance.Tween((float progress) =>
		{
			Color color = Color.Lerp(Color.white, m_NormalRingColor, progress);
			m_OuterRing.color = color;

		})
		.SetTerminateCallback(() =>
		{
			m_ChangeStateTweener = null;
		})
		.SetEase(EasingFunction.Ease.Linear).SetTime(m_ChangeStateAnimationTime);
	}

	public void UpdateTimerProgress(float progress)
	{
		m_InnerFillCircle.fillAmount = progress;
	}

	public void ChangeTimerColor(Color color)
	{
		m_InnerFillCircle.color = color;
	}

	public void PlayDeadAnimation()
	{
		m_CanvasGroup.alpha = 0.7f;
	}

	public void PlayRespawnAnimation()
	{
		m_CanvasGroup.alpha = 1.0f;
	}

	[ContextMenu("Player Change State Animation (Red)")]
	private void _debugChangeStateAnimation()
	{
		PlayChangeStateAnimation(Color.red);
	}
}
