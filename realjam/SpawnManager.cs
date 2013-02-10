using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class SpawnManager {

    public Scene scene {get; set;}
    private Cell sprite;

    public SpawnManager(Scene scene) {
      sprite = new Cell();

      sprite.CenterSprite();

      sprite.Position = scene.Camera.CalcBounds().Center;

      scene.AddChild(sprite);
    }

    public void FrameUpdate(float dt){
      sprite.Tick(dt);
    }
  }
}

