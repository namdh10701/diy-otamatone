using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowButtonHandler : MonoBehaviour
{
    public static UnityEvent<ArrowButton.ButtonDirection> OnButtonClicked = new UnityEvent<ArrowButton.ButtonDirection>();
    public List<Tile> CollidedTiles;
    private void OnEnable()
    {
        OnButtonClicked.AddListener((direction) => OnButton(direction));
    }
    private void OnDisable()
    {
        OnButtonClicked.RemoveListener((direction) => OnButton(direction));
    }

    private void OnButton(ArrowButton.ButtonDirection direction)
    {
        int index = (int)direction;
        int lowestInRow = 10000;
        Tile selectedTile = null;
        for (int i = 0; i < CollidedTiles.Count; i++)
        {
            if (CollidedTiles[i].Col == index)
            {
                if (CollidedTiles[i].Row < lowestInRow)
                {
                    lowestInRow = CollidedTiles[i].Row;
                    selectedTile = CollidedTiles[i];
                }
            }
        }

        if (selectedTile != null)
        {
            Destroy(selectedTile.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            Tile tile = collision.GetComponent<Tile>();
            CollidedTiles.Add(tile);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            Tile tile = collision.GetComponent<Tile>();
            CollidedTiles.Remove(tile);
        }
    }

}
