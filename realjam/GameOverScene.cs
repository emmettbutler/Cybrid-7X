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
      this.AddChild(Support.TiledSpriteFromFile("/Application/assets/robot_sheet2.png", 9, 4));
    }
  }
}

