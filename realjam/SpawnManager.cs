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
      limit = 150;
      cells = new List<Cell>();
      SpawnCell(scene.Camera.CalcBounds().Center);
      rng = new Random();
    }

    public void SpawnCell(Vector2 pos){
      Cell sprite = new Cell(pos);
      scene.AddChild(sprite);

      collider.add(sprite);
      cells.Add(sprite);
    }

    public void DestroyCell(Cell c){
      collider.remove(c);
      scene.RemoveChild(c,true);
      cells.Remove(c);
      c.TextureInfo.Dispose();
    }

    public void FrameUpdate(float dt){
      Console.WriteLine("cells count: " + cells.Count);
      for (int i = 0; i < cells.Count; i++){
        Cell c = cells[i];
        c.Tick(dt);
        //do magic & make love babbies
        if (c.getTimeAlive() > c.period && !c.hasReproduced && cells.Count < limit){
          SpawnCell(new Vector2 (c.Position.X+rng.Next(-20,20), c.Position.Y+rng.Next(-20,20)));
          SpawnCell(new Vector2 (c.Position.X+rng.Next(-20,20), c.Position.Y+rng.Next(-20,20)));
          c.hasReproduced = true;
        }
        //Console.WriteLine(counter);
        if (c.getTimeAlive() > c.getLifeSpan()){
          DestroyCell(c);
        }
      }
    }
  }
}

