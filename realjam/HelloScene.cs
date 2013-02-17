using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace realjam {
  public class HelloScene:Scene {
    public HelloScene() {
      Camera.SetViewFromViewport();
      
      var bg = Support.TiledSpriteFromFile("/Application/assets/titleScreen.png", 1, 1);
      bg.Position = new Vector2((this.Camera.CalcBounds().Max.X)/2,(Camera.CalcBounds().Max.Y)/2);
      bg.CenterSprite();
      bg.VertexZ = 0;
      this.AddChild(bg,0);
    }

    public void Tick(float dt){
      Console.WriteLine("ticciking");
      if(Input2.GamePad0.Square.Down){
        Console.WriteLine("pressed");
        Scheduler.Instance.Unschedule(this, Tick);
        Director.Instance.ReplaceScene(GameScene.Instance);

        Scheduler.Instance.Schedule(GameScene.Instance,SpawnManager.Instance.FrameUpdate,0.0f,false);
        Scheduler.Instance.Schedule(GameScene.Instance,GameScene.Instance.player.Tick,0.0f,false);
        Scheduler.Instance.Schedule(GameScene.Instance,GameScene.Instance.Tick,0.0f,false);
      }
    }
  }
}

