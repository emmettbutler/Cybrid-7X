using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class Cell:SpriteUV {

    public DateTime borntime;
    public Vector2 anchor;

    public Cell(Vector2 pos) {
      var texture = new TextureInfo( new Texture2D("/Application/assets/eyebulb.png",false));
      TextureInfo = texture;
      Quad.S = texture.TextureSizef;

      borntime = DateTime.UtcNow;
      Position = pos;
      anchor = Position;

    }

    public virtual void Tick(float dt) {
      TimeSpan t = DateTime.UtcNow - borntime;
      float ttime = (float) t.TotalMilliseconds/1000.0f;
      Position = new Vector2(anchor.X+20*(float)System.Math.Sin(ttime*10),anchor.Y);
    }
  }
}

