using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private Transform graphicsParent;

    private void Start()
    {
        // On start we need to clone the graphicsParent object and its children
        // and set the layer on that object (and all children) to PuzzlePiece layer

        var clonedGraphics = Instantiate(graphicsParent, transform);
        var puzzleLayer = LayerMask.NameToLayer("PuzzlePiece");

        clonedGraphics.name += " (PuzzleLayer)";
        clonedGraphics.gameObject.layer = puzzleLayer;

        foreach (Transform graphicsChild in clonedGraphics)
        {
            graphicsChild.gameObject.layer = puzzleLayer;
        }

        var renderers = graphicsParent.GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
        {
            r.maskInteraction = SpriteMaskInteraction.None;
        }
    }
}