using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace realjam {
  public class GameOverScene:Scene {
    public GameOverScene() {
      Camera.SetViewFromViewport();
      
      SpriteTile overs = Support.TiledSpriteFromFile("/Application/assets/cell_0000000054.png", 1, 1);
      overs.Quad.S = overs.TextureInfo.TextureSizef/2;
      overs.Position = new Vector2(Camera.CalcBounds().Max.X/2, Camera.CalcBounds().Max.Y/2);
      AddChild(overs);
    }
  }
}

