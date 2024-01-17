using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    public VerticalLayoutGroup verticalLayoutGroup;
    public float scrollSpeed = 10.0f;
    public float stopTime = 0.1f; // Set the stop time in seconds

    private float offset = 0.0f;
    private float timer = 0.0f;
    private Vector2 dragStartPos;

    void Update()
    {
        // Move the offset based on input
        HandleInput();

        // Apply the offset to the vertical layout group
        ApplyOffsetToLayout();

        // Check if the timer exceeds the stop time and reset the offset
        if (timer >= stopTime)
        {
            offset = 0.0f; // Reset the offset
            timer = 0.0f; // Reset the timer
        }
        else
        {
            timer += Time.deltaTime; // Increment the timer
        }
    }

    void HandleInput()
{
    // Keyboard Input
    float keyboardInput = Input.GetAxis("Vertical");
    offset += keyboardInput * scrollSpeed * Time.deltaTime;

    if (keyboardInput != 0.0f)
    {
        timer = 0.0f; // Reset the timer when there is keyboard input
    }

    // Touch Input
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                dragStartPos = touch.position;
                break;

            case TouchPhase.Moved:
                float dragDelta = touch.position.y - dragStartPos.y;
                offset += dragDelta * 0.01f; // Adjust sensitivity as needed

                // Clamp the offset to prevent scrolling left or right
                offset = Mathf.Clamp(offset, 0.0f, float.MaxValue);

                dragStartPos = touch.position;
                timer = 0.0f; // Reset the timer during touch movement
                break;

            case TouchPhase.Ended:
                timer = 0.0f; // Reset the timer when touch ends
                break;
        }
    }
    else
    {
        timer += Time.deltaTime; // Increment the timer when there is no touch input
    }
}


    void ApplyOffsetToLayout()
    {
        // Iterate through the child elements of the vertical layout group
        for (int i = 0; i < verticalLayoutGroup.transform.childCount; i++)
        {
            RectTransform childRectTransform = verticalLayoutGroup.transform.GetChild(i).GetComponent<RectTransform>();

            // Set the vertical position of each child based on the offset
            Vector2 newPosition = new Vector2(childRectTransform.anchoredPosition.x, childRectTransform.anchoredPosition.y - offset);
            childRectTransform.anchoredPosition = newPosition;
        }
    }
}
