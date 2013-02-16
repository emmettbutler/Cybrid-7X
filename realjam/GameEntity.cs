using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class GameEntity:SpriteUV {

    public DateTime borntime;
    protected float ttime;
    public SpriteTile sprite {get; set;}

    public GameEntity(Vector2 pos) {
      borntime = DateTime.UtcNow;
      ttime = 0;
      sprite = Support.TiledSpriteFromFile("/Application/assets/robot_sheet2.png", 9, 4);
      sprite.Position = pos;

      this.CenterSprite();
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
      if(sprite != null){
        return sprite.Position;
      } else {
        return Position;
      }
    }

    public virtual float GetRadius(){
      if(sprite != null){
        return (sprite.Quad.X.X > sprite.Quad.Y.Y ? sprite.Quad.Y.Y : sprite.Quad.X.X)/2;
      } else {
        return (Quad.X.X > Quad.Y.Y ? Quad.Y.Y : Quad.X.X)/2;
      }
    }

    public virtual void Tick(float dt) {
      TimeSpan t = DateTime.UtcNow - borntime;
      ttime = (float) t.TotalMilliseconds/1000.0f;
    }
  }
}

