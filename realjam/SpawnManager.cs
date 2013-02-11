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
    private List<Cell> pending;
    private Random rng;

    public SpawnManager(Scene scene, Collider collider) {
      this.collider = collider;
      this.scene = scene;
      limit = 150;
      cells = new List<Cell>();
      pending = new List<Cell>();
      SpawnCell(scene.Camera.CalcBounds().Center);
      rng = new Random();
    }

    public void SpawnCell(Vector2 pos){
      Cell sprite = new Cell(pos);

      collider.add(sprite);
      scene.AddChild(sprite);
      pending.Add(sprite);
    }

    public void FrameUpdate(float dt){
      foreach (Cell c in cells){
        c.Tick(dt);

        //do magic & make love babbies

        if (c.getTimeAlive() > c.period && !c.hasReproduced && cells.Count < limit){
          //create new cell & add to pending list
          SpawnCell(new Vector2 (c.Position.X+rng.Next(-20,20), c.Position.Y+rng.Next(-20,20)));
          SpawnCell(new Vector2 (c.Position.X+rng.Next(-20,20), c.Position.Y+rng.Next(-20,20)));
          c.hasReproduced = true;
        }
        //Console.WriteLine(counter);
      }
      foreach (Cell c in pending){
        //add the pending cells to the main list
        cells.Add(c);
      }
      pending.Clear();
    }
  }
}

