using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class Cell:SpriteUV {
    public Cell() {
      var texture = new TextureInfo( new Texture2D("/Application/assets/eyebulb.png",false));
      TextureInfo = texture;
      Quad.S = texture.TextureSizef;
    }

    public virtual void Tick(float dt) {
      Position = new Vector2(Position.X+1,Position.Y-1);
    }
  }
}

