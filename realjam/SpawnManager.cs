using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;
using System.Collections.Generic;

namespace realjam {
  public class SpawnManager {

    public Scene scene {get; set;}
    private Cell sprite;
    public List<Cell> cells {get; set;}

    public SpawnManager(Scene scene) {
      cells = new List<Cell>();
      sprite = new Cell(scene.Camera.CalcBounds().Center);

      sprite.CenterSprite();

      scene.AddChild(sprite);
      cells.Add(sprite);
    }

    public void FrameUpdate(float dt){
      foreach (Cell c in cells){
        c.Tick(dt);

      }
    }
  }
}

