using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class Cell:GameEntity {

    public Vector2 anchor;
    public Boolean hasReproduced {get; set;}
    public float period {get; set;}
    public Boolean grabbed {get; set;}
    private int lifespan;

    public Cell(Vector2 pos) : base(pos){
      Quad.S = TextureInfo.TextureSizef/4;
      hasReproduced = false;
      anchor = Position;
      lifespan = 2;

      Random rng = new Random();
      period = 1;
      //period = ((float)rng.NextDouble()*2)+3;
    }

    public int getLifeSpan(){
      return lifespan;
    }

    public override void CollideTo (GameEntity instance){}

    public override void CollideFrom (GameEntity instance){
      if(instance is Player && grabbed){
      } else if(instance is Cell){
        instance.Position += (instance.Position-Position)*.1f;
      }
    }

    public override void Tick(float dt) {
      base.Tick(dt);
    }
  }
}

