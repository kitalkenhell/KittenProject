using UnityEngine;
using System.Collections;

public class ControlsButtonsScale : MonoBehaviour
{
    const float minScale = 0.5f;
    const float maxScale = 2.0f;

    public RectTransform moveLeftButton;
    public RectTransform moveLeftButtonImage;
    public RectTransform moveRightButton;
    public RectTransform moveRightButtonImage;
    public RectTransform jumpButton;
    public RectTransform jumpButtonImage;

    float baseMoveLeftButtonWidth;
    float baseMoveRightButtonWidth;
    float baseMoveRightButtonImagePositionX;
    float baseCommonImageHeight;
    float baseCommonImageWidth;
    float baseCommonImagePositionY;

    void Start()
    {
        baseMoveLeftButtonWidth = moveLeftButton.rect.width;
        baseMoveRightButtonWidth = moveRightButton.rect.width;
        baseCommonImageHeight = moveLeftButtonImage.rect.height;
        baseCommonImageWidth = moveLeftButtonImage.rect.width;
        baseCommonImagePositionY = moveLeftButtonImage.anchoredPosition.y;
        baseMoveRightButtonImagePositionX = moveRightButtonImage.anchoredPosition.x;

        RefreshButtons();
    }

    [ContextMenu("Refresh Buttons")]
    public void RefreshButtons()
    {
        float scale = Mathf.Clamp(PersistentData.UiScale, minScale, maxScale);

        moveLeftButton.SetWidth(baseMoveLeftButtonWidth * scale);

        moveRightButton.SetWidth(baseMoveRightButtonWidth * scale);

        moveLeftButtonImage.SetWidth(baseCommonImageWidth * scale);
        moveLeftButtonImage.SetHeight(baseCommonImageHeight * scale);
        moveLeftButtonImage.SetAnchoredPositionY(baseCommonImagePositionY * scale);

        moveRightButtonImage.SetAnchoredPositionX(baseMoveRightButtonImagePositionX * scale);
        moveRightButtonImage.SetWidth(moveLeftButtonImage.rect.width);
        moveRightButtonImage.SetHeight(moveLeftButtonImage.rect.height);
        moveRightButtonImage.SetAnchoredPositionY(moveLeftButtonImage.anchoredPosition.y);

        jumpButtonImage.SetWidth(moveLeftButtonImage.rect.width);
        jumpButtonImage.SetHeight(moveLeftButtonImage.rect.height);
        jumpButtonImage.SetAnchoredPositionY(moveLeftButtonImage.anchoredPosition.y);

    }
}
