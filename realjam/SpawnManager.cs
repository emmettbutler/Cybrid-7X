using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;
using System.Collections.Generic;

namespace realjam {
  public class SpawnManager {

    public Scene scene {get; set;}
    public List<Cell> cells {get; set;}
    private List<Cell> pending;
    private int counter;

    public SpawnManager(Scene scene) {
      this.scene = scene;
      cells = new List<Cell>();
      pending = new List<Cell>();
      SpawnCell(scene.Camera.CalcBounds().Center);
    }

    public void SpawnCell(Vector2 pos){
      Cell sprite = new Cell(pos);

      sprite.CenterSprite();
      scene.AddChild(sprite);
      pending.Add(sprite);
    }

    public void FrameUpdate(float dt){
      foreach (Cell c in cells){
        c.Tick(dt);

        //do magic & make love babbies

        if (c.getTimeAlive() > c.period && !c.hasReproduced){
          //create new cell & add to pending list
          SpawnCell(new Vector2 (c.Position.X+50, c.Position.Y-50));
          SpawnCell(new Vector2 (c.Position.X-50, c.Position.Y-50));
          c.hasReproduced = true;
          counter += 2;
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

