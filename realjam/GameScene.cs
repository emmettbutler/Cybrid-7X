using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace realjam {
  public class GameScene:Scene {

    public Player player {get; set;}
    public SpawnManager spawnmngr {get; set;}
    public Collider collider {get; set;}
    public Boolean isRaining {get; set;}

    public GameScene() {
      Camera.SetViewFromViewport();

      isRaining = false;

      var bg = Support.TiledSpriteFromFile("/Application/assets/Background_Object.png", 1, 1);
      bg.Position = new Vector2((Camera.CalcBounds().Max.X)/2,(Camera.CalcBounds().Max.Y)/2);
      bg.CenterSprite();
      this.AddChild(bg,0);

      collider = new Collider();

      spawnmngr = new SpawnManager(this,collider);

      player = new Player(new Vector2(40,10), spawnmngr);
      AddChild(player.sprite);
      collider.add(player);

      var fencefront = Support.TiledSpriteFromFile("/Application/assets/Fence_Front.png", 1, 1);
      fencefront.Position = new Vector2((Camera.CalcBounds().Max.X)/2,(Camera.CalcBounds().Min.Y+50)/2);
      fencefront.CenterSprite();
      fencefront.VertexZ = 1;
      this.AddChild(fencefront,1);
      Console.WriteLine(fencefront.Position);

      spawnmngr = new SpawnManager(this,collider);

      var goal = new WinSection(new Vector2(Camera.CalcBounds().Max.X-100,Camera.CalcBounds().Max.Y-100),
                                spawnmngr,this);
      AddChild(goal.sprite);
      //scene.AddChild(goal.spriteoverlay);
      collider.add(goal);
      goal.startNewGoal();

      /*var trash = new TrashCan(new Vector2(Camera.CalcBounds().Min.X+60,Camera.CalcBounds().Max.Y-50),spawnmngr);
      AddChild(trash.sprite);
      collider.add(trash);*/
    }
  }
}

