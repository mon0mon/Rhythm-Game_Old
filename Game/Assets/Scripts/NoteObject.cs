using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    private GameManager _gameManager;
    
    private bool isDeleted = false;
    
    public bool canBePressed;
    public KeyCode KeyToPress;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyToPress))
        {
            if (canBePressed)
            {
                DestroyImmediate(gameObject);
                GameManager.Instance.NoteHit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;
        } else if (other.CompareTag("DeadZone"))
        {
            Debug.Log("Destroied");
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator") && !isDeleted)
        {
            _gameManager.NoteMissed();
            canBePressed = false;
            isDeleted = true;
        }
    }
}
