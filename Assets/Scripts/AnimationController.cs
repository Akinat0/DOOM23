using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimationController : MonoBehaviour
{
    [SerializeField] BaseCharacterController characterController;
    [SerializeField] AnimationState state;

    Transform cachedCameraTransform;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        cachedCameraTransform = Camera.main.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateBillboard();
        UpdateDirection();
    }

    void UpdateBillboard()
    {
        Vector3 targetPos = cachedCameraTransform.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
    }

    void UpdateDirection()
    {
        Vector3 targetPos = cachedCameraTransform.position;
        targetPos.y = 0;

        Vector3 sourcePos = characterController.transform.position;
        sourcePos.y = 0;

        float angle = Vector3.SignedAngle((targetPos - sourcePos).normalized, characterController.transform.forward, Vector3.up);

        Sprite sprite = null;

        int direction = Mathf.RoundToInt(angle / 45);

        switch (direction)
        {
            case 0:
                sprite = state.front;
                break;
            case 1:
                sprite = state.frontRight;
                break;
            case 2:
                sprite = state.right;
                break;
            case 3:
                sprite = state.rightBack;
                break;
            case 4:
            case -4:
                sprite = state.back;
                break;
            case -3:
                sprite = state.backLeft;
                break;
            case -2:
                sprite = state.left;
                break;
            case -1:
                sprite = state.frontLeft;
                break;
        }

        spriteRenderer.sprite = sprite;
    }
}

[Serializable]
public struct AnimationState
{
    public Sprite front;
    public Sprite frontLeft;
    public Sprite left;
    public Sprite backLeft;
    public Sprite back;
    public Sprite rightBack;
    public Sprite right;
    public Sprite frontRight;
}
