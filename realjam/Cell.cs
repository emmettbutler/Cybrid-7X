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
    private string spritePath;
    public Boolean destroyed {get; set;}
    public Boolean watered {get; set;}
    public int type {get; set;}

    public Cell(Vector2 pos, int type) : base(pos){

      Random rng = new Random();
      readyToSpawn = false;
      period = 4;
      destroyed = false;
      lifespan = rng.Next(6, 9);
      refreshPeriod = rng.Next(5, 10);

      hasReproduced = false;
      anchor = sprite.Position;

      this.type = type;
      spritePath = "/Application/assets/cell_";
      spritePath += this.type.ToString("D10") + ".png";

      sprite = Support.TiledSpriteFromFile(spritePath, 1, 1);
      sprite.Position = pos;
      sprite.CenterSprite();
    }

    public float getSquaredEffectRadius(){
      return 60*60;
    }

    public Boolean shouldDie(List<GameEntity> nearby){
      if(destroyed == true){
        return true;
      }

      if(ttime > lifespan){
        if(nearby.Count > 4){
          return false;
        }
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

    public List<int> OffspringTypes(List<GameEntity> nearby){
      List<int> types = new List<int>();
      int result = 0;

      for(int i = 0; i < nearby.Count; i++){
        Cell c = (Cell)nearby[i];
        result += c.type;
      }

      result = clampTypeLevels(result);
      if(result != 0){
        types.Add(result);
      }

      return types;
    }

    public static int clampTypeLevels(int type){
      int[] limits = {7,7,0,0,0,0,0,0,0,0};
      int[] levels = new int[10];
      int input = type;
      int divisor = 1000000000;
      int result = 0;
      for(int i = 9; i >= 0; i--){
        levels[i] = input/divisor;
        input %= divisor;
        divisor /= 10;
      }

      divisor = 1;

      for(int i = 0; i < 10; i++){
        if(levels[i] > limits[i]){
          levels[i] = limits[i];
        }
        result += divisor*levels[i];
        divisor *= 10;
      }
      return result;
    }

    public int newOffspringCount(List<GameEntity> nearby){
      return 1;
    }

    public override void CollideTo (GameEntity instance){}

    public override void CollideFrom (GameEntity instance){
      if(instance is Player && grabbed){
      } else if(instance is Cell && !grabbed){
        instance.sprite.Position += (instance.sprite.Position-sprite.Position)*.1f;
      }
    }

    public override void Tick(float dt) {
      base.Tick(dt);
      if(ttime > period && !readyToSpawn){
        readyToSpawn = true;
      }
      if(System.Math.Abs((ttime - lastSpawnTime) - refreshPeriod) < .001 && hasReproduced){
        hasReproduced = false;
      }
    }
  }
}

