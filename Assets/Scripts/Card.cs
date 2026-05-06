using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int id;

    [Header("UI")]
    public Image backImage;
    public Image frontImage;

    [Header("Animator")]
    public Animator animator;

    [Header("Canvas Groups")]
    public CanvasGroup frontCanvasGroup;
    public CanvasGroup backCanvasGroup;

    private GameController controller;
    private bool isRevealed = false;

    public void Setup(int id, Sprite face, Sprite back, GameController controller)
    {
        this.id = id;
        this.controller = controller;

        backImage.sprite = back;
        frontImage.sprite = face;

        frontCanvasGroup.alpha = 0f;
        backCanvasGroup.alpha = 1f;

        transform.localRotation = Quaternion.identity;
        animator.Rebind();
        animator.Update(0f);

        isRevealed = false;
    }

    public void OnClick()
    {
        if (isRevealed) return;
        if (controller.IsInputLocked()) return;

        isRevealed = true;

        animator.SetTrigger("Flip");
        SFXManager.Instance.PlayFlip();
        controller.CardRevealed(this);
    }

    public void Hide()
    {
        isRevealed = false;

        transform.localRotation = Quaternion.identity;

        animator.Play("Idle", 0, 0f);

        animator.Rebind();
        animator.Update(0f);
    }

    public void RevealInstant()
    {
        if (isRevealed) return;

        isRevealed = true;
        animator.SetTrigger("Flip");
    }

    public bool GetIsRevealed()
    {
        return isRevealed;
    }
}