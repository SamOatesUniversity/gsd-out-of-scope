using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Out_of_Scope.Graphics
{
    abstract class GfxEntity
    {
        public virtual void Draw( SpriteBatch spriteBatch, GraphicsDevice device )
        {

        }
    }
}
