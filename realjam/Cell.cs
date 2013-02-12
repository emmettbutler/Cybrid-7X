using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;
using System.Collections.Generic;

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
      lifespan = 10;

      Random rng = new Random();
      period = 8;
      //period = ((float)rng.NextDouble()*2)+3;
    }

    public float getSquaredEffectRadius(){
      return 30*30;
    }

    public Boolean shouldDie(List<GameEntity> nearby){
      if(ttime > lifespan){
        if(nearby.Count > 4){
          return false;
        }
        return true;
      }
      return false;
    }

    public Boolean shouldSpawn(List<GameEntity> nearby){
      if(ttime % period == 0){
        if(nearby.Count > 4){
          return true;
        }
        return false;
      }
      return true;
    }

    public int newOffspringCount(List<GameEntity> nearby){
      return 2;
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

