using UnityEngine;
using UnityEngine.Events;

namespace Battle_System
{
    public class ClickableSprite : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public Color defaultColor = Color.white;
        public Color hoverColor = Color.yellow;
        public Color clickColor = Color.red;
        
        public UnityEvent OnClick;

        private void OnMouseEnter()
        {
            spriteRenderer.color = hoverColor;
        }

        private void OnMouseExit()
        {
            spriteRenderer.color = defaultColor;
        }

        private void OnMouseDown()
        {
            spriteRenderer.color = clickColor;
            OnClick?.Invoke();
            // Add logic for what happens when the sprite is clicked
        }
    }
}