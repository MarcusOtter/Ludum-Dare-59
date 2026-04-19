using UnityEngine;

// Random Unity bug I don't know why this happens but it only renders like half of the pieces unless you jiggle the layer order?
[RequireComponent(typeof(SpriteRenderer))]
public class LayerOrderJiggle : MonoBehaviour
{
    private float _jiggleTime;
    private SpriteRenderer _spriteRenderer;
    private int _startOrder;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startOrder = _spriteRenderer.sortingOrder;
        // _spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        _jiggleTime = Time.time + 10f;
    }

    private void LateUpdate()
    {
        if (Time.time >= _jiggleTime)
        {
            _spriteRenderer.sortingOrder = _startOrder + 1;
            // _spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
// _spriteRenderer.sortingOrder = _startOrder;
            _jiggleTime = float.MaxValue;
        }
    }
}