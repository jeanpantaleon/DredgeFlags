using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DredgeFlags
{
    public class Flag : ScriptableObject
    {
        public String author;
        public String name;
        public Sprite sprite;
        public Texture2D texture;

        public Flag(String author, String name, Texture2D texture)
        {
            this.author = author;
            this.name = name;
            this.texture = texture;
            this.sprite = GetSprite();
        }

        public Sprite GetSprite()
        {
            var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            return sprite;
        }
    }
}
