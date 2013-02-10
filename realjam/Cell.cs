using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class Cell:GameEntity {

    public Vector2 anchor;
    public Boolean hasReproduced {get; set;}
    public float period {get; set;}

    public Cell(Vector2 pos) : base(pos){
      Quad.S = texture.TextureSizef/4;
      hasReproduced = false;
      anchor = Position;

      Random rng = new Random();
      period = ((float)rng.NextDouble()*2)+3;

    }

    public override void Tick(float dt) {
      base.Tick(dt);
      Position = new Vector2(anchor.X+20*(float)System.Math.Sin(ttime*10),anchor.Y);
    }
  }
}

