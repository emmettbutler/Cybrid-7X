using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class GameEntity:SpriteUV {

    public DateTime borntime;
    protected float ttime;
    protected TextureInfo texture;

    public GameEntity(Vector2 pos) {
      texture = new TextureInfo( new Texture2D("/Application/assets/eyebulb.png",false));
      TextureInfo = texture;
      Quad.S = texture.TextureSizef/4;

      borntime = DateTime.UtcNow;
      Position = pos;

    }

    public float getTimeAlive(){
      return ttime;
    }

    public virtual void Tick(float dt) {
      TimeSpan t = DateTime.UtcNow - borntime;
      ttime = (float) t.TotalMilliseconds/1000.0f;
    }
  }
}

