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
    private int runSpeed = 3;
    private Random rng;
    private int picktype;

    public Cell(Vector2 pos) : base(pos){
      Quad.S = TextureInfo.TextureSizef/4;
      hasReproduced = false;
      anchor = Position;
      lifespan = 10;

      rng = new Random();
      period = 8;
      //period = ((float)rng.NextDouble()*2)+3;

      picktype = rng.Next(1,3);

      switch(picktype){
      case 1:
        type = CellType.Eye;
        TextureInfo = new TextureInfo( new Texture2D("/Application/assets/bulb.png",false));
        break;
      case 2:
        type = CellType.Rose;
        TextureInfo = new TextureInfo( new Texture2D("/Application/assets/eyebulb.png",false));
        break;
      }
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
        if(type == CellType.Rose){
          for(int i = 0; i < nearby.Count; i++){
            if(nearby[i].type == CellType.Eye){
              return false;
            }
          }
          return true;
        } else if(type == CellType.Eye){
          return false;
        }
      }
      return true;
    }

    public CellType getCellType(Cell c){
      return c.type;
    }

    public int newOffspringCount(List<GameEntity> nearby){
      return 2;
    }

    public void plusWalk (){
      Position += new Vector2(runSpeed+rng.Next(0,5), runSpeed+rng.Next(0,5));
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

