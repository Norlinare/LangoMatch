using UnityEngine;

public class DragableCardScript : MonoBehaviour
{
    private Vector3 offset; // The offset between the object and the mouse position
    private float clickHoldThreshold = 0.2f; // Time threshold to distinguish quick click from hold
    private float mouseDownTime = 0f; // Time when the mouse button was pressed down
    private bool isMouseDown = false; // Whether the mouse button is currently held down

    bool isFlipped;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFlipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for mouse click and handle it
        HandleMouseClick();
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // 0 for left mouse button
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                mouseDownTime = Time.time;
                isMouseDown = true;

                offset = transform.position - (Vector3)mousePos;
            }
        }

        if (Input.GetMouseButton(0)) // While the mouse button is held down
        {
            // Check if it's a click-and-hold (longer than the threshold)
            if (isMouseDown && Time.time - mouseDownTime > clickHoldThreshold)
            {
                // Handle click and hold action (e.g., dragging, showing a menu)
                OnCardHold();

                // Move the object with the mouse (apply offset)
                MoveCardWithMouse();
            }
        }

        if (Input.GetMouseButtonUp(0)) // 0 for left mouse button
        {
            if (isMouseDown)
            {
                // Check for a quick click (before the threshold)
                if (Time.time - mouseDownTime <= clickHoldThreshold)
                {
                    // Perform action for quick click (e.g., selecting the card)
                    OnCardClick();
                }
            }
            isMouseDown = false; // Reset the flag when the mouse button is released
        }
    }

    private void OnCardClick()
    {
        // Action to perform on quick click (e.g., select the card)
        Debug.Log("Card clicked!");
        FlipCard();
    }

    private void OnCardHold()
    {
        // Action to perform on click-and-hold (e.g., dragging or showing extra info)
        Debug.Log($"Card held! for {clickHoldThreshold} seconds");
    }

    private void MoveCardWithMouse()
    {
        // Get the current mouse position and move the card to follow it
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Keep the z-axis value the same (no movement in 3D space)

        // Move the card to the mouse position adjusted by the initial offset
        transform.position = mousePos + offset;
    }




    //Probably for use later

    // Example: Method to flip the card (you can replace this with any action you want)
    private void FlipCard()
    {

        if (isFlipped)
        {

            isFlipped = false;
            ChangeColorOfChild("LanguageCardBase", Color.green);

        }
        else if (!isFlipped)
        {

            isFlipped = true;
            ChangeColorOfChild("LanguageCardBase", Color.red);
        }

    }

    // Method to change the color of a child object's sprite
    public void ChangeColorOfChild(string childName, Color newColor)
    {
        // Find the child by name
        Transform childTransform = transform.Find(childName);

        if (childTransform != null)
        {
            // Get the SpriteRenderer component from the child
            SpriteRenderer spriteRenderer = childTransform.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // Change the color to the new color passed as a parameter
                spriteRenderer.color = newColor;
            }
            else
            {
                Debug.Log("SpriteRenderer not found on child.");
            }
        }
        else
        {
            Debug.Log("Child not found");
        }
    }
}
