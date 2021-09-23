using System;
using UnityEngine;

namespace Models
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Sprite _bombedSprite;

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log(this.gameObject + " collision " + other.gameObject);
        }
    }
}