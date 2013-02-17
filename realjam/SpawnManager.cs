using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;
using System.Collections.Generic;

namespace realjam {
  public class SpawnManager {

    public Scene scene {get; set;}
    public Collider collider {get; set;}
    public List<Cell> cells {get; set;}
    public int limit {get; set;}
    private Random rng;
    public int onScreenCount;

    public SpawnManager(Scene scene, Collider collider) {
      this.collider = collider;
      this.scene = scene;
      limit = 100;
      cells = new List<Cell>();
      SpawnCell(new Vector2(100, 100), 10);
      SpawnCell(new Vector2(300, 100), 10);
      SpawnCell(new Vector2(400, 320), 10);
      SpawnCell(new Vector2(430, 320), 1);
      rng = new Random();
    }

    public void SpawnCell(Vector2 pos, int type){
      Cell sprite = new Cell(pos, type);
      scene.AddChild(sprite.sprite);

      collider.add(sprite);
      cells.Add(sprite);
    }

    public void DestroyCell(Cell c){
      collider.remove(c);
      scene.RemoveChild(c.sprite,true);
      cells.Remove(c);
      //c.sprite.TextureInfo.Dispose();
    }

    public Boolean cellsOverLimit(){
      return cells.Count >= limit;
    }

    public void RainSpawn(float dt){
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

        if (c.shouldSpawn(nearby) && cells.Count < limit){
          List<int> offspringTypes = c.OffspringTypes(nearby);
          int typeCounter = 0;
          for(i = 0; i < c.newOffspringCount(nearby); i++){
            SpawnCell(new Vector2 (c.sprite.Position.X+rng.Next(-20,20), c.sprite.Position.Y+rng.Next(-20,20)), offspringTypes[typeCounter]);
            if(typeCounter < offspringTypes.Count - 1){
              typeCounter++;
            } else{
              typeCounter = 0;
            }
          }
          c.hasReproduced = true;
        }

        if (c.shouldDie(nearby)){
          DestroyCell(c);
        }
      }
    }

    public void FrameUpdate(float dt){
      RainSpawn(dt);
    }
  }
}

