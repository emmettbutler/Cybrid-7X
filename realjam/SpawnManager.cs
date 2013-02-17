using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;
using System.Collections.Generic;

namespace realjam {
  public class SpawnManager {

    public static SpawnManager Instance;

    public Collider collider {get; set;}
    public List<Cell> cells {get; set;}
    public int limit {get; set;}
    private Random rng;
    public int onScreenCount;

    public SpawnManager() {
      limit = 100;
      cells = new List<Cell>();
      rng = new Random();
    }

    public void setup(){
      SpawnCell(new Vector2(100, 100), 10);
      SpawnCell(new Vector2(300, 100), 10);
      SpawnCell(new Vector2(400, 320), 10);
      SpawnCell(new Vector2(430, 320), 1);
    }

    public void SpawnCell(Vector2 pos, int type){
      Cell sprite = new Cell(pos, type);
      GameScene.Instance.AddChild(sprite.sprite);

      Collider.Instance.add(sprite);
      cells.Add(sprite);
    }

    public void DestroyCell(Cell c){
      Collider.Instance.remove(c);
      GameScene.Instance.RemoveChild(c.sprite,true);
      cells.Remove(c);
      //c.sprite.TextureInfo.Dispose();
    }

    public Boolean cellsOverLimit(){
      return cells.Count >= limit;
    }

    public void RainSpawn(Cell c, List<GameEntity> nearby){
      if (c.shouldSpawn(nearby) && cells.Count < limit){
        List<int> offspringTypes = c.OffspringTypes(nearby);
        int typeCounter = 0;
        for(int i = 0; i < c.newOffspringCount(nearby); i++){
          SpawnCell(new Vector2 (c.sprite.Position.X+rng.Next(-20,20), c.sprite.Position.Y+rng.Next(-20,20)), offspringTypes[typeCounter]);
          if(typeCounter < offspringTypes.Count - 1){
            typeCounter++;
          } else{
            typeCounter = 0;
          }
        }
        c.hasReproduced = true;
      }
    }

    public void NotRainingUpdate(Cell c, List<GameEntity> nearby){
      if(c.watered){
        Console.WriteLine("not raining update");
        if(!c.hasReproduced){
          for(int j = 0; j < nearby.Count; j++){
            Cell e = (Cell)nearby[j];
            if(e.watered){
              Console.WriteLine("watered " + c.type);
              List<int> offspringTypes = c.OffspringTypes(nearby);
              SpawnCell(new Vector2 (c.sprite.Position.X+rng.Next(-20,20), c.sprite.Position.Y+rng.Next(-20,20)), offspringTypes[0]);
              c.hasReproduced = true;
              c.watered = false;
              break;
            }
          }
        } else {
          //Console.WriteLine("has birthd babby");
        }
      }
    }

    public void FrameUpdate(float dt){
      for (int i = 0; i < cells.Count; i++){
        Cell c = cells[i];
        c.Tick(dt);
        List<GameEntity> nearby = new List<GameEntity>();

        for (int j = 0; j < cells.Count; j++){
          Cell d = cells[j];
          if(d == c){
            continue;
          }
          Vector2 displacement = d.sprite.Position - c.sprite.Position;

          if(displacement.LengthSquared() < c.getSquaredEffectRadius()){
            nearby.Add(d);
          }
        }

        if(GameScene.Instance.isRaining){
          RainSpawn(c, nearby);
        } else {
          NotRainingUpdate(c, nearby);
        }

        if (c.shouldDie(nearby)){
          DestroyCell(c);
        }
      }
    }
  }
}

