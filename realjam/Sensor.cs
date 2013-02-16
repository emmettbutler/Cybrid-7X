using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class Sensor:GameEntity {

    private float radius;
    private Vector2 displacement;
    public List<Cell> collisionCells;
    public SpawnManager s;

    public Sensor(Vector2 pos, SpawnManager s) : base(pos) {
      this.s = s;
      sprite = Support.TiledSpriteFromFile("/Application/assets/eyebulb.png", 1, 1);
      sprite.Position = pos;
      sprite.CenterSprite();
    }
  }
}

