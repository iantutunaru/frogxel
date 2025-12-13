using UnityEngine;

/// <summary>
/// Class that handles the movement of the objects from one edge to another. If an object reaches the end edge then it's moved to the start
/// </summary>
public class MoveCycle : MonoBehaviour
{
    // Vector representing the direction movement axis
    public Vector2 direction = Vector2.right;
    // Speed at which the object will move
    public float speed = 1f;
    // Size of the object
    public int size = 1;
    // Left screen border
    [SerializeField] private Vector3 leftEdge;
    // Right screen border
    [SerializeField] private Vector3 rightEdge;
    // Modifier to used to find center of the object
    private float sizeModifier = 2;

    /// <summary>
    /// Move the object either to the left or right and if the object reaches an edge then move it to the opposite edge
    /// </summary>
    private void Update()
    {
        switch (direction.x)
        {
            // Check if object is moving right and at least half of it crossed the right edge
            case > 0 when (transform.position.x - size/sizeModifier) > rightEdge.x:
            {
                var position = transform.position;
                position.x = leftEdge.x - size/sizeModifier;
                transform.position = position;

                // Check if the object is moving left and at least half of it crossed the left edge
                break;
            }
            case < 0 when (transform.position.x + size/sizeModifier)  < leftEdge.x:
            {
                var position = transform.position;
                position.x = rightEdge.x + size/sizeModifier;
                transform.position = position;

                // Move the object in the set direction at the set speed
                break;
            }
            default:
                transform.Translate(direction * (speed * Time.deltaTime));
                break;
        }
    }
}
