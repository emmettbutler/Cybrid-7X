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

    public virtual void CollideTo(GameEntity instance){
      throw new NotImplementedException();
    }

    public virtual void CollideFrom(GameEntity instance){
      throw new NotImplementedException();
    }

    public virtual Vector2 GetCenter(){
      return Position;
    }

    public virtual float GetRadius(){
      //TODO - get the actual radius
      return 10;
    }

    public virtual void Tick(float dt) {
      TimeSpan t = DateTime.UtcNow - borntime;
      ttime = (float) t.TotalMilliseconds/1000.0f;
    }
  }
}

