using System;

namespace realjam {
  public class HelloScene {
    public HelloScene() {
      var bg = Support.TiledSpriteFromFile("/Application/assets/titleScreen.png", 1, 1);
      bg.Position = new Vector2((Camera.CalcBounds().Max.X)/2,(Camera.CalcBounds().Max.Y)/2);
      bg.CenterSprite();
      bg.VertexZ = 0;
      this.AddChild(bg,0);
    }
  }
}

