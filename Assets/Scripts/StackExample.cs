using UnityEngine;
using System.Collections.Generic;

public class StackExample : MonoBehaviour
{
    public GameObject prefab; // The prefab you want to stack
    public int rowCount = 5;   // Number of rows
    public int colCount = 5;   // Number of columns
    public float spacing = 1f; // Spacing between objects

    private Stack<GameObject> objectStack = new Stack<GameObject>();

    void Start()
    {
        CreateStack();
    }

    void CreateStack()
    {
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                // Calculate position based on row and column
                Vector3 position = new Vector3(col * spacing, row * spacing, 0f);

                // Instantiate the prefab at the calculated position
                GameObject obj = Instantiate(prefab, position, Quaternion.identity);

                // Set the parent to this GameObject (optional)
                obj.transform.parent = transform;

                // Push the instantiated object onto the stack
                objectStack.Push(obj);
            }
        }

        // Example: Pop the top object from the stack and do something with it
        if (objectStack.Count > 0)
        {
            GameObject topObject = objectStack.Pop();
            // Do something with the top object, for example, deactivate it
            topObject.SetActive(false);
        }
    }
}