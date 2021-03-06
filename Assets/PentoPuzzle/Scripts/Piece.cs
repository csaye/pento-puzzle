﻿using UnityEngine;

namespace PentoPuzzle
{
    public class Piece : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private bool halfPivot = false;
        [SerializeField] private Vector2Int[] tiles = null;
        
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer = null;

        private const float scaleFactor = 1.1f;

        public bool HalfPivot
        {
            get { return halfPivot; }
        }

        public Vector2Int[] Tiles
        {
            get { return tiles; }
        }

        private int _rotation = 0;
        private int rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation == value) return;
                else if (value < 0) _rotation = 3;
                else _rotation = value;
                // Rotate
                int zRot = 90 * rotation;
                transform.eulerAngles = new Vector3(0, 0, zRot);
            }
        }

        // Rotates tiles clockwise around pivot
        private void RotateTiles()
        {
            // Rotate tiles
            for (int i = 0; i < tiles.Length; i++)
            {
                Vector2Int tile = tiles[i];
                int tileX = tile.y;
                int tileY = halfPivot ? -tile.x : -tile.x - 1;
                tiles[i] = new Vector2Int(tileX, tileY);
            }
        }

        private bool _flip = false;
        private bool flip
        {
            get { return _flip; }
            set
            {
                if (_flip == value) return;
                else _flip = value;
                // Flip scale
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }

        // Flips tiles
        private void FlipTiles()
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                Vector2Int tile = tiles[i];

                // Flip horizontally
                if (rotation == 0 || rotation == 2)
                {
                    int tileX = halfPivot ? -tile.x : -(tile.x + 1);
                    tiles[i] = new Vector2Int(tileX, tile.y);
                }
                // Flip vertically
                else
                {
                    int tileY = halfPivot ? -tile.y : -(tile.y + 1);
                    tiles[i] = new Vector2Int(tile.x, tileY);
                }
            }
        }

        private Camera mainCamera;

        private bool offsetSet = false;
        private Vector2 mouseOffset;

        private Vector2Int[] startTiles = new Vector2Int[5];
        private Vector2 startPosition;
        private int startRotation;
        private bool startFlip;

        private void Start()
        {
            // Cache main camera reference
            mainCamera = Camera.main;

            // Register piece with manager
            Vector2Int position = halfPivot ? Operation.FloorToInt(transform.position) : Operation.RoundToInt(transform.position);
            PieceManager.instance.InitializePiece(position, tiles);
        }

        // Move to mouse position on drag
        private void OnMouseDrag()
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // Set mouse offset if not set
            if (!offsetSet)
            {
                offsetSet = true;
                mouseOffset = (Vector2)transform.position - mousePosition;
                // Shallow clone tiles
                for (int i = 0; i < tiles.Length; i++) startTiles[i] = tiles[i];
                startPosition = transform.position;
                // Set start rotation and flip
                startRotation = rotation;
                startFlip = flip;

                // Update scale and sorting layer
                transform.localScale = transform.localScale * scaleFactor;
                spriteRenderer.sortingOrder = 1;
            }

            // Rotate
            if (Input.GetKeyDown(KeyCode.R))
            {
                rotation -= 1;
                RotateTiles();
            }
            // Flip
            if (Input.GetKeyDown(KeyCode.F))
            {
                flip = !flip;
                FlipTiles();
            }

            // Round mouse position to snap to grid
            float xRaw = mousePosition.x + mouseOffset.x;
            float x = halfPivot ? Operation.RoundToHalf(xRaw) : Mathf.Round(xRaw);
            float yRaw = mousePosition.y + mouseOffset.y;
            float y = halfPivot ? Operation.RoundToHalf(yRaw) : Mathf.Round(yRaw);
            // Move to rounded mouse position
            transform.position = new Vector2(x, y);
        }

        private void OnMouseUp()
        {
            // Reset mouse offset on mouse up
            offsetSet = false;

            // Update scale and sorting layer
            transform.localScale = transform.localScale / scaleFactor;
            spriteRenderer.sortingOrder = 0;

            // If cannot move piece
            Vector2Int position = halfPivot ? Operation.FloorToInt(transform.position) : Operation.RoundToInt(transform.position);
            Vector2Int startPos = halfPivot ? Operation.FloorToInt(startPosition) : Operation.RoundToInt(startPosition);
            if (!PieceManager.instance.MovePiece(startPos, startTiles, position, tiles))
            {
                // Reset position and tiles
                transform.position = startPosition;
                // Shallow clone tiles
                for (int i = 0; i < tiles.Length; i++) tiles[i] = startTiles[i];
                // Reset rotation and flip
                rotation = startRotation;
                flip = startFlip;
            }
        }
    }
}
