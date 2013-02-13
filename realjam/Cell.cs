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
    private int lifespan, refreshPeriod;
    private Boolean readyToSpawn;
    private float lastSpawnTime;

    public Cell(Vector2 pos) : base(pos){
      Quad.S = TextureInfo.TextureSizef/4;
      hasReproduced = false;
      anchor = Position;

      Random rng = new Random();
      readyToSpawn = false;

      period = 6;
      lifespan = rng.Next(6, 9);
      refreshPeriod = rng.Next(5, 10);

    }

    public float getSquaredEffectRadius(){
      return 60*60;
    }

    public Boolean shouldDie(List<GameEntity> nearby){
      if(ttime > lifespan){
        if(nearby.Count > 4){
          return false;
        }
        //return true;
      }
      return false;
    }

    public Boolean shouldSpawn(List<GameEntity> nearby){
      if((int)ttime % period == 0 && ttime > period){
        if(nearby.Count >= 1 && readyToSpawn && !hasReproduced){
          lastSpawnTime = ttime;
          return true;
        }
      }
      return false;
    }

    public int newOffspringCount(List<GameEntity> nearby){
      return 1;
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
      if(ttime > period && !readyToSpawn){
        readyToSpawn = true;
      }
      if(System.Math.Abs((ttime - lastSpawnTime) - refreshPeriod) < .001 && hasReproduced){
        Console.WriteLine(ttime);
        hasReproduced = false;
      }
    }
  }
}

