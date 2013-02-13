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

    public SpawnManager(Scene scene, Collider collider) {
      this.collider = collider;
      this.scene = scene;
      limit = 100;
      cells = new List<Cell>();
      SpawnCell(new Vector2(100, 100));
      SpawnCell(new Vector2(300, 100));
      SpawnCell(new Vector2(400, 320));
      SpawnCell(new Vector2(430, 320));
      rng = new Random();
    }

    public void SpawnCell(Vector2 pos){
      Cell sprite = new Cell(pos);
      scene.AddChild(sprite.sprite);

      collider.add(sprite);
      cells.Add(sprite);
    }

    public void DestroyCell(Cell c){
      collider.remove(c);
      scene.RemoveChild(c.sprite,true);
      cells.Remove(c);
      c.TextureInfo.Dispose();
    }

    public void FrameUpdate(float dt){
      //Console.WriteLine("cells count: " + cells.Count);
      for (int i = 0; i < cells.Count; i++){
        Cell c = cells[i];
        c.Tick(dt);
        //do magic & make love babbies

        List<GameEntity> nearby = new List<GameEntity>();

        for (int j = 0; j < cells.Count; j++){
          Cell d = cells[j];
          if(d == c){
            continue;
          }
          Vector2 displacement = d.Position - c.Position;

          if(displacement.LengthSquared() < c.getSquaredEffectRadius()){
            nearby.Add(d);
          }
          //Console.WriteLine("nearby count: " + nearby.Count);
        }
        if (c.shouldSpawn(nearby) && cells.Count < limit){
          Console.WriteLine(i + ": " + c.newOffspringCount(nearby));
          for(i = 0; i < c.newOffspringCount(nearby); i++){
            SpawnCell(new Vector2 (c.Position.X+rng.Next(-20,20), c.Position.Y+rng.Next(-20,20)));
          }
          c.hasReproduced = true;
        }
        //Console.WriteLine(counter);
        if (c.shouldDie(nearby)){
          DestroyCell(c);
        }
      }
    }
  }
}

