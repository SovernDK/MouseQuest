using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HandOfCardsUI : MonoBehaviour
{
    [SerializeField] private float maxRotationAngle = 30f; // Maximum rotation angle for the outermost cards
    [SerializeField] private float spacingX = 50f; // Spacing between the cards
    [SerializeField] private float spacingY = 50f; // Spacing between the cards

    private RectTransform[] childTransforms;

    void Start()
    {
        // Get all top-level children that have an Image component
        childTransforms = new RectTransform[transform.childCount];
        int validChildCount = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i) as RectTransform;
            if (child != null && child.GetComponent<Image>() != null)
            {
                childTransforms[validChildCount] = child;
                validChildCount++;
            }
        }

        // Resize the array to only include valid children
        System.Array.Resize(ref childTransforms, validChildCount);

        RotateHand();
    }

    [Button("Rotate")]
    void RotateHand()
    {
        if (childTransforms == null || childTransforms.Length == 0)
        {
            Debug.LogWarning("No UI Image children found to rotate.");
            return;
        }

        int childCount = childTransforms.Length;
        float angleStep = maxRotationAngle * 2 / (childCount - 1); // Calculate the angle step between each card

        for (int i = 0; i < childCount; i++)
        {
            RectTransform child = childTransforms[i];

            // Calculate the rotation angle for this card
            float angle = -maxRotationAngle + angleStep * i;

            // Apply the rotation
            child.localRotation = Quaternion.Euler(0, 0, angle);

            // Adjust the position to create a fan effect
            float offsetX = spacingX * i - (spacingX * (childCount - 1)) / 2f;
            float offsetY = Mathf.Abs(spacingY * i - (spacingY * (childCount - 1)) / 2f);
            child.localPosition = new Vector3(offsetX, -offsetY, 0);
        }
    }

    // Optionally, you can call this method to update the hand dynamically
    public void UpdateHand()
    {
        RotateHand();
    }
}