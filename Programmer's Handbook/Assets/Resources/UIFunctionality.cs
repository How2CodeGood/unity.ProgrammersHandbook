using UnityEngine;
using UnityEngine.UI;

public class UIFunctionality : MonoBehaviour
{
    void Start()
    {
        // Add ScrollRect component
        ScrollRect scrollRect = gameObject.AddComponent<ScrollRect>();

        // Add the Mask component to the ScrollRect
        Mask mask = gameObject.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        // Create a new GameObject for the content
        GameObject contentGO = new GameObject("Content");
        RectTransform contentRectTransform = contentGO.AddComponent<RectTransform>();
        contentRectTransform.SetParent(transform, false);

        // Add the Vertical Layout Group component to the content GameObject
        VerticalLayoutGroup contentVerticalLayout = contentGO.AddComponent<VerticalLayoutGroup>();
        contentVerticalLayout.childControlWidth = true;
        contentVerticalLayout.childControlHeight = true;

        // Add ContentSizeFitter to enable scrolling
        ContentSizeFitter contentSizeFitter = contentGO.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Set the content for the ScrollRect
        scrollRect.content = contentRectTransform;

        // Set up the ScrollRect properties
        scrollRect.viewport = GetComponent<RectTransform>();
        scrollRect.horizontal = false;
        scrollRect.vertical = true;

        // Add a Scrollbar and assign it to the Scroll Rect
        GameObject scrollbarGO = new GameObject("Scrollbar");
        Scrollbar scrollbar = scrollbarGO.AddComponent<Scrollbar>();
        scrollbarGO.transform.SetParent(transform, false);
        scrollbarGO.transform.SetAsLastSibling(); // Ensure it's rendered on top

        // Set up the Scrollbar properties
        scrollbar.direction = Scrollbar.Direction.BottomToTop;
        scrollbar.value = 1; // Start at the bottom

        // Associate the Scrollbar with the ScrollRect
        scrollRect.verticalScrollbar = scrollbar;
        scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        Debug.Log("Completed");
    }
}
