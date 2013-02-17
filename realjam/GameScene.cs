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

    public GameScene() {
      Camera.SetViewFromViewport();

      collider = new Collider();

      player = new Player(new Vector2(40,10));
      AddChild(player.sprite);
      collider.add(player);

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

